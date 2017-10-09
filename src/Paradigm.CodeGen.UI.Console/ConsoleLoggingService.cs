using System;
using Paradigm.CodeGen.Logging;

namespace Paradigm.CodeGen.UI.Console
{
    public class ConsoleLoggingService: ILoggingService
    {
        public void Write(string text)
        {
            Write(text, ConsoleColor.Gray, ConsoleColor.Black);
        }

        public void WriteLine(string text)
        {
            Write(text + Environment.NewLine, ConsoleColor.Gray, ConsoleColor.Black);
        }

        public void Error(string text)
        {
            Write("ERROR: " + text + Environment.NewLine, ConsoleColor.Red, ConsoleColor.Black);
        }

        public void Warning(string text)
        {
            Write("WARNING: " + text + Environment.NewLine, ConsoleColor.Yellow, ConsoleColor.Black);
        }

        public void Notice(string text)
        {
            Write(text + Environment.NewLine, ConsoleColor.Green, ConsoleColor.Black);
        }

        private static void Write(string text, ConsoleColor foreground, ConsoleColor background)
        {
            var oldForeground = System.Console.ForegroundColor;
            var oldBackground = System.Console.BackgroundColor;
          
            System.Console.ForegroundColor = foreground;
            System.Console.BackgroundColor = background;
            
            System.Console.Write(text);
            
            System.Console.ForegroundColor = oldForeground;
            System.Console.BackgroundColor = oldBackground;
        }
    }
}