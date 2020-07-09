using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace InSysOut
{
     internal static class Constants
    {
        internal static readonly string helpMsg = @"
Usage:  oj Command
        oj [options] 

Options:
  -h|--help         Display help.
  -i|--info         Display OutputAsJson information.
  -v|--version      Display OutputAsJson version.

Command:
  ls|dir            List current directory content

Examples:
  oj -h
  oj ls
  oj dir

Remark: [] above means optional. | above means or.
";
        //internal static readonly string aMsg = @"";
        internal static readonly string infoMsg = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("InSysOut.ProductInfo.json")).ReadToEnd();
        //internal static readonly string versionMsg = @"1.0.0";
        internal static readonly string versionMsg = JsonDocument.Parse(infoMsg).RootElement.GetProperty("version").GetString();

        internal static readonly string unknownInputMsg = "Wrong/Unknown Input";

    }
}
