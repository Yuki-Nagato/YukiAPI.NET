using System;
using System.Runtime.CompilerServices;

namespace YukApiCSharp {
    public class Logger {
        public static void Log(string msg, [CallerMemberName] string callerMemberName = null, [CallerLineNumber] int callerLineNumber = 0) {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fffzzz}] [Caller \"{callerMemberName}\"] [Line {callerLineNumber}] {msg}");
        }
    }
}
