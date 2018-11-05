using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Party_Affilication_Classifier
{
    class AI
    {
        //All the possible parties
        List<string> allParties = new List<string> { "Labour", "Conservative", "Coalition" };
        //The list of parties and all there associated data.
        private List<Party> m_PartyList = new List<Party>();
        public List<Party> getPartyList { set { m_PartyList = value; } }

        //The list of files that the user has selected.
        private FileInfo[] files = null;
        //The contents of the file the user is consulting.
        private string fileContent;

        /// <summary>
        /// Allows the user to select the files they want to use for training or the file they wish to consult.
        /// </summary>
        /// <param name="Training">Is the user trying to train the program? True/False</param>
        public void SelectFile(bool Training)
        {
            DirectoryInfo d;
            string selection;
            bool retry = false;

            if (Training)
            {
                d = new DirectoryInfo("TrainingFiles");
                files = d.GetFiles("*.txt");
                
                do
                {
                    retry = false;
                    Console.WriteLine("Please select which files you'd like to use split up by a comma");

                    for (int i = 0; i < files.Count(); i++)
                    {
                        Console.WriteLine(i + 1 + ") " + files[i].Name);
                    }

                    selection = Console.ReadLine();
                    string[] splitSelection = selection.Split(',');
                    d = new DirectoryInfo("TestFiles");
                    Console.Clear();
                    Console.WriteLine("You have selected: ");
                    try
                    {
                        foreach (string s in splitSelection)
                        {
                            Console.WriteLine(files[int.Parse(s) - 1].Name);
                        }
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid Selection. Please try again.\nPRESS ENTER TO CONTINUE");
                        Console.ReadLine();
                        Console.Clear();
                        retry = true;
                    }
                } while (retry == true);
            }
            else if(!Training)
            {
                d = new DirectoryInfo("TestFiles");
                files = d.GetFiles("*.txt");
                StreamReader sr;
                do
                {
                    retry = false;
                    Console.WriteLine("Please select which file you would like to consult.");

                    for (int i = 0; i < files.Count(); i++)
                    {
                        Console.WriteLine(i + 1 + ") " + files[i].Name);
                    }
                    selection = Console.ReadLine();
                    try
                    {
                        FileInfo f = files[int.Parse(selection) - 1];

                        sr = new StreamReader(@"TestFiles\" + f.Name);
                        fileContent = sr.ReadToEnd();
                        sr.Close();
                    }
                    catch
                    {
                        Console.Clear();
                        retry = true;
                        Console.WriteLine("Invalid Selection. Please ensure you are only entering one number.\nPRESS ENTER TO CONTINUE");
                        Console.ReadLine();
                        Console.Clear();
                    }

                } while (retry == true);
            }
            Console.Clear();
        }

        #region AITrainingMethods
        /// <summary>
        /// Gets the party affilication of all of the selected files
        /// </summary>
        public void GetCategories()
        {
            List<string> filteredCats = new List<string>();
            List<Party> filteredParties = new List<Party>();
            foreach (FileInfo f in files)
            {
                foreach (string p in allParties)
                {
                    if (f.Name.ToLower().Contains(p.ToLower()))
                    {
                        if (!filteredCats.Any(x => x.ToString() == p))
                        {
                            filteredCats.Add(p);
                            filteredParties.Add(new Party(p));
                        }
                    }
                }
            }
            m_PartyList = filteredParties;
        }
        /// <summary>
        /// Sorts all of the files into the party list
        /// </summary>
        public void sortFiles()
        {
            StreamReader sr;
            int totalFiles = 0;
            foreach (FileInfo f in files)
            {
                foreach (Party p in m_PartyList)
                {
                    if (f.Name.Contains(p.getName))
                    {
                        sr = new StreamReader(@"TrainingFiles\" + f.Name);
                        string content = sr.ReadToEnd();
                        p.getSpeechList.Add(new Speech(f.Name, content));
                        sr.Close();
                    }
                }
            }
            //get the total number of files.
            foreach (Party p2 in m_PartyList)
            {
                totalFiles = totalFiles + p2.getSpeechList.Count();
            }
            //calculate the Pcatagory
            foreach (Party p3 in m_PartyList)
            {
                p3.getProbability = ((double)p3.getSpeechList.Count() / totalFiles);
            }
        }

        /// <summary>
        /// Reads all of he words in each file and assigns them as words used by a particular party.
        /// </summary>
        public void TrainingWords()
        {
            Filter fil = new Filter();
            StreamReader sr;
            //for each party.
            foreach (Party p in m_PartyList)
            {
                //for each speech assosiated with the party
                foreach (Speech f in p.getSpeechList)
                {
                    //split all the words in the speech
                    sr = new StreamReader(@"TrainingFiles\" + f.getName);
                    string str = sr.ReadToEnd();
                    str = str.ToLower();
                    str = fil.RemoveAll(str);
                    string[] splitWords = str.Split(' ');
                    //for each word in the speech
                    for (int i = 0; i < splitWords.Count(); i++)
                    {
                        if (p.getWordList.Count() == 0)
                            p.getWordList.Add(new Word(splitWords[i], 1, 0));

                        //checks if the word is in the wordlist currently.
                        if (p.getWordList.Any(x => x.ToString() == splitWords[i]))
                        {
                            //finds the word and adds one to the frequency of the word
                            p.getWordList[i].getFreq++;
                        }
                        else
                        {
                            //if the word is not in the word list adds the word to the word list with a frequency of 1.
                            p.getWordList.Add(new Word(splitWords[i], 1, 0));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Gives the probability of each word being in the catagory mentioned.
        /// </summary>
        public void getWordProbability()
        {
            double totalWords = 0;
            foreach (Party p in m_PartyList)
            {
                totalWords = totalWords + p.getWordList.Count();
            }
            foreach (Party p in m_PartyList)
            {
                Console.WriteLine("" + p.getName + "");
                foreach (Word kvp in p.getWordList)
                {
                    kvp.getProbability = ((double)(kvp.getProbability + 1) / (p.getWordList.Count() + totalWords));
                }
            }
            Console.Clear();

        }
        /// <summary>
        /// Saves the current word probabilities and frequencies.
        /// </summary>
        public void SaveTraining()
        {
            StreamWriter sr = new StreamWriter("WordProbability.txt");
            foreach (Party p in m_PartyList)
            {
                sr.WriteLine(p.getName);
                foreach (Word kvp in p.getWordList)
                {
                    sr.WriteLine("Word: " + kvp.getWord + ", Frequency: " + kvp.getFreq + ", Probability: " + p.getProbability);
                }
            }

            XmlSerializer xml = new XmlSerializer(typeof(Party));
            int i = 0;
            XmlWriter xmlw;
            foreach (Party p in m_PartyList)
            {
                Party party = p;
                xmlw = new XmlTextWriter(@"TrainingData\Party" + i + ".xml", Encoding.UTF8);
                xml.Serialize(xmlw, party);
                i++;
                xmlw.Close();
            }
        }
        #endregion

        #region Consult
        /// <summary>
        /// Calculates the probability that the file is associatated with each party.
        /// </summary>
        public void CalculateParty()
        {
            Dictionary<string, double> calculatedParty = new Dictionary<string, double>();
            List<string> fileWords = new List<string>();
            Dictionary<string, double> commonWords = new Dictionary<string, double>();
            fileWords = fileContent.Split(' ').ToArray().ToList();

            //removes the grammar and stop words from the document.
            double probability = 0;
            fileWords = fileContent.Split(' ').ToArray().ToList();
            bool addWord = true;
            double totalProb = 0;
            //for each party
            foreach (Party p in m_PartyList)
            {
                //for each word assosiated with that party.
                foreach (Word w in p.getWordList)
                {
                    //if the words are in the party word list and the documents word list
                    if (fileWords.Any(x => x.ToString() == w.getWord))
                    {
                        //check if it's been added before
                        foreach (KeyValuePair<string, double> kvp in commonWords)
                        {
                            if (kvp.Key == w.getWord)
                            {
                                addWord = false;
                            }
                        }
                        //if it hasn't add it.
                        if (addWord)
                        {
                            commonWords.Add(w.getWord, w.getProbability);
                        }

                    }
                    //reset the addword bool.
                    addWord = true;
                }
                //calculate the probability that the document is assosiated with that party.
                foreach (KeyValuePair<string, double> kvp in commonWords)
                {
                    if (probability == 0)
                        probability = Math.Log((double)kvp.Value);
                    else
                        probability += Math.Log(kvp.Value);
                }
                p.getDocumentProbability = probability;
                totalProb += probability;
                
                probability = 0;
                commonWords.Clear();
            }
            string HighestParty = "";
            double HighestValue = 0;
            foreach (Party p in m_PartyList)
            {
                p.PartyPercentage = (p.getDocumentProbability / totalProb)*100;
                if (p.getDocumentProbability < HighestValue)
                {
                    HighestValue = p.getDocumentProbability;
                    HighestParty = p.getName;
                }

                Console.WriteLine("Probability of " + p.getName + ": " + p.getDocumentProbability + " with " + p.PartyPercentage + "%");
            }
            Console.WriteLine("The document is most likely: " + HighestParty);
            Console.ReadLine();
        }
        /// <summary>
        /// Calculates the probability that each of the files is associated with each of the parties
        /// </summary>
        public void CalculatePartyTFIDF()
        {
            int TotalDocs=0;
            List<Word> CommonWords = new List<Word>();
            foreach(Party p in m_PartyList)
            {
                TotalDocs = TotalDocs + p.getSpeechList.Count();
            }
            //total words
            //total num of scripts (in entire program)
            //the number of scripts containing that word.
            List<string> fileWords = fileContent.Split(' ').ToArray().ToList();
            bool add = true;

            //gets all of the words and the number of times they have appeared in the document. 
            for (int i = 0; i < fileWords.Count(); i++)
            {
                foreach(Word w in CommonWords)
                {
                    if (fileWords[i] == w.getWord)
                        add = false;
                }
                if (add)
                    CommonWords.Add(new Word(fileWords[i],1,0));
                else if(!add)
                {
                    foreach(Word w in CommonWords)
                    {
                        if(w.getWord == fileWords[i])
                        {
                            w.getFreq++;
                        }
                    }
                    add = true;
                }
            }

            int TotalWordDocs = 0;
            foreach (Word w in CommonWords)
            {
                foreach (Party p in m_PartyList)
                {
                    foreach (Speech s in p.getSpeechList)
                    {
                        List<string> words = s.getContent.Split(' ', '\n').ToArray().ToList();

                        foreach (string str in words)
                        {
                            if (str.ToLower() == w.getWord.ToLower())
                            {
                                TotalDocs++;
                            }
                        }
                    }
                }
                w.CalculateTFIDF(TotalDocs, CommonWords.Count(), TotalWordDocs);
                Console.WriteLine(w.getTFIDF);
            }
            //TFIDF FOR EACH WORD. THEN TIMES PROBABILITIES WITH ONE ANOTHER LIKE WITH THE LAST THING YOU DONE. (TIMES THEM TOGETHER THINGY) 
            Console.ReadLine();


            //count number of occurences each word has. 
        }
        private void CalculatePartyNgrams()
        {
            Dictionary<string, int> NgramsDict = new Dictionary<string, int>();
        }
        #endregion
    }
}
    
