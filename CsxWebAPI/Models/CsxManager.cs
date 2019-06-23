using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace CsxWebAPI.Models
{
    public static class CsxManager
    {
        public static string WebRootPath { get; set; }

        public static async Task<T> RunScriptAsync<T>(string scriptId)
        {
            var settings = AppSettings.Instance;

            var scriptPath = $"{WebRootPath}{settings.ScriptPaths[scriptId]}";
            var scriptText = File.ReadAllText(scriptPath, Encoding.UTF8);

            var script = CSharpScript.Create<T>(scriptText);

            var result = await script.RunAsync();
            return result.ReturnValue;
        }

        public static async Task<T> RunScriptAsync<T>(string scriptId, dynamic args)
        {
            var settings = AppSettings.Instance;

            var scriptPath = $"{WebRootPath}{settings.ScriptPaths[scriptId]}";
            var scriptText = File.ReadAllText(scriptPath, Encoding.UTF8);

            var script = CSharpScript.Create<T>(scriptText);

            var result = await script.RunAsync(args);
            return result.ReturnValue;
        }
    }
}
