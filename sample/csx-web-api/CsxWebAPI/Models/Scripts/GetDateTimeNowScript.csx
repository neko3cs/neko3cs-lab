using System;

// Define Section.
public static class Script
{
    public static string Run()
    {
        var now = DateTime.Now;

        return now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

// Run Section.
#region RELEASE
return Script.Run();
#endregion

#region DEBUG
// var ret = Script.Run();
// Console.WriteLine($"return value: {ret}");
// return 0;
#endregion
