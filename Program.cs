using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace InSysOut
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                string targ = string.Join("", args).ToLower(); //Quick hack to trim and lower the trolls (typos).
                if (string.IsNullOrWhiteSpace(targ))
                {
                    Console.WriteLine(Constants.helpMsg);
                }
                else
                {
                    if (targ.Equals("-h") || targ.Equals("--help"))
                    {
                        Console.WriteLine(Constants.helpMsg);
                    }
                    else if (targ.Equals("-i") || targ.Equals("--info"))
                    {
                        Console.WriteLine(Constants.infoMsg);
                    }
                    else if (targ.Equals("-v") || targ.Equals("--version"))
                    {
                        Console.WriteLine(Constants.versionMsg);
                        //int i = 0; int f = i / 0;//Emulate Exception.
                    }
                    else if (targ.Equals("ls") || targ.Equals("dir"))
                    {
                        string curdir = Directory.GetCurrentDirectory();
                        string path = Path.Join(curdir, Path.DirectorySeparatorChar.ToString());
                        string[] files = Directory.GetFiles(curdir);
                        FilesObj fo = new FilesObj();
                        fo.Path = path;
                        fo.Files = new string[files.Length];
                        
                        for(int i=0;i<files.Length;i++)
                        {
                            string fname = files[i].Replace(path, "");
                            fo.Files[i]=fname;
                        }
                        
                        var jsOpt = new JsonSerializerOptions
                        {
                            WriteIndented = true, 
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        };
                        string jsonString = JsonSerializer.Serialize(fo, jsOpt);
                        
                        Console.WriteLine(jsonString);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(Constants.unknownInputMsg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(Constants.helpMsg);
                    }


                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: {0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            //ToDos:
            //Support pipes (| oj) and (< f.txt). ref: https://stackoverflow.com/questions/199528/c-sharp-console-receive-input-with-pipe?answertab=oldest#tab-top

            //Remark: its Million dollar mistake to create json manually using StringBuilder! avoid Serialization issues by using objs.

        }

        static string FormatJsonText(string jsonString)
        {
            using var doc = JsonDocument.Parse(
                jsonString,
                new JsonDocumentOptions
                {
                    AllowTrailingCommas = true
                }
            );
            MemoryStream memoryStream = new MemoryStream();
            using (
                var utf8JsonWriter = new Utf8JsonWriter(
                    memoryStream,
                    new JsonWriterOptions
                    {
                        Indented = true
                    }
                )
            )
            {
                doc.WriteTo(utf8JsonWriter);
            }
            return new UTF8Encoding()
                .GetString(memoryStream.ToArray());
        }
    }

    public class FilesObj
    {
        public string Path { get; set; }
        public string[] Files { get; set; }
    }
}
