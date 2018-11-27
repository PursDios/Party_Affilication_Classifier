using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    public class Word
    {
        /// <summary>
        /// The word
        /// </summary>
        private string m_Word;
        /// <summary>
        /// get or set the word.
        /// </summary>
        public string getWord { get { return m_Word; } set { m_Word = value; } }

        /// <summary>
        /// The frequency of the word occuring
        /// </summary>
        private int m_Freq;
        /// <summary>
        /// get or set the word frequency
        /// </summary>
        public int getFreq { get { return m_Freq; } set { m_Freq = value; } }

        /// <summary>
        /// the probability of the word
        /// </summary>
        private double m_Probability;
        /// <summary>
        /// get or set the word probability
        /// </summary>
        public double getProbability { get { return m_Probability; } set { m_Probability = value; } }

        /// <summary>
        /// The TFIDF probability of the word
        /// </summary>
        private double m_TFIDF;
        /// <summary>
        /// get or set the TFIDF probability of the word.
        /// </summary>
        public double getTFIDF { get { return m_TFIDF; } set { m_TFIDF = value; } }

        /// <summary>
        /// The number of documents the word appears in
        /// </summary>
        private double m_NumberOfDocs;
        /// <summary>
        /// get or set the number of documents the word appears in
        /// </summary>
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
