using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    public class Party
    {
        private string m_Name;
        public string getName { get { return m_Name; } }
        private Dictionary<string, int> m_Words;
        public Dictionary<string, int> getWordFreq { get { return m_Words; } set { m_Words = value; } }
        private List<FileInfo> m_speechList;
        public List<FileInfo> getSpeechList { get { return m_speechList; } set { m_speechList = value; } }
        private Dictionary<string,double> m_WordProbabilities;
        public Dictionary<string,double> getWordProbabilities { get { return m_WordProbabilities; } set { m_WordProbabilities = value; } }
        private double m_Proability;
        public double getProbability { get { return m_Proability; } set { m_Proability = value; } }

        public Party(string partyName)
        {
            m_speechList = new List<FileInfo>();
            m_Words = new Dictionary<string, int>();
            m_WordProbabilities = new Dictionary<string,double>();
            m_Name = partyName;
        }
        public Party()
        {
            //needed for serializer.
        }
        private void removeSpeechGrammar()
        {
            //remove all special characters from speeches { '"', ':', ';', '\n', '\t', '.', ',', '\r' }
        }
    }
}
