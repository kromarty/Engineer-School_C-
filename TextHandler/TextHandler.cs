using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace TextHandler
{
    public class TextHandler
    {
        private string[] words;
        private Dictionary<string, int> dict = new Dictionary<string, int>();
        public TextHandler()
        {
            dict = new Dictionary<string, int>();
        }
        private void Count(object input)
        {
            try
            {
                string text = ' ' + (string)input;
                words = text.Split(new[] { '-', '.', '?', '!', ')', '(', ',', ':', ' ', '\"', '«', '»' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    if (!String.IsNullOrWhiteSpace(word))
                    {
                        if (dict.Keys.Contains(word.ToLower()))
                        {
                            dict[word.ToLower()]++;
                        }
                        else
                        {
                            dict.Add(word.ToLower(), 1);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
            }
        }
        public Dictionary<string, int> MultiThreadedResult(string text)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(Count));
            
            thread.Start(text);
            if (thread.IsAlive)
            {
                thread.Join();
            }
            return dict;

        }
        public Dictionary<string, int> SingleThreadedResult(string text)
        {
            Count(text);
            return dict;
        }
    }
}
