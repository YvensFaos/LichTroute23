using System;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEngine;

namespace Utils
{
    public class LoggerUtils : Singleton<LoggerUtils>
    {
        [SerializeField, ReadOnly]
        private string logTo;
        [SerializeField, ReadOnly]
        private List<string> currentLog;
        
        private void Start()
        {
            logTo = Path.Combine(Application.dataPath, $"harmonia_log_[{DateTime.Now:dd-MM-HH-mm}].txt");

            if (!File.Exists(logTo))
            {
                File.Create(logTo).Close();
            }
            LogMessage("Logger initialized.");
            currentLog = new List<string>();
        }
        
        public void LogMessage(string message)
        {
            var logMessage = $"[{DateTime.Now:HH:mm:ss}]: {message}";
            using var streamWriter = File.AppendText(logTo);
            streamWriter.WriteLine(logMessage);
            currentLog.Add(logMessage);
            DebugUtils.DebugLogMsg($"Logger: {logMessage}");
        }

        public string LastLog() => currentLog[^1];
    }
}