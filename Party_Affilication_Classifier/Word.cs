using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    public class Word
    {
        //The word
        private string m_Word;
        public string getWord { get { return m_Word; } set { m_Word = value; } }

        //The frequency of the word occuring
        private int m_Freq;
        public int getFreq { get { return m_Freq; } set { m_Freq = value; } }

        //the probability of the word
        private double m_Probability;
        public double getProbability { get { return m_Probability; } set { m_Probability = value; } }

        //the TFIDF of the word
        private double m_TFIDF;
        public double getTFIDF { get { return m_TFIDF; } set { m_TFIDF = value; } }

        //The number of documents the word appears in
        private double m_NumberOfDocs;
        public double getNumberOfDocs { get { return m_NumberOfDocs; } set { m_NumberOfDocs = value; } }

        /// <summary>
        /// Creates a new word
        /// </summary>
        /// <param name="p_Word">The word</param>
        /// <param name="p_Freq">The frequency of the word</param>
        /// <param name="p_Probability">The probability of the word</param>
        public Word(string p_Word, int p_Freq, double p_Probability)
        {
            m_Word = p_Word.ToLower();
            m_Freq = p_Freq;
            m_Probability = p_Probability;
        }
        //Needed for serialization.
        public Word()
        {
        }
        /// <summary>
        /// Calculates the TFIDF of a word.
        /// </summary>
        /// <param name="DocWords">The number of words in a document</param>
        /// <param name="TotalDocs">The total number of Documents</param>
        /// <param name="DocumentWordCount">The number of times a word appears in all of the documents</param>
        public void CalculateTFIDF(int DocWords, int TotalDocs, int DocumentWordCount)
        {
            double TF = (double)getFreq / DocWords;
            double IDF = (double)TotalDocs / DocumentWordCount;
            m_TFIDF = Math.Log(TF * IDF);
        }
    }
}
