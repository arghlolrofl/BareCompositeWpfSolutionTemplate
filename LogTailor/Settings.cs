using System;
using System.Collections.Generic;

namespace LogTailor
{
    public class Settings
    {
        private int refreshTime = 1000;

        public int RefreshTime
        {
            get { return refreshTime; }
            set { refreshTime = value; }
        }

        private int lineCount = 5;

        public int LineCount
        {
            get { return lineCount; }
            set { lineCount = value; }
        }


        public string[] SliceConfigFlags(string[] cmdLineArgs)
        {
            List<string> args = new List<string>(cmdLineArgs);

            for (int i = 0; i < args.Count; i++) {
                string arg = args[i].Trim();
                if (arg.StartsWith("--")) {
                    switch (arg) {
                        case "--help":
                            args.RemoveAt(i--);
                            PrintHelp();
                            break;
                        case "--lines":
                            args.RemoveAt(i);
                            arg = args[i].Trim();

                            lineCount = Int32.Parse(arg);
                            args.RemoveAt(i--);
                            break;
                        case "--refresh":
                            args.RemoveAt(i);
                            arg = args[i].Trim();

                            refreshTime = Int32.Parse(arg);

                            if (RefreshTime < 100)
                                throw new ArgumentException("Tail refresh time less than 100 ms is not allowed!");

                            args.RemoveAt(i--);
                            break;
                        default:
                            Console.WriteLine("ERROR: Unknown config flag: " + arg);
                            break;
                    }
                }
            }

            return args.ToArray();
        }

        public static void PrintHelp()
        {
            Console.WriteLine("+-------------------------------------------------------------");
            Console.WriteLine("| Help");
            Console.WriteLine("+-------------------------------------------------------------");
            Console.WriteLine("|");
            Console.WriteLine("| --help:        These help instructions");
            Console.WriteLine("|");
            Console.WriteLine("| --lines:       Number of lines to review from end of file");
            Console.WriteLine("|");
            Console.WriteLine("| --refresh:     Tail refresh time in milliseconds");
            Console.WriteLine("|");
            Console.WriteLine("+-------------------------------------------------------------");
            Console.WriteLine();
        }
    }
}
