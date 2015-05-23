using System;
using System.IO;
using System.Linq;

namespace Toktassyn.Nsudotnet.LinesCounter
{
    class Program
    {
           public static int CountLines(string dir, string[] typeOfFiles)
        {
            bool multiRowsComment = false;
            string[] directories = Directory.GetDirectories(dir);
            string[] files = Directory.GetFiles(dir);

            int count = directories.Sum(directory => CountLines(directory, typeOfFiles));

            foreach (string type in typeOfFiles)
            {
                foreach (string f in files.Where(f => f.EndsWith(type)))
                {
                    using (StreamReader fileReader = new StreamReader(f))
                    {
                        string line = null;
                        while ((line = fileReader.ReadLine()) != null)
                        {
                            line = line.Trim();

                            if (line.Contains("/*") && !line.StartsWith("/*"))
                            {
                                multiRowsComment = true;
                                count++;
                                continue;
                            }

                            if (line.Contains("*/") && !line.EndsWith("*/") && multiRowsComment)
                            {
                                multiRowsComment = false;
                                count++;
                                continue;
                            }

                            if (line.StartsWith("/*"))
                            {
                                multiRowsComment = true;
                                continue;
                            }

                            if (line.StartsWith("*/") && multiRowsComment)
                            {
                                multiRowsComment = false;
                                continue;
                            }

                            if (line.StartsWith("//") || multiRowsComment || line.Length == 0)
                            {
                                continue;
                            }

                            count++;
                        }
                    }
                }
            }
            return count;
        }
            
            static void Main(string[] args)
        {
            string curdir = Directory.GetCurrentDirectory();
            int result = CountLines(curdir, args);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
    }
