using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Hw1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (StreamReader sr = new StreamReader("Text.txt"))
                {
                    //File.SetAttributes(sr, FileAttributes.Normal);
                    TextHandler.TextHandler textHandler1 = new TextHandler.TextHandler();
                    TextHandler.TextHandler textHandler2 = new TextHandler.TextHandler();
                    string text = sr.ReadToEnd();
                    Stopwatch stopWatch1 = new Stopwatch();
                    stopWatch1.Start();
                    Dictionary<string, int> dict1 = textHandler1.SingleThreadedResult(text);
                    stopWatch1.Stop();
                    TimeSpan ts = stopWatch1.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);
                    Console.WriteLine("Время однопоточной реализации " + elapsedTime);
                    Stopwatch stopWatch2 = new Stopwatch();
                    stopWatch2.Start();
                    Dictionary<string, int> dict2 = textHandler2.MultiThreadedResult(text);

                    
                    Directory.CreateDirectory("D:\\Temp");
                    using (StreamWriter sw1 = new StreamWriter("D:\\Temp\\Answer1.txt", false))
                    {
                        foreach (string key in dict1.Keys)
                        {
                            sw1.WriteLine(key + " " + dict1[key]);
                        }
                    }
                    using (StreamWriter sw2 = new StreamWriter("D:\\Temp\\Answer2.txt", false))
                    {
                        Dictionary<string, int> emptydict = new Dictionary<string, int>();
                        while(emptydict == dict2)
                        {
                            System.Threading.Thread.Sleep(1);
                        }
                        stopWatch2.Stop();
                        TimeSpan nts = stopWatch2.Elapsed;
                        string nelapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);
                        Console.WriteLine("Время многопоточной реализации " + elapsedTime);
                        foreach (string key in dict2.Keys)
                        {
                            sw2.WriteLine(key + " " + dict2[key]);
                        }
                        
                    }
                    Console.WriteLine("Успешное выполнение. Ответ лежит по адресу D:\\Temp");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}
