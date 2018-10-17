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
        public void MainMenu()
        {
            bool retry = true;
            int choice = 0;
            do
            {
                Console.WriteLine("1) Undertake Training");
                Console.WriteLine("2) Undertake a Classification");
                try
                {
                    choice = int.Parse(Console.ReadLine());
                    retry = false;
                }
                catch
                {
                    Console.WriteLine("Invalid Selection. Please try again");
                    Console.ReadLine();
                }
            } while (retry != false);
            if (choice == 1)
            {
                Training();
            }
            else if (choice == 2)
            {
                Consult();
            }
        }
        //Calls all of the nessecary methods required to train the program correctly.
        protected void Training()
        {
            AITraining AI = new AITraining();
            //All the possible categories of government (E.G labour, conservative etc. This is to allow the program to easily be expanded upon).
            List<string> allCategories = new List<string> { "Labour", "Conservative", "Coalition" };
            //All the categories selected for training
            List<string> selectedCats = new List<string>();

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

            AI.GetCategories(files, allCategories, selectedCats);
            AI.TrainingWords();
        }
        protected void Consult()
        {

        }
    }
}
