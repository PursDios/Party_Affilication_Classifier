using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    public class Controller
    {
        //All the possible categories of government (E.G labour, conservative etc. This is to allow the program to easily be expanded upon).
        List<string> allCategories = new List<string> { "Labour", "Conservative", "Coalition" };
        List<Party> partyList = new List<Party>();

        /// <summary>
        /// Allows the user to navigate the program
        /// </summary>
        public void MainMenu()
        {
            bool retry = true;
            char choice=' ';
            do
            {
                Console.WriteLine("1) Undertake Training");
                Console.WriteLine("2) Undertake a Classification");
                Console.WriteLine("Q) Exit");
                try
                {
                    choice = char.Parse(Console.ReadLine());
                    choice = char.ToLower(choice);
                    retry = false;
                }
                catch
                {
                    Console.WriteLine("Invalid Selection. Please try again");
                    Console.ReadLine();
                }
            
                switch(choice)
                {
                    case '1':
                        Training();
                        break;
                    case '2':
                        Consult();
                        break;
                    case 'Q':
                        break;
                    default:
                        Console.WriteLine("Please only input 1, 2 or Q");
                        Console.ReadLine();
                        retry = true;
                        break;
                }
            } while (retry != false);
        }
        /// <summary>
        /// Calls all of the nessecary methods required to train the program correctly.
        /// </summary>
        private void Training()
        {
            AITraining AI = new AITraining();

            DirectoryInfo d = new DirectoryInfo("TrainingFiles");
            FileInfo[] files = d.GetFiles("*.txt");
            Console.WriteLine("Please select which files you'd like to use split up by a comma");

            int i = 0;
            foreach (FileInfo f in files)
            {
                i++;
                Console.WriteLine(i + ") " + f.Name);
            }
            string selection = Console.ReadLine();
            string[] splitSelection = selection.Split(',');

            Console.Clear();
            Console.WriteLine("You have selected: ");
            foreach (string s in splitSelection)
            {
                Console.WriteLine(files[int.Parse(s) - 1].Name);
            }
            Console.ReadLine();
            Console.Clear();

            partyList = AI.GetCategories(files, allCategories);
            AI.sortFiles(files, partyList);
            AI.TrainingWords(partyList);
            AI.getWordProbability(partyList);

            Consult();
        }
        /// <summary>
        /// Method used to determine the party affilication of a given file.
        /// </summary>
        private void Consult()
        {
            Filter f = new Filter();
            StreamReader sr;
            //ADD VALIDATION TO CHECK TRAINING HAS BEEN DONE HERE.
            if(partyList.Count() == 0)
            {
                Console.WriteLine("No training has been performed during this run. \nLoading prior training...");
                LoadPriorTraining();
            }

            AIConsult AI = new AIConsult();
            FileInfo consultFile;

            try
            {
                consultFile = AI.SelectFile();
                sr = new StreamReader(@"TestFiles\" + consultFile.Name);
                string fileContent = f.removeStopwords(f.removeGrammar(sr.ReadToEnd()));
                AI.CalculateParty(partyList, fileContent);
            }
            catch(Exception e)
            {
                //This shouldn't happen...But it's here just incase.
                Console.WriteLine("Oops. Something went wrong!\n\n");
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
        /// <summary>
        /// Loads previously saved training data.
        /// </summary>
        private void LoadPriorTraining()
        {
            //ALL OF THIS WILL NEED TO BE REPLACED WITH SERIALIZATION DUE TO COMPLICATIONS WITH PCATA AND USING THE DATA TO CONSULT MOST OF THE CALCULATED VALUES WILL BE MISSING IF DATA IS NOT SERIALIZED.
            StreamReader sr = new StreamReader("WordProbability.txt");
            int numOfLines = File.ReadAllLines("WordProbability.txt").Length;
            int partyNum = -1;
            //for each line in the document.
            for (int i = 0; i < numOfLines; i++)
            {
                string str = sr.ReadLine();
                //if the word is a party name
                if(allCategories.Any(x => x.ToString() == str))
                {
                    partyList.Add(new Party(str));
                    partyNum++;
                }
                //if the string isn't blank.
                else if(str != "")
                {
                    List<string> stringSplit = new List<string>();
                    List<string> splitViaColon = new List<string>();
                    List<string> sortedValues = new List<string>();

                    //split the line into the word, frequency and probability
                    stringSplit = str.Split(',').ToArray().ToList();

                    foreach(string spl in stringSplit)
                    {
                        //splits the Word:, Frequency: and Probability: From the actual content by ensuring that the relevent information is always stored in an odd element number.
                        splitViaColon = spl.Split(':').ToArray().ToList();
                        foreach(string s in splitViaColon)
                        {
                            sortedValues.Add(s);
                        }
                    }
                    //Populates the word frequency and word probability dictionaries with the relevent information from the text document. 
                    partyList[partyNum].getWordFreq.Add(sortedValues[1].Trim(), int.Parse(sortedValues[3].Trim()));
                    partyList[partyNum].getWordProbabilities.Add(sortedValues[1].Trim(), double.Parse(sortedValues[5].Trim()));
                }
            }
        }
    }
}
