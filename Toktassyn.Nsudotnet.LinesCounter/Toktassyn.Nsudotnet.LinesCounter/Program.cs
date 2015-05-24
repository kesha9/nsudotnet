using System;
using System.IO;
using System.Linq;

namespace Toktassyn.Nsudotnet.LinesCounter
{
    class Program
    {
           public static int Count(string directori, string[] typeOfFiles)
        {
            bool multiRowsComment = false;
            string[] files = Directory.GetFiles(directori);
            string[] directories = Directory.GetDirectories(directori);

            int count = directories.Sum(dir => Count(dir, typeOfFiles));

            foreach (string type in typeOfFiles)
            {
                foreach (string tr in files.Where(tr => tr.EndsWith(type)))
                {
                    using (StreamReader reader = new StreamReader(tr))
                    {
                        string str = null;
                        while ((str = reader.ReadLine()) != null)
                        {
                            str = str.Trim();

                            if (str.Contains("/*") && !str.StartsWith("/*"))
                            {
                                multiRowsComment = true;
                                count++;
                                continue;
                            }

                            if (str.Contains("*/") && !str.EndsWith("*/") && multiRowsComment)
                            {
                                multiRowsComment = false;
                                count++;
                                continue;
                            }

                            if (str.StartsWith("/*"))
                            {
                                multiRowsComment = true;
                                continue;
                            }

                            if (str.StartsWith("*/") && multiRowsComment)
                            {
                                multiRowsComment = false;
                                continue;
                            }

                            if (str.StartsWith("//") || multiRowsComment || str.Length == 0)
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
            string curdir = args[0];
            int result = Count(curdir, args.Skip(1).ToArray());
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
    }
