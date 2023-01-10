# Build script

New-Item `
  -ItemType Directory `
  -Path .\publish\ `
  -Force `
  2>&1 > $null

vbc.exe `
  .\Program.vb `
  /out:.\publish\a.exe `
  /reference:.\package\newtonsoft.json.13.0.2\lib\net45\Newtonsoft.Json.dll

Copy-Item `
  -Path .\images `
  -Destination .\publish\ `
  -Recurse `
  -Force
Copy-Item `
  -Path .\package\newtonsoft.json.13.0.2\lib\net45\Newtonsoft.Json.dll `
  -Destination .\publish\ `
  -Force
