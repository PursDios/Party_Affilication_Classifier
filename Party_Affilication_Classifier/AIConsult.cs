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
    }
}
