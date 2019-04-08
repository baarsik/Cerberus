using System;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace DataGrabber
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var parser = new Parser();
            await parser.Parse();
            await parser.Save();
        }

        public static void WriteLine(string line, WriteLineType type = WriteLineType.Normal)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"[{DateTime.Now.ToLongTimeString()}] ");
            
            switch (type)
            {
                case WriteLineType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case WriteLineType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case WriteLineType.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
            Console.WriteLine(line);
            Console.ResetColor();
        }
    }
    
    public enum WriteLineType
    {
        Normal,
        Warning,
        Error,
        Success
    }
}