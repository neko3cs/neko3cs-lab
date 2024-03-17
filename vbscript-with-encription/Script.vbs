' Sample Script
Option Explicit

' 事前準備
Dim fileSystem
Set fileSystem = CreateObject("Scripting.FileSystemObject")
If fileSystem.FileExists("./temp.json") Then
  fileSystem.DeleteFile "./temp.json"
End If

' JSON取得
Dim xmlHttp
Set xmlHttp = CreateObject("MSXML2.XMLHTTP.3.0")

xmlHttp.Open "GET", "https://api.sampleapis.com/coffee/hot", False
xmlHttp.Send

If xmlHttp.Status = 200 Then

  Dim stream
  Set stream = CreateObject("ADODB.Stream")
  stream.Open
  stream.Type = 1
  stream.Write xmlHttp.responseBody

  stream.Position = 0
  stream.Type = 2
  stream.Charset = "_autodetect"
  stream.SaveToFile "./temp.json"

  stream.Close
  
End If

' JSON2CSV
Dim cmd
cmd = "(Get-Content -Path ./temp.json -Encoding utf8 | ConvertFrom-Json) | " & _
      "Select-Object title, description, image, id | " & _ 
      "Export-Csv -Path result.csv -NoTypeInformation;" & _
      "Remove-Item -Path ./temp.json"

Dim shell
Set shell = CreateObject("WScript.Shell")
Dim retCode
shell.Run "powershell -ExecutionPolicy RemoteSigned -Command " & cmd, 0, True
