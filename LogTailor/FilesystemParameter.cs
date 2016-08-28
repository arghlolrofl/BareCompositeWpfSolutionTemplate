using System;
using System.Collections.Generic;
using System.IO;

namespace LogTailor
{
    public class FilesystemParameter
    {
        /// <summary>
        /// Identifier if there are valid items
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return FilesystemObjects.Count <= 0;
            }
        }

        /// <summary>
        /// Command Line parameters as <see cref="FileSystemInfo"/> objects
        /// </summary>
        public List<FileSystemInfo> FilesystemObjects { get; set; }


        /// <summary>
        /// Creates a new instance of the <see cref="FilesystemParameter"/> class
        /// </summary>
        /// <param name="cmdLineArgs">Given command line arguments</param>
        public FilesystemParameter() { }

        /// <summary>
        /// Parses all given command line arguments and tries
        /// to cache them as <see cref="FileSystemInfo"/> objects. 
        /// </summary>
        /// <param name="cmdLineArgs">Console application arguments</param>
        public void TryParse(string[] cmdLineArgs)
        {
            FilesystemObjects = new List<FileSystemInfo>();

            foreach (string arg in cmdLineArgs)
                if (!TryParse(arg))
                    Console.WriteLine("Invalid argument: " + arg);
        }

        /// <summary>
        /// Tries to parse a string and convert it into a 
        /// <see cref="FileInfo"/> or <see cref="DirectoryInfo"/> object,
        /// which is then being cached.
        /// </summary>
        /// <param name="cmdLineArg">File or directory path</param>
        private bool TryParse(string cmdLineArg)
        {
            if (File.Exists(cmdLineArg)) {
                FilesystemObjects.Add(new FileInfo(cmdLineArg));
                return true;
            }

            if (Directory.Exists(cmdLineArg)) {
                FilesystemObjects.Add(new DirectoryInfo(cmdLineArg));
                return true;
            }

            return false;
        }
    }
}
