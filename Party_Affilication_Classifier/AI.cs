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

        //a list of all the files in the TrainingFiles directory.
        private FileInfo[] Unfilteredfiles = null;
        //The list of files that the user has selected.
        private List<FileInfo> files = new List<FileInfo>();

        private FileInfo consultFile;
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
                if (!Directory.Exists("TrainingFiles"))
                    Directory.CreateDirectory("TrainingFiles");

                d = new DirectoryInfo("TrainingFiles");
                Unfilteredfiles = d.GetFiles("*.txt");
                
                do
                {
                    retry = false;
                    Console.WriteLine("Please select which files you'd like to use split up by a comma");

                    for (int i = 0; i < Unfilteredfiles.Count(); i++)
                    {
                        Console.WriteLine(i + 1 + ") " + Unfilteredfiles[i].Name);
                    }

                    selection = Console.ReadLine();
                    string[] splitSelection = selection.Split(',');

                    if (!Directory.Exists("TestFiles"))
                        Directory.CreateDirectory("TestFiles");

                    d = new DirectoryInfo("TestFiles");
                    Console.Clear();
                    Console.WriteLine("You have selected: ");
                    try
                    {
                        for(int i=0;i<splitSelection.Count();i++)
                        {
                            Console.WriteLine(Unfilteredfiles[int.Parse(splitSelection[i])-1].Name);
                            files.Add(Unfilteredfiles[i]);
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
                Unfilteredfiles = d.GetFiles("*.txt");
                StreamReader sr;
                do
                {
                    retry = false;
                    Console.WriteLine("Please select which file you would like to consult.");

                    for (int i = 0; i < Unfilteredfiles.Count(); i++)
                    {
                        Console.WriteLine(i + 1 + ") " + Unfilteredfiles[i].Name);
                    }
                    selection = Console.ReadLine();
                    try
                    {
                        FileInfo f = Unfilteredfiles[int.Parse(selection) - 1];
                        consultFile = f;
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
                    sr.Close();
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
            if (!Directory.Exists("TrainingData"))
                Directory.CreateDirectory("TrainingData");

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
            List<string> SpeechWordsNew = new List<string>();
            List<string> fileContentNew = new List<string>();
            List<string> SpeechNgram = new List<string>();
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

            int PartyNum=0;
            //for each party
            foreach(Party p in m_PartyList)
            {
                PartyNum++;
                Console.WriteLine("Calculating Probabilities...\nThis may take some time... \nParty:" + PartyNum + "/" + m_PartyList.Count());
                //for each speech associated with that party.
                foreach (Speech s in p.getSpeechList)
                {
                    SpeechWordsNew = s.NgramFromSpeech();
                }
                //Grabs and properly splits the Ngram File.
                List<string> NgramDicTemp = new List<string>();
                NgramDicTemp = File.ReadAllLines("Ngrams.txt").ToList();
                List<string> NgramDic = new List<string>();
                
                foreach(string s in NgramDicTemp)
                {
                    string[] tempStr = new string[2];
                    tempStr = s.Split('/').ToArray();

                    if(tempStr.Count() >= 2)
                        NgramDic.Add(tempStr[0] + " " + tempStr[1]);
                }

                //Compares the speechword Ngram with the NgramDic
                foreach(string s in NgramDic)
                {
                    foreach(string ss in SpeechWordsNew)
                    {
                        if (s == ss)
                        {
                            SpeechNgram.Add(s);
                        }
                    }
                }
                Console.Clear();

                //Adapted TFIDF code.
                totalWords += SpeechNgram.Count();
                //for each string in the speech
                foreach (string str in SpeechNgram)
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
                SpeechNgram.Clear();
                SpeechWordsNew.Clear();
                totalWords = 0;
            }
        }
        /// <summary>
        /// Prints the probabilities that the file belongs to each of the parties (prints results of CalculateParty, CalculatePartyTFIDF and CalculatePartyNgrams)
        /// </summary>
        public void PrintValues()
        {
            string HighestParty = "", HighestPartyTFIDF="", HighestPartyNgrams="";
            double HighestValue = 0, HighestValueTFIDF=0, HighestValueNgrams=0;
            double total=0, totalTFIDF=0, totalNgrams=0;
            List<string> outputs = new List<string>();

            if (!Directory.Exists("OutputFiles"))
                Directory.CreateDirectory("OutputFiles");

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
                outputs.Add("Using General Probability: " + p.getName + " " + ((p.getDocumentProbability * -1) / total) * 100 + "%");
            }
            outputs.Add("The document is most likely: " + HighestParty + Environment.NewLine);

            foreach (Party p in m_PartyList)
            {
                totalTFIDF += p.getTFIDF;
            }
            totalTFIDF *= -1;
            //TFIDF Percentages
            foreach (Party p in m_PartyList)
            {
                outputs.Add("Using TFIDF: " + p.getName + " " + ((p.getTFIDF * -1) / total) * 100 + "%");
                if (p.getTFIDF < HighestValueTFIDF || HighestValueTFIDF == 0)
                {
                    HighestValueTFIDF = p.getTFIDF;
                    HighestPartyTFIDF = p.getName;
                }
            }
            outputs.Add("The document is most likely: " + HighestPartyTFIDF + Environment.NewLine);

            foreach (Party p in m_PartyList)
            {
                totalNgrams += p.getNgrams;
            }
            totalNgrams *= -1;
            //Ngram Percentages
            foreach (Party p in m_PartyList)
            {
                outputs.Add("Using Ngrams and TFIDF " + p.getName + " " + ((p.getNgrams * -1) / totalNgrams) * 100 + "%");
                if (p.getNgrams < HighestValueNgrams || HighestValueNgrams == 0)
                {
                    HighestValueNgrams = p.getNgrams;
                    HighestPartyNgrams = p.getName;
                }
            }
            outputs.Add("The document is most likely: " + HighestPartyNgrams);

            //outputs to the console.
            foreach(string str in outputs)
            {
                Console.WriteLine(str);
            }
            //outputs to a file.
            File.WriteAllLines(consultFile.Name, outputs);
            Console.ReadLine();
        }
        #endregion
    }
}
    
