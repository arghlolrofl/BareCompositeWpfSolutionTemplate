using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogTailor
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            // Start 2 concurrent writers for log files in debugFolder/logs
            Writer w = new Writer();
            w.Run();
#endif

            Console.WriteLine();
            if (args.Length == 0) {
                Settings.PrintHelp();
                return;
            }

            // Parse config flags and slice them from command line args
            Settings settings = new Settings();
            try {
                args = settings.SliceConfigFlags(args);
            }
            catch (Exception ex) {
                Console.WriteLine("ERROR: " + ex.Message + Environment.NewLine);
                return;
            }

            // Check arguments and check if they are valid filesystem arguments
            FilesystemParameter param = new FilesystemParameter();
            param.TryParse(args);

            // No need to go on with an empty list
            if (param.IsEmpty) {
                Console.WriteLine("No valid arguments given. Exiting ...");
                return;
            }

            // Create a list of tail watchers
            List<Tailor> tailors = Tailor.Create(param, settings);
            if (tailors.Count == 0)
                throw new ArgumentException("Error creating tailors!");

            List<Task> tailTasks = StartTaskForEveryTailor(tailors);

            // Prevents exit while there are still pending tasks
            // and rechecks every ~2 secs
            while (tailTasks.Any((t) => t.Status == TaskStatus.Running || t.Status == TaskStatus.WaitingToRun))
                Thread.Sleep(2000);

            Console.WriteLine(Environment.NewLine + "Exiting LogTailor ... ByeBye!");
        }

        private static List<Task> StartTaskForEveryTailor(IEnumerable<Tailor> tailors)
        {
            // Create a task for every tailor
            List<Task> tailTasks = new List<Task>();
            foreach (Tailor tailor in tailors) {
                Task tailTask = new Task(tailor.Tail);
                tailTasks.Add(tailTask);
            }

            Console.WriteLine("> Items to watch: " + tailTasks.Count + Environment.NewLine);

            // Start all tasks
            for (int i = 0; i < tailTasks.Count; i++) {
                Task tailTask = tailTasks[i];
                tailTask.Start();
            }

            return tailTasks;
        }
    }
}
