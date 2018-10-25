using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            int totalFiles=0;
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
            //get the total number of files.
            foreach(Party p2 in partyList)
            {
                totalFiles = totalFiles + p2.getSpeechList.Count();
            }
            //calculate the Pcatagory
            foreach(Party p3 in partyList)
            {
                p3.getProbability = p3.getSpeechList.Count() / totalFiles;
            }
        }
        public void getDocuments()
        {

        }
        /// <summary>
        /// Reads all of he words in each file and assigns them as words used by a particular party.
        /// </summary>
        public void TrainingWords(List<Party> partyList)
        {
            Filter fil = new Filter();
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
                    str = fil.removeStopwords(fil.removeGrammar(str));
                    string[] splitWords = str.Split(' ');
                    //for each word in the speech
                    foreach(string s in splitWords)
                    {
                        //checks if the word is in the wordlist currently.
                        if(p.getWordFreq.ContainsKey(s))
                        {
                            //finds the word and adds one to the frequency of the word
                            p.getWordFreq[s]++;
                        }
                        else
                        {
                            //if the word is not in the word list adds the word to the word list with a frequency of 1.
                            p.getWordFreq.Add(s, 1);
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
                }
            }
            Console.Clear();
            StreamWriter sr = new StreamWriter("WordProbability.txt");

            foreach(Party p in partyList)
            {
                sr.WriteLine(p.getName);
                foreach(KeyValuePair<string,int> kvp in p.getWordFreq)
                {
                    sr.WriteLine("Word: " + kvp.Key + ", Frequency: " + kvp.Value + ", Probability: " + p.getWordProbabilities[kvp.Key]);
                }
            }
        }
    }
}
