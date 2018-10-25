using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Party_Affilication_Classifier
{
    class AIConsult
    {
        public FileInfo SelectFile()
        {
            FileInfo consultFile = null;
            DirectoryInfo d = new DirectoryInfo("TestFiles");
            FileInfo[] files = d.GetFiles("*.txt");
            bool retry = false;
            char selection;
            do
            {
                Console.Clear();
                Console.WriteLine("Please select the file you want to evaluate");
                Console.WriteLine(files.Count() + " files found.");

                for (int i = 0; i < files.Count(); i++)
                {
                    Console.WriteLine(i + ") " + files[i].Name);
                }
                try
                {
                    int choice = int.Parse(Console.ReadLine());
                    consultFile = files[choice];
                    Console.WriteLine(consultFile.Name + " has been selected");
                    Console.ReadLine();
                }
                catch
                {
                    Console.WriteLine("Invalid Selection");
                    Console.WriteLine("Would you like to try again? Y/N");

                    selection = char.Parse(Console.ReadLine());

                    if (selection == 'Y' || selection == 'y')
                        retry = true;
                    else
                        retry = false;
                }
            }while(retry == true);

            return consultFile;
        }
        public void CalculateParty(List<Party> partyList, string fileContent)
        {
            Dictionary<string, double> calculatedParty = new Dictionary<string, double>();
            List<string> fileWords = new List<string>();
            Dictionary<string,double> commonWords = new Dictionary<string, double>();

            fileWords = fileContent.Split(' ').ToArray().ToList();
            /*
             * P(cata/doc) = P(word1/cata) x P(word2/cata) x …P(wordi/cata) x P(cata)
             * P(catb/doc) = P(word1/catb) x P(word2/catb) x …P(wordi/catb) x P(catb)
             * 
             * P(word1/cata) is something you have already worked out. You just need to times the probability of each of the words together then times it by P(cata) which you will need to
             * calculate and save. This is NOT the number of documents for that catagory it is the number of catagories divided by the total number of documents it will be a decimal.
             * 
             * Do this for all catagories and you will know which one of the catagories it is likely to be.
             */
            //removes the grammar and stop words from the document.
            double probability=0;
            foreach(string s in fileWords)
            {
                foreach(Party p in partyList)
                {
                    foreach(KeyValuePair<string,double> kvp in p.getWordProbabilities)
                    {
                        if(s == kvp.Key)
                        {
                            commonWords.Add(s,kvp.Value);
                        }
                    }
                    foreach(KeyValuePair<string,double> kvp2 in commonWords)
                    {
                        if (probability == 0)
                            probability = kvp2.Value;
                        else
                            probability = probability * kvp2.Value;
                    }
                    //probability * p.documentCount / totalDocs
                    p.getProbability = probability * p.getProbability;
                }
            }
            foreach(Party p in partyList)
            {
                Console.WriteLine("Party Name: " + p.getName + " Probability: " + p.getProbability);
            }
            Console.ReadLine();
        }
    }
}
