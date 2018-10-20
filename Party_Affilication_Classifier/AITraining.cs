using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    public class AITraining
    {
        /// <summary>
        /// Gets the party affilication of all of the selected files
        /// </summary>
        /// <param name="files">The array of files.</param>
        /// <param name="allCategories">All the possible catagories</param>
        /// <param name="selectedCats">The list of filtered catagories</param>
        public List<Party> GetCategories(FileInfo[] files, List<string> allCategories)
        {
            List<string> filteredCats = new List<string>();
            List<Party> filteredParties = new List<Party>();
            foreach (FileInfo f in files)
            {
                foreach (string c in allCategories)
                {
                    if (f.Name.Contains(c))
                    {
                        if (!filteredCats.Any(x => x.ToString() == c))
                        {
                            filteredCats.Add(c);
                            filteredParties.Add(new Party(c));
                        }
                    }
                }
            }
            return filteredParties;
        }
        /// <summary>
        /// Sorts all of the files into the party list
        /// </summary>
        /// <param name="files"></param>
        /// <param name="partList"></param>
        public void sortFiles(FileInfo[] files, List<Party> partyList)
        {
            foreach(FileInfo f in files)
            {
                foreach(Party p in partyList)
                {
                    if(f.Name.Contains(p.getName))
                    {
                        p.getSpeechList.Add(f);
                    }
                }
            }
        }
        /// <summary>
        /// Reads all of he words in each file and assigns them as words used by a particular party.
        /// </summary>
        public void TrainingWords(List<Party> partyList)
        {
            StreamReader sr;
            //for each party.
            foreach(Party p in partyList)
            {
                //for each speech assosiated with the party
                foreach(FileInfo f in p.getSpeechList)
                {
                    //split all the words in the speech
                    sr = new StreamReader(@"TrainingFiles\" + f.Name);
                    string str = sr.ReadToEnd();
                    string[] splitWords = str.Split(' ');
                    
                    //for each word in the speech
                    foreach(string s in splitWords)
                    {
                        str = removeGrammar(s);
                        //checks if the word is in the wordlist currently.
                        if(p.getWordFreq.ContainsKey(str))
                        {
                            //finds the word and adds one to the frequency of the word
                            p.getWordFreq[str]++;
                        }
                        else
                        {
                            //if the word is not in the word list adds the word to the word list with a frequency of 1.
                            p.getWordFreq.Add(str, 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Gives the probability of each word being in the catagory mentioned.
        /// </summary>
        public void getWordProbability(List<Party> partyList)
        {
            double totalWords=0;
            foreach(Party p in partyList)
            {
                totalWords = totalWords + p.getWordFreq.Count();
            }
            foreach(Party p in partyList)
            {
                Console.WriteLine("--- " + p.getName + "---");
                foreach(KeyValuePair<string,int> kvp in p.getWordFreq)
                {
                    p.getWordProbabilities.Add(kvp.Key,(double)(kvp.Value + 1) / (p.getWordFreq.Count() + totalWords));
                    //output to a file at some point.
                }
            }
        }
        private string removeGrammar(string s)
        {
            List<char> forbiddenChars = new List<char> { '"', ':', ';', '\n', '\t', '.', ',', '\r' };
            s = s.Trim(new char[] {'"', ':', ';', '\n', '\t', '.', ',', '\r'});
            return s;
        }
    }
}
