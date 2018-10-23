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
            DirectoryInfo d = new DirectoryInfo("TestFiles");
            FileInfo[] files = d.GetFiles("*.txt");

            Console.Clear();
            Console.WriteLine("Please select the file you want to evaluate");
            Console.WriteLine(files.Count() + " files found.");

            for (int i = 0; i < files.Count(); i++)
            {
                Console.WriteLine(i + ") " + files[i].Name);
            }
            int choice = int.Parse(Console.ReadLine());
            FileInfo consultFile = files[choice];
            Console.WriteLine(consultFile.Name + " has been selected");
            Console.ReadLine();
            return consultFile;
        }
        public void CalculateParty(List<Party> partyList)
        {
            Dictionary<string, double> calculatedParty = new Dictionary<string, double>();
            /*
             * P(cata/doc) = P(word1/cata) x P(word2/cata) x …P(wordi/cata) x P(cata)
             * P(catb/doc) = P(word1/catb) x P(word2/catb) x …P(wordi/catb) x P(catb)
             * 
             * for each word divide by the number of words in the party and then at the end times by the number of words in the document.
             */
             foreach(Party p in partyList)
            {
                foreach(KeyValuePair<string, int> kvp in p.getWordFreq)
                {
                    //calculation?
                    
                }
            }

        }
    }
}
