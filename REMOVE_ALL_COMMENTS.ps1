#!/usr/bin/env pwsh
# Remove All Comments from Project Files

Write-Host "=== REMOVING COMMENTS FROM PROJECT ===" -ForegroundColor Cyan
Write-Host ""

$projectRoot = "C:\Users\gguo1\OneDrive\Desktop\Projects\LearningManagementSystem"
$totalFiles = 0
$processedFiles = 0

# File extensions to process
$fileExtensions = @("*.cs", "*.cshtml", "*.js", "*.css", "*.sql")

# Directories to exclude
$excludeDirs = @("bin", "obj", "node_modules", ".vs", "Migrations", "wwwroot\lib")

Write-Host "Scanning project files..." -ForegroundColor Yellow
Write-Host ""

foreach ($ext in $fileExtensions) {
    $files = Get-ChildItem -Path $projectRoot -Filter $ext -Recurse -File | Where-Object {
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
   $totalFiles++
        $relativePath = $file.FullName.Replace($projectRoot, "").TrimStart("\")
        
        Write-Host "Processing: $relativePath" -ForegroundColor Gray
        
      try {
  $content = Get-Content $file.FullName -Raw
            $originalLength = $content.Length
            $modified = $false
      
# Remove comments based on file type
            if ($file.Extension -eq ".cs" -or $file.Extension -eq ".js") {
         # Remove single-line comments (//)
    $content = $content -replace '//.*?(\r?\n|$)', "`$1"
       
     # Remove multi-line comments (/* ... */)
      $content = $content -replace '/\*[\s\S]*?\*/', ''
   
                $modified = $true
 }
      elseif ($file.Extension -eq ".cshtml") {
     # Remove Razor comments (@* ... *@)
       $content = $content -replace '@\*[\s\S]*?\*@', ''
        
  # Remove HTML comments (<!-- ... -->)
  $content = $content -replace '<!--[\s\S]*?-->', ''
     
        # Remove single-line comments (//)
      $content = $content -replace '//.*?(\r?\n|$)', "`$1"
  
        # Remove multi-line comments (/* ... */)
  $content = $content -replace '/\*[\s\S]*?\*/', ''
      
             $modified = $true
      }
 elseif ($file.Extension -eq ".css") {
      # Remove CSS comments (/* ... */)
     $content = $content -replace '/\*[\s\S]*?\*/', ''
        
            $modified = $true
      }
  elseif ($file.Extension -eq ".sql") {
# Remove SQL comments (--)
  $content = $content -replace '--.*?(\r?\n|$)', "`$1"
    
     # Remove multi-line comments (/* ... */)
     $content = $content -replace '/\*[\s\S]*?\*/', ''
    
    $modified = $true
 }
    
        # Remove excessive blank lines (more than 2 consecutive)
    $content = $content -replace '(\r?\n){3,}', "`$1`$1"
            
   # Trim trailing whitespace
        $content = $content -replace '[ \t]+(\r?\n)', "`$1"
     
     if ($modified) {
        $newLength = $content.Length
        $saved = $originalLength - $newLength
       
        if ($saved -gt 0) {
       Set-Content -Path $file.FullName -Value $content -NoNewline
            Write-Host "  ? Removed $saved characters" -ForegroundColor Green
          $processedFiles++
         } else {
          Write-Host "  ??  No comments found" -ForegroundColor DarkGray
      }
            }
 }
        catch {
   Write-Host "  ? Error: $($_.Exception.Message)" -ForegroundColor Red
        }
  }
}

Write-Host ""
Write-Host "=== SUMMARY ===" -ForegroundColor Cyan
Write-Host "Total files scanned: $totalFiles" -ForegroundColor White
Write-Host "Files modified: $processedFiles" -ForegroundColor Green
Write-Host ""
Write-Host "? Comment removal complete!" -ForegroundColor Green
Write-Host ""
Write-Host "IMPORTANT: Please review the changes before committing!" -ForegroundColor Yellow
Write-Host "Some comments might be documentation that should be kept." -ForegroundColor Yellow
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
