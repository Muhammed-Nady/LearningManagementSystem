#!/usr/bin/env pwsh
# Safe Comment Removal for LMS Project
# This preserves XML docs and important markers while removing inline comments

$projectRoot = "C:\Users\gguo1\OneDrive\Desktop\Projects\LearningManagementSystem"
$excludeDirs = @("bin", "obj", "node_modules", ".vs", "Migrations", "wwwroot\lib", ".git")
$processedCount = 0

Write-Host "?? Starting Safe Comment Removal..." -ForegroundColor Cyan
Write-Host ""

function Remove-CSharpInlineComments {
    param([string]$content)
    
    $lines = $content -split "`r?`n"
    $cleaned = @()
    $inMultiComment = $false
    
    foreach ($line in $lines) {
     # Handle multi-line comments
    if ($line -match '/\*' -and $line -notmatch '\*/') {
   $inMultiComment = $true
     continue
        }
        if ($inMultiComment) {
            if ($line -match '\*/') { $inMultiComment = $false }
            continue
        }
        
        # Preserve XML documentation
        if ($line -match '^\s*///') {
            $cleaned += $line
 continue
        }
      
        # Remove inline comments but keep code
        if ($line -match '^(\s*)(.+?)\s*//\s*(.*)$') {
       $code = $matches[2].TrimEnd()
   $comment = $matches[3]
         
            # Keep TODO/FIXME/HACK comments
       if ($comment -match '^\s*(TODO|FIXME|HACK|NOTE|IMPORTANT)') {
       $cleaned += $line
    } else {
      $cleaned += $matches[1] + $code
     }
   continue
        }
        
    # Skip comment-only lines (except XML docs)
if ($line -match '^\s*//') {
       continue
      }
        
        $cleaned += $line
    }
    
    return ($cleaned -join "`r`n")
}

# Get all C# files
$files = Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse | Where-Object {
    $exclude = $false
    foreach ($dir in $excludeDirs) {
        if ($_.FullName -like "*\$dir\*") {
          $exclude = $true
  break
        }
    }
    -not $exclude
}

foreach ($file in $files) {
    $relativePath = $file.FullName.Replace("$projectRoot\", "")
    
    try {
$original = Get-Content $file.FullName -Raw
   $cleaned = Remove-CSharpInlineComments $original
 
        # Remove excessive blank lines
        $cleaned = $cleaned -replace '(\r?\n){4,}', "`r`n`r`n`r`n"
  
        if ($original -ne $cleaned) {
   $saved = $original.Length - $cleaned.Length
if ($saved -gt 50) {
           Set-Content -Path $file.FullName -Value $cleaned -NoNewline
                Write-Host "? $relativePath (-$saved chars)" -ForegroundColor Green
    $processedCount++
            }
        }
    }
    catch {
        Write-Host "? $relativePath : $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "? Processed $processedCount files" -ForegroundColor Green
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
