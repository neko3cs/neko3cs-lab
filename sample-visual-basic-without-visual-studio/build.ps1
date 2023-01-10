# Build script

$NewtonsoftJsonDllPath = ".\package\newtonsoft.json.13.0.2\lib\net45\Newtonsoft.Json.dll"

New-Item `
  -ItemType Directory `
  -Path .\publish\ `
  -Force `
  2>&1 > $null

vbc.exe `
  .\Program.vb `
  /out:.\publish\a.exe `
  /reference:$NewtonsoftJsonDllPath

Copy-Item `
  -Path $NewtonsoftJsonDllPath `
  -Destination .\publish\ `
  -Force
