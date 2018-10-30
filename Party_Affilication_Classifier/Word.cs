using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    public class Word
    {
        private string m_Word;
        public string getWord { get { return m_Word; } set { m_Word = value; } }
        private int m_Freq;
        public int getFreq { get { return m_Freq; } set { m_Freq = value; } }
        private double m_Probability;
        public double getProbability { get { return m_Probability; } set { m_Probability = value; } }
        private double m_TFIDF;
        public double getTFIDF { get { return m_TFIDF; } set { m_TFIDF = value; } }

        public Word(string p_Word, int p_Freq, double p_Probability)
        {
            m_Word = p_Word.ToLower();
            m_Freq = p_Freq;
            m_Probability = p_Probability;
        }
        public Word()
        {
            //Needed for serialization.
        }

        public void CalculateTFIDF()
        {
            
        }
    }
}
