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
    /// <summary>
    /// Contains all of the formula's and Artifical Intelligence aspects of the program.
    /// </summary>
    class AI
    {
        //All the possible parties
        List<string> allParties = new List<string> { "Labour", "Conservative", "Coalition" };
        //The list of parties and all there associated data.
        private List<Party> m_PartyList = new List<Party>();
        public List<Party> getPartyList { set { m_PartyList = value; } }
        int totalDocs=0;

        //The list of files that the user has selected.
        private FileInfo[] files = null;
        //The contents of the file the user is consulting.
        List<string> fileContent = new List<string>();

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
                Filter filter = new Filter();
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
                        fileContent = filter.RemoveAll(sr.ReadToEnd().ToLower()).Split(' ', '\n').ToArray().ToList();
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
                totalDocs = totalDocs + p2.getSpeechList.Count();
            }
            //calculate the Pcatagory
            foreach (Party p3 in m_PartyList)
            {
                p3.getPcata = ((double)p3.getSpeechList.Count() / totalDocs);
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
                    sr.WriteLine("Word: " + kvp.getWord + ", Frequency: " + kvp.getFreq + ", Probability: " + p.getPcata);
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
            Dictionary<string, double> commonWords = new Dictionary<string, double>();

            //gets the total number of documents.
            foreach(Party p in m_PartyList)
            {
                totalDocs += p.getSpeechList.Count();
            }

            //removes the grammar and stop words from the document.
            double probability = 0;
            bool addWord = true;
            //for each party
            foreach (Party p in m_PartyList)
            {
                //for each word assosiated with that party.
                foreach (Word w in p.getWordList)
                {
                    //if the words are in the party word list and the documents word list
                    if (fileContent.Any(x => x.ToString() == w.getWord))
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
                        probability = Math.Log(kvp.Value);
                    else
                        probability += Math.Log(kvp.Value);
                }
                p.getDocumentProbability = probability;
                probability = 0;
                commonWords.Clear();
            }
        }
        /// <summary>
        /// Calculates the probability that each of the file is associated with each of the parties using TFIDF
        /// </summary>
        public void CalculatePartyTFIDF()
        {
            int totalWords = 0;
            List<string> SpeechWords = new List<string>();
            List<Word> CommonWords = new List<Word>();
            bool add = false;
            // for each party
            foreach (Party p in m_PartyList)
            {
                //for each speech
                foreach (Speech s in p.getSpeechList)
                {
                    //make a list of words.
                    SpeechWords = s.getContent.ToLower().Split(' ', '\n').ToArray().ToList();
                    //get the total words amongst all documents assosiated with that party.
                    totalWords += SpeechWords.Count();
                    //for each string in the speech
                    foreach (string str in SpeechWords)
                    {
                        //for each word in the document being classified.
                        foreach (string str2 in fileContent)
                        {
                            //if they are in both documents.
                            if (str2.ToLower() == str.ToLower())
                            {
                                add = true;
                            }
                        }
                        //add  the word to the common words
                        if (add)
                        {
                            if (CommonWords.Any(x => x.getWord == str))
                            {
                                foreach (Word w in CommonWords)
                                {
                                    if (w.getWord == str)
                                    {
                                        w.getFreq++;
                                    }
                                }
                            }
                            else
                            {
                                CommonWords.Add(new Word(str, 1, 0));
                            }
                            add = false;
                        }
                    }
                }
                foreach (Word word2 in CommonWords)
                {
                    word2.CalculateTFIDF(totalWords, p.getSpeechList.Count(), fileContent.Count());
                    p.getTFIDF += word2.getTFIDF;
                }

                CommonWords.Clear();
                totalWords = 0;
            }
        }
        /// <summary>
        /// Uses Ngrams and TFIDF to calculate the probability that the file is associated with each of the parties
        /// </summary>
        public void CalculatePartyNgrams()
        {
            List<string> SpeechWords = new List<string>();
            List<string> SpeechWordsNew = new List<string>();
            List<string> fileContentNew = new List<string>();
            List<Word> CommonWords = new List<Word>();
            int totalWords=0;
            string temp = "";
            bool add = false;

            //link two words together in filecontent
            for(int i =0;i < fileContent.Count()-1;i++)
            {
                temp = fileContent[i];
                i++;
                temp += " " + fileContent[i];
                fileContentNew.Add(temp);
                temp = "";
            }
            //for each party
            foreach(Party p in m_PartyList)
            {
                //for each speech associated with that party.
                foreach(Speech s in p.getSpeechList)
                {
                    SpeechWords = s.getContent.ToLower().Split(' ', '\n').ToArray().ToList();
                    //add two words together.
                    for(int i =0;i < SpeechWords.Count();i++)
                    {
                        temp = SpeechWords[i];
                        i++;
                        if (!(SpeechWords.Count()-1 < i))
                        {
                            temp += " " + SpeechWords[i];
                            
                        }
                        SpeechWordsNew.Add(temp);
                        temp = "";
                    }
                }

                //Adapted TFIDF code.
                totalWords += SpeechWordsNew.Count();
                //for each string in the speech
                foreach (string str in SpeechWordsNew)
                {
                    //for each word in the document being classified.
                    foreach (string str2 in fileContentNew)
                    {
                        //if they are in both documents.
                        if (str2.ToLower() == str.ToLower())
                        {
                            add = true;
                        }
                    }
                    //add  the word to the common words
                    if (add)
                    {
                        if (CommonWords.Any(x => x.getWord == str))
                        {
                            foreach (Word w in CommonWords)
                            {
                                if (w.getWord == str)
                                {
                                    w.getFreq++;
                                }
                            }
                        }
                        else
                        {
                            CommonWords.Add(new Word(str, 1, 0));
                        }
                        add = false;
                    }
                }
                //calculate the TFIDF for each Ngram word (word family)
                foreach (Word word2 in CommonWords)
                {
                    word2.CalculateTFIDF(totalWords, p.getSpeechList.Count(), fileContentNew.Count());
                    p.getNgrams += word2.getTFIDF;
                }
                CommonWords.Clear();
                totalWords = 0;
            }
        }
        /// <summary>
        /// Prints the probabilities that the file belongs to each of the parties (prints results of CalculateParty, CalculatePartyTFIDF and CalculatePartyNgrams)
        /// </summary>
        public void PrintValues()
        {
            string HighestParty = "";
            double HighestValue = 0;
            double total=0;

            foreach (Party p in m_PartyList)
            {
                total += p.getDocumentProbability;
            }
            total *= -1;
            //Normal Percentages
            foreach (Party p in m_PartyList)
            {

                if (p.getDocumentProbability < HighestValue || HighestValue == 0)
                {
                    HighestValue = p.getDocumentProbability;
                    HighestParty = p.getName;
                }
                Console.WriteLine("Using Maths: " + p.getName + " " + ((p.getDocumentProbability * -1) / total) * 100 + "%");
            }
            Console.WriteLine("The document is most likely: " + HighestParty + "\n\n\n");
            HighestValue = 0;
            HighestParty = "";
            total = 0;

            foreach (Party p in m_PartyList)
            {
                total += p.getTFIDF;
            }
            total *= -1;
            //TFIDF Percentages
            foreach (Party p in m_PartyList)
            {
                Console.WriteLine("Using TFIDF: " + p.getName + " " + ((p.getTFIDF * -1) / total) * 100 + "%");
                if (p.getTFIDF < HighestValue || HighestValue == 0)
                {
                    HighestValue = p.getTFIDF;
                    HighestParty = p.getName;
                }
            }
            Console.WriteLine("The document is most likely: " + HighestParty + "\n\n\n");
            HighestValue = 0;
            HighestParty = "";
            total = 0;

            foreach (Party p in m_PartyList)
            {
                total += p.getNgrams;
            }
            total *= -1;
            //Ngram Percentages
            foreach (Party p in m_PartyList)
            {
                Console.WriteLine("Using Ngrams and TFIDF " + p.getName + " " + ((p.getNgrams * -1) / total) * 100 + "%");
                if(p.getNgrams < HighestValue || HighestValue == 0)
                {
                    HighestValue = p.getNgrams;
                    HighestParty = p.getName;
                }
            }
            Console.WriteLine("The document is most likely: " + HighestParty);
            Console.ReadLine();
        }
        
        #endregion
    }
}
    
