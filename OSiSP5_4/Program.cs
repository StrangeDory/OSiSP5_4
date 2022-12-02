using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;

namespace OSiSP5_4
{
    class Program
    {
        const String SRC_FILEPATH = "..\\..\\..\\..\\src.txt";
        const String OUT_FILEPATH = "..\\..\\..\\..\\out.txt";
        const int THREAD_COUNT = 6;
        static List<List<String>> allWords;

        static void Main(string[] args)
        {
            allWords = new List<List<String>>();
            var words = readFile();
            var tasks = initTasks(ref words);
            Queue queueTasks = new Queue();
            for (int i = 0; i < THREAD_COUNT; i++)
                queueTasks.Enqueue(tasks[i]);
            ExecuteTasks(queueTasks);
            words.Clear();
            words = allWords[0];
            for (int i = 1; i < allWords.Count; i++)
                words = SortLast(words, allWords[i]);
            writeFile(words);
        }

        static List<String> readFile()
        {
            List<String> res;
            String text = "";
            try
            {
                StreamReader sr = new StreamReader(SRC_FILEPATH);
                text = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            res = new List<String>(text.Split(new char[] { ' ', '\r', '\n' })).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            return res;
        }

        static void writeFile(List<String> result)
        {
            try
            {
                StreamWriter sw = new StreamWriter(OUT_FILEPATH);
                sw.Write(String.Join('\n', result));
                sw.Close();
            } 
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        static void ExecuteTasks(Queue queueTasks)
        {
            Thread[] threads = new Thread[THREAD_COUNT];
            for (int i = 0; i < THREAD_COUNT; i++)
            {
                threads[i] = new Thread(() => {
                    if (queueTasks.Count != 0)
                    {
                        Task task = (Task)queueTasks.Dequeue();
                        task.Start();
                    }
                });
                threads[i].Start();
            }
            for (int i = 0; i < THREAD_COUNT; i++)
                threads[i].Join();
        }

        static List<Task> initTasks(ref List<String> words)
        {
            List<Task> tasks = new List<Task>();
            int wordsPerThread = (int)Math.Ceiling((float)words.Count() / (float)THREAD_COUNT);
            for(int i = 0; i < THREAD_COUNT; i++)
            {
                List<String> part = new List<String>();
                for(int j = i * wordsPerThread; j < (words.Count() < (i + 1) * wordsPerThread ?
                    words.Count() : (i + 1) * wordsPerThread); j++)
                {
                    part.Add(words[j]);
                }
                allWords.Add(part);
                Task task = new Task(() =>
                {
                    lock(allWords)
                        allWords[i].Sort();
                });
                tasks.Add(task);
            }
            return tasks;
        }

        static List<String> SortLast(List<String> list1, List<String> list2)
        {
            List<String> result = new List<String>(list1);
            for(int i = 0; i < list2.Count; i++)
                result.Add(list2[i]);
            result.Sort();
            return result;
        }
    }
}
