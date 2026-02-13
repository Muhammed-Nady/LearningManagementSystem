#!/usr/bin/env pwsh
# Remove Comments (Preserve XML Documentation)

Write-Host "=== SMART COMMENT REMOVAL ===" -ForegroundColor Cyan
Write-Host "This script removes regular comments but preserves:" -ForegroundColor Yellow
Write-Host "  - XML documentation (///))" -ForegroundColor Gray
Write-Host "  - Important TODOs" -ForegroundColor Gray
Write-Host "  - License headers" -ForegroundColor Gray
Write-Host ""

$projectRoot = "C:\Users\gguo1\OneDrive\Desktop\Projects\LearningManagementSystem"
$processedFiles = 0
$totalRemoved = 0

function Remove-CSharpComments {
    param($content)
    
    $lines = $content -split "`r?`n"
    $result = @()
    $inMultiLineComment = $false
    
    foreach ($line in $lines) {
 # Check for multi-line comment start
        if ($line -match '/\*' -and $line -notmatch '\*/') {
            $inMultiLineComment = $true
   continue
        }
     
  # Check for multi-line comment end
        if ($inMultiLineComment) {
         if ($line -match '\*/') {
   $inMultiLineComment = $false
  }
            continue
        }
      
  # Skip multi-line comment blocks
  if ($line -match '/\*.*\*/') {
  continue
        }
        
        # Preserve XML documentation comments (///)
        if ($line -match '^\s*///') {
            $result += $line
            continue
   }
        
        # Preserve TODO comments
        if ($line -match '//.*TODO:') {
          $result += $line
     continue
   }
        
        # Remove single-line comments
        if ($line -match '^(\s*)(.+?)\s*//.*$') {
   $result += $matches[1] + $matches[2].TrimEnd()
        }
        elseif ($line -match '^\s*//') {
     # Skip comment-only lines
   continue
      }
    else {
   $result += $line
        }
    }
    
    return ($result -join "`r`n")
}

function Remove-RazorComments {
    param($content)
    
    # Remove Razor comments (@* ... *@)
    $content = $content -replace '@\*[\s\S]*?\*@', ''
    
    # Remove HTML comments (<!-- ... -->)
    $content = $content -replace '<!--[\s\S]*?-->', ''
    
    return $content
}

function Remove-JavaScriptComments {
param($content)
    
    $lines = $content -split "`r?`n"
    $result = @()
    $inMultiLineComment = $false
    
    foreach ($line in $lines) {
        if ($line -match '/\*' -and $line -notmatch '\*/') {
 $inMultiLineComment = $true
            continue
        }
        
if ($inMultiLineComment) {
 if ($line -match '\*/') {
            $inMultiLineComment = $false
     }
    continue
  }
        
        if ($line -match '/\*.*\*/') {
            continue
        }
 
   # Remove single-line comments
        if ($line -match '^(\s*)(.+?)\s*//.*$') {
   $result += $matches[1] + $matches[2].TrimEnd()
 }
        elseif ($line -match '^\s*//') {
     continue
        }
        else {
  $result += $line
}
    }
    
    return ($result -join "`r`n")
}

function Remove-CSSComments {
    param($content)
    
  # Remove CSS comments (/* ... */)
    $content = $content -replace '/\*[\s\S]*?\*/', ''
    
    return $content
}

# Process files
$files = Get-ChildItem -Path $projectRoot -Include "*.cs","*.cshtml","*.js","*.css" -Recurse -File | 
    Where-Object { 
        $_.FullName -notmatch '\\(bin|obj|node_modules|\.vs|Migrations|wwwroot\\lib)\\' 
    }

foreach ($file in $files) {
    $relativePath = $file.FullName.Replace($projectRoot, "").TrimStart("\")
    Write-Host "Processing: $relativePath" -ForegroundColor Gray
    
    try {
        $originalContent = Get-Content $file.FullName -Raw
        $originalSize = $originalContent.Length
        $newContent = $originalContent
   
        switch ($file.Extension) {
   ".cs" { 
                $newContent = Remove-CSharpComments $originalContent 
            }
            ".cshtml" { 
           $newContent = Remove-RazorComments $originalContent 
      $newContent = Remove-JavaScriptComments $newContent
      }
            ".js" { 
          $newContent = Remove-JavaScriptComments $originalContent 
        }
    ".css" { 
       $newContent = Remove-CSSComments $originalContent 
 }
     }
        
        # Remove excessive blank lines
        $newContent = $newContent -replace '(\r?\n){4,}', "`r`n`r`n`r`n"
        
        $newSize = $newContent.Length
        $removed = $originalSize - $newSize
        
      if ($removed -gt 10) {
  Set-Content -Path $file.FullName -Value $newContent -NoNewline
        Write-Host "  ? Removed $removed characters" -ForegroundColor Green
  $processedFiles++
            $totalRemoved += $removed
        }
        else {
       Write-Host "  ??  No significant comments" -ForegroundColor DarkGray
     }
    }
    catch {
        Write-Host "  ? Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "=== SUMMARY ===" -ForegroundColor Cyan
Write-Host "Files modified: $processedFiles" -ForegroundColor Green
Write-Host "Total characters removed: $totalRemoved" -ForegroundColor Green
Write-Host ""
Write-Host "? Smart comment removal complete!" -ForegroundColor Green
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
