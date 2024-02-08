using System;
using System.IO;
using Newtonsoft.Json;

namespace VoiceCraft.Server
{
    public class Logger
    {
        private static string logFilePath;
        private static DateTime lastLogFileCreationTime;
        private static int logFileIntervalHours;

        static Logger()
        {
            LoadConfig();
            CreateLogFileIfRequired();
        }

        private static void LoadConfig()
        {
            try
            {
                string configPath = "config.json";
                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    dynamic config = JsonConvert.DeserializeObject(json);
                    logFilePath = config["LogFilePath"];
                    logFileIntervalHours = config["LogFileIntervalHours"];
                }
                else
                {
                    logFilePath = "logfile.txt"; // Default log file path if config file is not found
                    logFileIntervalHours = 6; // Default interval if config file is not found
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading config file: {ex.Message}");
                logFilePath = "logfile.txt"; // Default log file path if error occurs
                logFileIntervalHours = 6; // Default interval if error occurs
            }
        }

        private static void CreateLogFileIfRequired()
        {
            if (!File.Exists(logFilePath) || (DateTime.Now - lastLogFileCreationTime).TotalHours >= logFileIntervalHours)
            {
                lastLogFileCreationTime = DateTime.Now;
                CloseLogFile();
            }
        }

        private static void CloseLogFile()
        {
            // Close the existing log file (if any) and create a new one
            try
            {
                if (File.Exists(logFilePath))
                {
                    File.Move(logFilePath, $"{Path.GetFileNameWithoutExtension(logFilePath)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(logFilePath)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing log file: {ex.Message}");
            }
        }

        public static void LogToConsole(LogType logType, string message, string tag)
        {
            CreateLogFileIfRequired(); // Check if a new log file needs to be created
            LogToFile(logType, message, tag); // Log to file first
            switch (logType)
            {
                case LogType.Info:
                    Console.ResetColor();
                    Console.WriteLine($"[{DateTime.Now}] [{tag}]: {message}");
                    break;

                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{DateTime.Now}] [Error] [{tag}]: {message}");
                    Console.ResetColor();
                    break;

                case LogType.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{DateTime.Now}] [Warning] [{tag}]: {message}");
                    Console.ResetColor();
                    break;

                case LogType.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{DateTime.Now}] [{tag}]: {message}");
                    Console.ResetColor();
                    break;
            }
        }

        private static void LogToFile(LogType logType, string message, string tag)
        {
            string logEntry = $"[{DateTime.Now}] [{logType}] [{tag}]: {message}";
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }

    public enum LogType
    {
        Info,
        Warn,
        Error,
        Success
    }
}
