using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace LinesCounter
{
    class LinesCounter
    {
        /// <summary>
        /// first element of args is the directory
        /// second and subsequent - file extensions
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("to few arguments. format: \"<dir>\" \".<ext>\"");
                return;
            }
            if (args.Length > 2)
            {
                Console.WriteLine("too many arguments. format: \"<dir>\" \".<ext>\"");
                return;
            }
            Console.WriteLine("{0} line(s)", CountInDirectory(new DirectoryInfo(args[0]), args[1]));
            Console.ReadKey();
        }
 
 
        private static int CountInDirectory(DirectoryInfo directoryInfo, string extension)
        {
            return directoryInfo.EnumerateFiles(extension).Sum(file => CountInFile(file))
            + directoryInfo.EnumerateDirectories().Sum(dir => CountInDirectory(dir, extension));
        }
 
        private static int CountInFile(FileInfo fileInfo)
        {
            var count = 0;
            using (var streamReader = new StreamReader(fileInfo.OpenRead(), Encoding.UTF8))
            {
                var input = streamReader.ReadToEnd();
                const string lineComments = @"//(.*?)\r?\n";
                const string blockComments = @"/\*(.*?)\*/";
                const string strings = @"""((\\[^\n]|[^""\n])*)""";
                const string verbatimStrings = @"@(""[^""]*"")+";
 
                var noCommentsAndEmptyLines = Regex.Replace(input,blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
                    me =>
                    {
                        if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                            return "";// Keep the literal strings
                        return me.Value;
                    },RegexOptions.Singleline);
               
                noCommentsAndEmptyLines = Regex.Replace(noCommentsAndEmptyLines, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                Console.Write(noCommentsAndEmptyLines);
 
                count += noCommentsAndEmptyLines.Split('\n').Length - 1;
            }
            return count;
        }
    }
}
