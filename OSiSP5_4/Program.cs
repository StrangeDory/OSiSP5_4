using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OSiSP5_4
{
    class Program
    {
        static String SRC_FILEPATH = "..\\..\\..\\..\\src.txt";
        static String OUT_FILEPATH = "..\\..\\..\\..\\out.txt";

        static void Main(string[] args)
        {
            var words = readFile();
            
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
    }
}
