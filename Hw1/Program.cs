using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

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
                    string text = sr.ReadToEnd();
                    Assembly asm = Assembly.LoadFrom("TextHandler.dll");
                    Type[] types = asm.GetTypes();

                    Type t = asm.GetType("TextHandler.TextHandler", true, true);
                    object obj1 = Activator.CreateInstance(t);
                    object obj2 = Activator.CreateInstance(t);
                    MethodInfo MultiThreadedResultmethod = t.GetMethod("MultiThreadedResult");
                    MethodInfo SingleThreadedResultmethod = t.GetMethod("SingleThreadedResult");
                    
                    Stopwatch stopWatch2 = new Stopwatch();
                    stopWatch2.Start();

                    object obj = SingleThreadedResultmethod.Invoke(obj2, new object[] { text });

                    stopWatch2.Stop();
                    TimeSpan ts = stopWatch2.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);
                    Console.WriteLine("Время однопоточной реализации " + elapsedTime);

                    ConcurrentDictionary<string, int> dict2 = (ConcurrentDictionary<string, int>)obj;

                    Stopwatch stopWatch1 = new Stopwatch();
                    stopWatch1.Start();

                    obj = MultiThreadedResultmethod.Invoke(obj1, new object[] { text });

                    stopWatch1.Stop();
                    ts = stopWatch1.Elapsed;
                    elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);
                    Console.WriteLine("Время многопоточной реализации " + elapsedTime);

                    ConcurrentDictionary<string, int> dict = (ConcurrentDictionary<string, int>)obj;

                    Directory.CreateDirectory("D:\\Temp");
                    using (StreamWriter sw1 = new StreamWriter("D:\\Temp\\Answer1.txt", false))
                    {
                        foreach (var key in dict.Keys)
                        {
                            sw1.WriteLine(key + " " + dict[key]);
                        }
                    }
                    using (StreamWriter sw2 = new StreamWriter("D:\\Temp\\Answer2.txt", false))
                    {
                        foreach (var key in dict2.Keys)
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
            Console.WriteLine("Нажмите Enter для выхода");
            Console.ReadLine();
        }
    }
}
