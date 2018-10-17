using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    public class AITraining
    {
        public Dictionary<string,int> Labour { get; set; }
        public Dictionary<string,int> Conservative { get; set; }
        public Dictionary<string,int> Coalition { get; set; }

        //The files selected for training being ordered in order of category.
        Dictionary<FileInfo, string> fileCats = new Dictionary<FileInfo, string>();

        //Gets the party affilication of all of the selected files
        public void GetCategories(FileInfo[] files, List<string> allCategories, List<string> selectedCats)
        {
            foreach (FileInfo f in files)
            {
                foreach (string c in allCategories)
                {
                    if (f.Name.Contains(c))
                    {
                        if (!selectedCats.Any(x => x.ToString() == c))
                        {
                            selectedCats.Add(c);
                            fileCats.Add(f, c);
                        }
                    }
                }
            }

        }
        //Reads all of he words in each file and assigns them as words used by a particular party.
        public void TrainingWords()
        {
            StreamReader sr;
            foreach (KeyValuePair<FileInfo, string> f in fileCats)
            {
                if (f.Value == "Labour")
                {
                    sr = new StreamReader (f.Key.ToString());
                }
                if (f.Value == "Conservative")
                {

                }
                if (f.Value == "Coalition")
                {

                }
            }
        }
    }
}
