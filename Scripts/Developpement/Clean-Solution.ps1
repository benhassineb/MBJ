[string]$rootFolder = [System.IO.Path]::GetFullPath("$PSScriptRoot\..\..\Source")
Get-ChildItem $rootFolder -Include bin, obj -Recurse | ForEach-Object ($_) { Remove-Item $_.FullName -Force -Recurse }