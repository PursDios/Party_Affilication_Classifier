using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Party_Affilication_Classifier
{
    /// <summary>
    /// Filters out unwanted content from strings.
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Removes the grammar, stopwords and lementizes words.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string RemoveAll(string s)
        {
            return LementizeWords(RemoveStopwords(RemoveGrammar(s)));
        }
        /// <summary>
        /// Removes all of the special characters and words in a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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
            
            return s;
        }
        /// <summary>
        /// Lementizes words.
        /// </summary>
        /// <param name="s">The string containing the words you want to Lementize</param>
        /// <returns></returns>
        public string LementizeWords(string s)
        {
            Lementize l = new Lementize();
            List<string> stringWords = new List<string>();
            s.ToLower().Split(' ', '\n').ToArray().ToList();

            foreach(string str in stringWords)
            {
                l.LementizeWord(str);
            }
            return s;
        }
    }
}
