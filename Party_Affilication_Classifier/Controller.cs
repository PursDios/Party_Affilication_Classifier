﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    public class Controller
    {
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
            //All the possible categories of government (E.G labour, conservative etc. This is to allow the program to easily be expanded upon).
            List<string> allCategories = new List<string> { "Labour", "Conservative", "Coalition" };
            List<Party> partyList = new List<Party>();

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
        }
        private void Consult()
        {

        }
    }
}
