﻿namespace EzhikLoader.Server.Logger
{
    public class FileLogger : ILogger, IDisposable
    {
        string filePath;
        static object _lock = new object();

        public FileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
        {
            lock (_lock)
            {
                File.AppendAllText(filePath, formatter(state, exception) +  Environment.NewLine);
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose() { }
    }
}
