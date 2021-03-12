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
        public TextHandler()
        {

        }
        private void AddWord(object input)
        {
            string word = (string)input;
            if (!String.IsNullOrWhiteSpace(word))
            {
                
                if (!dict.TryAdd(word.ToLower(), 1))
                {
                    dict[word.ToLower()]++;       
                }
            }
        }            
    
        private ConcurrentDictionary<string, int> MultiThreadedResult(string text)
        {
            string[] words = text.Split(new[] { '-', '.', '?', '!', ')', '(', ',', ':', ' ', '\"', '«', '»' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(AddWord));
                thread.Start(word);
            }
            return dict;
        }

        private ConcurrentDictionary<string, int> SingleThreadedResult(string text)
        {
            string[] words = text.Split(new[] { '-', '.', '?', '!', ')', '(', ',', ':', ' ', '\"', '«', '»' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var word in words)
            {
                AddWord(word);
            }
            return dict;
        }
    }
}