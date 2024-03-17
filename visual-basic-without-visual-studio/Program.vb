Imports System.IO
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module Program

  Sub Main(arg As String())

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