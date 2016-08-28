using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace LogTailor
{
    public class Writer : IDisposable
    {
        const string log1 = "logs\\test.log";
        const string log2 = "logs\\test2.log";

        public FileInfo LogFile1 { get; set; }
        public FileInfo LogFile2 { get; set; }

        public Timer Timer { get; set; }

        public Writer()
        {
            LogFile1 = new FileInfo(log1);
            LogFile2 = new FileInfo(log2);
        }

        public void Run()
        {
            Task t = new Task(new Action(() => {
                Timer = new Timer();
                Timer.AutoReset = true;
                Timer.Elapsed += Timer_OnElapsed;
                Timer.Interval = 3000;

                Timer.Start();
            }));

            t.Start();
        }

        private void Timer_OnElapsed(object sender, ElapsedEventArgs e)
        {
            WriteLog1();
            WriteLog2();
        }

        private void WriteLog1()
        {
            using (StreamWriter writer = new StreamWriter(new FileStream(LogFile1.FullName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))) {
                writer.BaseStream.Seek(0, SeekOrigin.End);
                writer.WriteLine("Message from Logger 1");
            }
        }

        private void WriteLog2()
        {
            using (StreamWriter writer = new StreamWriter(new FileStream(LogFile2.FullName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))) {
                writer.BaseStream.Seek(0, SeekOrigin.End);
                writer.WriteLine("Message from Logger 2");
            }
        }

        public void Dispose()
        {
            Timer.Stop();
            Timer.Elapsed -= Timer_OnElapsed;
            Timer.Dispose();
        }
    }
}
