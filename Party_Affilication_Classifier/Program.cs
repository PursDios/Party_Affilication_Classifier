/*
 * Project: Party Classifer
 * Filename: Program.cs
 * Created: 28/10/2018
 * Edited: 21/11/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Party_Affilication_Classifier
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller c = new Controller();
            c.MainMenu();
        }
    }
}
