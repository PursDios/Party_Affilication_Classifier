/*
 * Project: Party Classifer
 * Filename: Controller.cs
 * Created: 28/10/2018
 * Edited: 21/11/2018
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Party_Affilication_Classifier
{
    /// <summary>
    /// Controls the entire program. Everything is called from here.
    /// </summary>
    public class Controller
    {
        bool doneTraining = false;
        AI AI = new AI();
        /// <summary>
        /// Allows the user to navigate the program
        /// </summary>
        public void MainMenu()
        {
            bool retry = true;
            char choice=' ';
            do
            {
                Console.Clear();
                Console.WriteLine("1) Undertake Training");
                Console.WriteLine("2) Undertake a Classification");
                Console.WriteLine("Q) Exit");
                try
                {
                    choice = char.Parse(Console.ReadLine());
                    choice = char.ToLower(choice);
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
                        retry = false;
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
            Console.Clear();
            AI.SelectFile(true);
            AI.GetCategories();
            AI.sortFiles();
            AI.TrainingWords();
            AI.getWordProbability();
            AI.SaveTraining();

            doneTraining = true;
            Console.WriteLine("Training Complete\nDo you wish to proceed to consultation? Y/N");
            char choice = char.Parse(Console.ReadLine());
            if(choice == 'Y' || choice == 'y')
                Consult();
        }
        /// <summary>
        /// Method used to determine the party affilication of a given file.
        /// </summary>
        private void Consult()
        {
            Console.Clear();
            if(!doneTraining)
            {
                Console.WriteLine("No training has been performed during this run. \nLoading prior training...");
                LoadPriorTraining();
            }
            AI.SelectFile(false);
            AI.CalculateParty();
            AI.CalculatePartyTFIDF();
            AI.CalculatePartyNgrams();
            AI.PrintValues();
        }
        /// <summary>
        /// Loads previously saved training data from the saved xml serialized files in the TrainingData folder.
        /// </summary>
        private void LoadPriorTraining()
        {
            if (!Directory.Exists("TrainingData"))
                Directory.CreateDirectory("TrainingData");

            List<Party> partyList = new List<Party>();
            DirectoryInfo d = new DirectoryInfo("TrainingData");
            Stream stream;
            FileInfo[] files = d.GetFiles("*.xml");
            XmlSerializer xml = new XmlSerializer(typeof(Party));
            XmlReader xmlRead;

            for (int i = 0; i < files.Count(); i++)
            {
                //deserialize
                stream = new FileStream(@"TrainingData\Party" + i + ".xml", FileMode.Open);
                xmlRead = new XmlTextReader(stream);
                partyList.Add((Party)xml.Deserialize(xmlRead));
                stream.Close();
                xmlRead.Close();
            }
            AI.getPartyList = partyList;
        }
    }
}
