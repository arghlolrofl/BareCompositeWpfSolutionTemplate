using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace LogTailor
{
    public class Tailor
    {
        const string PATTERN_START = "^.+?";
        const string PATTERN_LINE_MATCH = "\\n.+?";
        const string PATTERN_END = "$(?:[\\r\\n]?)(?![\\r\\n])";

        private static int nextTailorId = 0;
        private static int sleepTime = 750;

        /// <summary>
        /// Creates a list of Tailor objects. Each one can
        /// be used to tail a file.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<Tailor> Create(FilesystemParameter param, Settings settings)
        {
            sleepTime = settings.RefreshTime;
            Console.WriteLine("> Set sleep time to: " + sleepTime);

            List<Tailor> tailors = new List<Tailor>();

            foreach (FileSystemInfo fsInfo in param.FilesystemObjects) {
                if (fsInfo is FileInfo)
                    tailors.Add(new Tailor(fsInfo as FileInfo, settings.LineCount));

                if (fsInfo is DirectoryInfo)
                    foreach (FileInfo fileInDir in (fsInfo as DirectoryInfo).GetFiles("*.*", SearchOption.TopDirectoryOnly))
                        tailors.Add(new Tailor(fileInDir, settings.LineCount));
            }

            return tailors;
        }

        /// <summary>
        /// Tail Stream Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// File to be tailed
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// Creates a new tailor instance
        /// </summary>
        /// <param name="fileInfo">The file to be tailed</param>
        /// <param name="lineCount">Number of lines to be reviewed</param>
        public Tailor(FileInfo fileInfo, int lineCount = 5)
        {
            File = fileInfo;
            Id = nextTailorId++;

            Console.WriteLine($"> Created tail [{Id}] for '{File.FullName}'");
            ReviewLastLines(lineCount);
        }

        /// <summary>
        /// Prints the last lines of a file
        /// </summary>
        /// <param name="lineCount">Number of lines to print beginning from the end of the file</param>
        private void ReviewLastLines(int lineCount)
        {
            string text = String.Empty;

            using (StreamReader reader = new StreamReader(new FileStream(File.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                text = reader.ReadToEnd();

            StringBuilder sb = new StringBuilder();
            sb.Append(PATTERN_START);
            for (int i = 0; i < lineCount - 1; i++)
                sb.Append(PATTERN_LINE_MATCH);
            sb.Append(PATTERN_END);

            Regex regex = new Regex(sb.ToString(), RegexOptions.Multiline);
            Match m = regex.Match(text);

            if (m.Success)
                Console.WriteLine($"{Environment.NewLine} [{Id}] "
                    + m.Value.Replace(Environment.NewLine, $"{Environment.NewLine} [{Id}] "));

            Console.WriteLine();
        }

        /// <summary>
        /// Checks in intervals, if there have been changes in a file.
        /// If there are changes, they will be printed to console.
        /// </summary>
        public void Tail()
        {
            using (StreamReader reader = new StreamReader(new FileStream(File.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
                string text = reader.ReadToEnd();

                //start at the end of the file
                long lastMaxOffset = reader.BaseStream.Length;

                while (true) {
                    Thread.Sleep(sleepTime);

                    //if the file size has not changed, idle
                    if (reader.BaseStream.Length == lastMaxOffset)
                        continue;

                    //seek to the last max offset
                    reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin);

                    //read out of the file until the EOF
                    string line = "";
                    while ((line = reader.ReadLine()) != null) {
                        if (String.IsNullOrWhiteSpace(line))
                            continue;

                        Console.WriteLine($" [{Id}] " + line);
                    }

                    //update the last max offset
                    lastMaxOffset = reader.BaseStream.Position;
                }
            }
        }
    }
}