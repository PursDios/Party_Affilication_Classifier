using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Party_Affilication_Classifier
{
    class Program
    {
        static void Main(string[] args)
        {
            //StreamReader sr = new StreamReader("Ngrams.txt");
            //StreamWriter sw = new StreamWriter("Ngrams2.txt");
            Controller c = new Controller();
            c.MainMenu();
            //string str = sr.ReadToEnd();
            //sr.Close();
            //str.Replace('\t', ' ');

            //List<string> Ngrams = str.Split('\t', ' ', '\n', '\r').ToArray().ToList();
            //List<string> newNgrams = new List<string>();
            //foreach (string s in Ngrams)
            //{
            //    if (s == "" || s == " " || s.Contains('\r') || s.Contains('\n') || s.Contains('\r') || s.Contains('\t'))
            //    {

            //    }
            //    else
            //    {
            //        newNgrams.Add(s);
            //    }
            //}
            //for (int i = 0; i < newNgrams.Count(); i++)
            //{
            //    sw.WriteLine(newNgrams[i] + "/");
            //    i++;
            //}
            //Console.WriteLine(newNgrams.Count());
            //Console.ReadLine();
        }
    }
}
