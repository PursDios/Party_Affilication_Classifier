using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    class Controller
    {
        public Controller()
        {
            MainMenu();
        }
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
        protected void Training()
        {
            int i = 0;
            DirectoryInfo d = new DirectoryInfo("TrainingFiles");
            FileInfo[] files = d.GetFiles("*.txt");

            Console.WriteLine("Please select which files you'd like to use split up by a comma");
            foreach(FileInfo f in files)
            {
                i++;
                Console.WriteLine(i + " " + f.Name);
            }
            string selection = Console.ReadLine();
            string[] splitSelection = selection.Split(',');

            Console.WriteLine("You have selected: ");
            foreach (string s in splitSelection)
            {
                Console.WriteLine(files[int.Parse(s) - 1].Name);
            }
            Console.ReadLine();
        }
        protected void Consult()
        {

        }
    }
}
