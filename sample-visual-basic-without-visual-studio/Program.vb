Imports System
Imports System.IO
Imports System.Net
Imports System.Linq
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module Program

  Sub Main(arg As String())

    Dim existsFile = Directory.GetFiles("./images/").Any()
    If Not existsFile Then
      Console.WriteLine("File not exists!!")
      Return
    End If

    Dim imageFile = new FileInfo(Directory.GetFiles("./images/").Single())

    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 ' URLがhttpsなのでTLS1.2を指定しないとプログラムが落ちる
    Dim request = WebRequest.Create("https://api.sampleapis.com/coffee/hot")
    Dim response = request.GetResponse()

    Using stream = response.GetResponseStream()
      Using streamReader = New StreamReader(stream)
        
        Dim json = JArray.Parse(streamReader.ReadToEnd())

        File.WriteAllText("./result.csv", ("""title"",""description"",""image"",""id""" & Environment.NewLine))
        For Each child As JObject In json
          File.AppendAllText("./result.csv", ("""" & child("title").ToString() & """,""" & child("description").ToString() & """,""" & _
              child("image").ToString() & """,""" & child("id").ToString() & """" & Environment.NewLine))
        Next

      End Using
    End Using
  End Sub

End Module