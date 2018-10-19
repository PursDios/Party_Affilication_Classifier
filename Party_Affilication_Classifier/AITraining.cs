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
        private Dictionary<string, int> m_Labour = new Dictionary<string, int>();
        public Dictionary<string,int> get_Labour { get { return m_Labour; } set { m_Labour = value; } }

        private Dictionary<string, int> m_Conservative = new Dictionary<string, int>();
        public Dictionary<string,int> get_Conservative { get { return m_Conservative; } set { m_Conservative = value; } }

        private Dictionary<string, int> m_Coalition = new Dictionary<string, int>();
        public Dictionary<string,int> Coalition { get { return m_Coalition; } set { m_Coalition = value; } }

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
                    sr = new StreamReader (@"TrainingFiles\" +  f.Key.ToString());
                    string s = sr.ReadToEnd();
                    string[] splitWords = s.Split(' ');

                    foreach(string str in splitWords)
                    {
                        if(!get_Labour.ContainsKey(str))
                        {
                            get_Labour.Add(str, 1);
                        }
                        else
                        {
                            get_Labour[str]++;
                        }
                    }
                }
                if (f.Value == "Conservative")
                {
                    sr = new StreamReader(@"TrainingFiles\" + f.Key.ToString());
                    string s = sr.ReadToEnd();
                    string[] splitWords = s.Split(' ');

                    foreach (string str in splitWords)
                    {
                        if (!get_Labour.ContainsKey(str))
                        {
                            get_Labour.Add(str, 1);
                        }
                        else
                        {
                            get_Labour[str]++;
                        }
                    }
                }
                if (f.Value == "Coalition")
                {
                    sr = new StreamReader(@"TrainingFiles\" + f.Key.ToString());
                    string s = sr.ReadToEnd();
                    string[] splitWords = s.Split(' ');

                    foreach (string str in splitWords)
                    {
                        if (!get_Labour.ContainsKey(str))
                        {
                            get_Labour.Add(str, 1);
                        }
                        else
                        {
                            get_Labour[str]++;
                        }
                    }
                }
            }
        }
    }
}
