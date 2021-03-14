using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TextHandler
{
    public class TextHandler
    {
        private ConcurrentDictionary<string, int> dict = new ConcurrentDictionary<string, int>();
        public TextHandler() { }
        private void AddWords(object input)
        {
            string[] words = (string[])input;
            foreach(var word in words)
            {
                if (!String.IsNullOrWhiteSpace(word))
                {
                    if (!dict.TryAdd(word.ToLower(), 1))
                    {
                        dict[word.ToLower()]++;
                    }
                }
            }
           
        }            
    
        public ConcurrentDictionary<string, int> MultiThreadedResult(string text)
        {
            const char V = '\n';
            string[] words = text.Split(new[] { '-', '.', '?', '!', ')', '(', ',', ':', ' ', '\"', '«', '»', V }, StringSplitOptions.RemoveEmptyEntries);
            int counter = words.Length / 2;
            int cnt = words.Length / 2;
            var words_group = words.GroupBy(_ => counter++ / cnt).Select(v => v.ToArray()).ToArray();
            foreach (var wrds in words_group)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(AddWords));
                thread.Start(wrds);
            }
            return dict;
        }

        private ConcurrentDictionary<string, int> SingleThreadedResult(string text)
        {
            const char V = '\n';
            string[] words = text.Split(new[] { '-', '.', '?', '!', ')', '(', ',', ':', ' ', '\"', '«', '»', V }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (!String.IsNullOrWhiteSpace(word))
                {
                    if (!dict.TryAdd(word.ToLower(), 1))
                    {
                        dict[word.ToLower()]++;
                    }
                }
            }
            return dict;
        }
    }
}