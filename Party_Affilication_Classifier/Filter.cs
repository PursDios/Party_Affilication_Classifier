using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Party_Affilication_Classifier
{
    public class Filter
    {
        public string RemoveAll(string s)
        {
            return RemoveStopwords(RemoveGrammar(s));
        }
        public string RemoveGrammar(string s)
        {
            //has to be done character by character otherwise some \n's or \r's won't be filtered out properly. Or two words will blend together.
            char[] forbiddenChars = { '"', ':', ';', '\n', '\t', '.', ',', '\r' };
            char[] chars = s.ToCharArray();

            for (int i = 0; i < chars.Count(); i++)
            {
                foreach (char c in forbiddenChars)
                {
                    if (c == chars[i])
                    {
                        chars[i] = ' ';
                    }
                }
            }
            return (new string(chars));
        }
        public string RemoveStopwords(string s)
        {
            //remove stopwords
            StreamReader sr = new StreamReader("stopwords.txt");
            string read = sr.ReadToEnd();
            List<string> documentWords = s.Split(' ').ToArray().ToList();
            List<string> stopWords = read.Split('\r').ToArray().ToList();

            for (int i = 0; i < stopWords.Count(); i++)
            {
                stopWords[i] = stopWords[i].Trim();
            }

            List<string> finalList = new List<string>();
            bool add = true;
            //Stop word implementation here.
            for (int i = 0; i < documentWords.Count(); i++)
            {
                foreach (string word in stopWords)
                {
                    if (documentWords[i].ToLower() == word)
                    {
                        add = false;
                    }
                }
                if (add)
                    finalList.Add(documentWords[i]);
                add = true;
            }
            s = (string.Join(" ", finalList));
            Regex trimmer = new Regex(@"\s\s+");
            s = trimmer.Replace(s, " ");
            
            LementizeWords(s);
            return s;
        }
        public string LementizeWords(string s)
        {
            List<string> wordList = s.Split(' ').ToArray().ToList();
            List<string> finalList = new List<string>();
            List<char> charList;
            s.ToLower();

            foreach(string str in wordList)
            {
                charList = str.ToCharArray().ToList();

                if (3 < charList.Count())
                {
                    //removes ies
                    if (charList[charList.Count() - 3] == 'i' && charList[charList.Count() - 2] == 'e' && charList[charList.Count() - 1] == 's')
                    {
                        Console.WriteLine("ies");
                    }
                    //removes s
                    else if (charList[charList.Count() - 1] == 's')
                    {
                        //Console.WriteLine("s");
                        charList[charList.Count() - 1] = ' ';
                        finalList.Add(new string (charList.ToArray()));
                    }
                    else
                    {
                        finalList.Add(new string(charList.ToArray()));
                    }
                }
                else
                {
                    finalList.Add(new string(charList.ToArray()));
                }
            }
            
            return s;
        }
    }
}
