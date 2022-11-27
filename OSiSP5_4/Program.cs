using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace OSiSP5_4
{
    class Program
    {
        const String SRC_FILEPATH = "..\\..\\..\\..\\src.txt";
        const String OUT_FILEPATH = "..\\..\\..\\..\\out.txt";
        const int THREAD_COUNT = 6;

        static void Main(string[] args)
        {
            var words = readFile();
            var tasks = initTasks(words);
            Queue queueTasks = new Queue();
            for (int i = 0; i < THREAD_COUNT; i++)
                queueTasks.Enqueue(tasks[i]);

            Console.ReadKey();
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
                sw.Write(result);
                sw.Close();
            } 
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        static List<Task> initTasks(List<String> words)
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
                Task task = new Task(() =>
                {
                    part.Sort();
                });
                tasks.Add(task);
            }
            return tasks;
        }
    }
}
