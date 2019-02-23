using System;
using System.IO;

namespace S3Batcher.Logging
{
    public class Logger
    {
        private readonly string _loggerName;

        public Logger(Type owner)
        {
            _loggerName = owner.Name;
        }

        public void Info(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] {_loggerName} - {message}");
        }
    }
}