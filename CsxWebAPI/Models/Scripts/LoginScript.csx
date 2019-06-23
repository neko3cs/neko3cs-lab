#r "RespClient.dll"

using System;

// Define Section.
public class User
{
    public string Id { get; set; }
    public string Password { get; set; }
}
public static class Script
{
    public static bool Run(string[] args)
    {
        var userID = args[0];
        var userPW =  args[1];
        
        var isValidAccount = false;
        using (var client = new Redis.Protocol.RespClient())
        {
            var results = client.UsePipeline()
                                .QueueCommand($"get user:id:{userID}", Encoding.UTF8.GetString)
                                .Execute();
            try
            { 
                var pw = (string)results[0];
                isValidAccount = userPW.Equals(pw);
            }
            catch { isValidAccount = false; }
        }
    
        return isValidAccount;
    }
}

// Run Section.
#region RELEASE
return Script.Run(Args.ToArray());
#endregion

#region DEBUG
// if (Args.Count() != 2)
// {
//     Console.Error.WriteLine("wrong arguments!: LoginScript.csx {id} {password}");
//     return 1;
// }
// var ret = Script.Run(Args.ToArray());
// Console.WriteLine($"return value: {ret}");
// return 0;
#endregion
