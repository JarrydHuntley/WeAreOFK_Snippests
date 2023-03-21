#Make sure Image is installed first 

cls

Try{Start-transcript C:\ResolutionResizeOutput.txt -ErrorAction Stop}catch{Start-Transcript C:\ResolutionResizeOutput.txt}

write-host "start time $(Get-Date)"
$startScriptTime = (Get-Date).Second

cd C:\Backgrounds\

ForEach ($file in (Get-ChildItem -Recurse -include *.png  | % { $_.FullName}))
{
    #Write-Output $file  "#"
    $resolution = magick identify -format '%wx%h' $file

    if($resolution -eq '8192x3432')
    {
      Write-Host "Already converted $file" -ForegroundColor green -BackgroundColor black
    }

    elseif($resolution -eq '8192x3428')
    {
        Write-Host "$resolution - $file"
        
        $starProcessingTime = (Get-Date).Millisecond
        magick convert $file -resize 8192x3432 -background transparent -gravity center -extent 8192x3432 $file
        $stopProcessingTime = (Get-Date).Millisecond

        Write-Host "--Processing took $($stopProcessingTime - $starProcessingTime) ms"

        if($LASTEXITCODE -eq 0)
        {
            #Write-Host "Convert Success"
            ((Get-Content -path $file'.meta' -Raw) -replace 'maxTextureSize: 8192','maxTextureSize: 4096') | Set-Content -Path $file'.meta'
            ((Get-Content -path $file'.meta' -Raw) -replace 'resizeAlgorithm: 0','resizeAlgorithm: 1') | Set-Content -Path $file'.meta'
        } 
        else 
        {
            Write-Host "Magick convert failed command failed"  -ForegroundColor red -BackgroundColor white
        }
    }
    else
    {
        Write-Host "Warning. - Resolution is $resolution for $file" -ForegroundColor red -BackgroundColor white
    }

}
$stopScriptTime = (Get-Date).Second
$executionTime =  $stopScriptTime - $startScriptTime

Write-Host "Script finished execution in $($stopScriptTime - $startScriptTime) seconds"
write-host "end time $(Get-Date)"
Stop-Transcript
