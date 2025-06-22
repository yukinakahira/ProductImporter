using System;
using System.Runtime.CompilerServices;

namespace ImporterApp.Services
{
    // ロガー
    public static class Logger
    {
        public static void Info(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            var fileName = System.IO.Path.GetFileName(filePath);
            Console.WriteLine($"[INFO] ({fileName}:{memberName}:{lineNumber}) {message}");
        }

        public static void Warn(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            var fileName = System.IO.Path.GetFileName(filePath);
            Console.WriteLine($"[WARN] ({fileName}:{memberName}:{lineNumber}) {message}");
        }

        public static void Error(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            var fileName = System.IO.Path.GetFileName(filePath);
            Console.WriteLine($"[ERROR] ({fileName}:{memberName}:{lineNumber}) {message}");
        }
    }
}
