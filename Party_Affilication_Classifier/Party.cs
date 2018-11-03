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
        private string m_Name="";
        public string getName { get { return m_Name; } set { m_Name = value; } }

        private List<Speech> m_SpeechList = new List<Speech>();
        public List<Speech> getSpeechList { get { return m_SpeechList; } set { m_SpeechList = value; } }

        private double m_DocumentProbability = 0;
        public double getDocumentProbability { get { return m_DocumentProbability; } set { m_DocumentProbability = value; } }

        private double m_Proability=0;
        public double getProbability { get { return m_Proability; } set { m_Proability = value; } }

        private List<Word> m_WordList = new List<Word>();
        public List<Word> getWordList { get { return m_WordList; } set { m_WordList = value; } }

        private double m_TFIDF;
        public double getTFIDF { get { return m_TFIDF; } set { m_TFIDF = value; } }

        public Party(string partyName)
        {
            m_Name = partyName;
        }
        public Party()
        {
            //needed for serializer.
        }
    }
}
