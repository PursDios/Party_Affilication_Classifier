using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    /// <summary>
    /// Contains all of the information about a party.
    /// </summary>
    public class Party
    {
        /// <summary>
        /// the name of the party
        /// </summary>
        private string m_Name = "";
        /// <summary>
        /// get or set the name of the party
        /// </summary>
        public string getName { get { return m_Name; } set { m_Name = value; } }

        /// <summary>
        /// The list of speeches associated with that party.
        /// </summary>
        private List<Speech> m_SpeechList = new List<Speech>();
        /// <summary>
        /// Get or set The list of speeches associated with that party.
        /// </summary>
        public List<Speech> getSpeechList { get { return m_SpeechList; } set { m_SpeechList = value; } }

        /// <summary>
        /// The likelyhood of a document being associated with this party.
        /// </summary>
        private double m_DocumentProbability = 0;
        /// <summary>
        /// get or set the documentProbability.
        /// </summary>
        public double getDocumentProbability { get { return m_DocumentProbability; } set { m_DocumentProbability = value; } }

        /// <summary>
        /// The P(cata) of the party.
        /// </summary>
        private double m_Pcata = 0;
        /// <summary>
        /// get or set the P(cata) of the party.
        /// </summary>
        public double getPcata { get { return m_Pcata; } set { m_Pcata = value; } }

        /// <summary>
        /// the list of words associated with this party.
        /// </summary>
        private List<Word> m_WordList = new List<Word>();
        /// <summary>
        /// get or set the list of words associated with this party.
        /// </summary>
        public List<Word> getWordList { get { return m_WordList; } set { m_WordList = value; } }

        /// <summary>
        /// the TFIDF probability of a document being associated with this party.
        /// </summary>
        private double m_TFIDF;
        /// <summary>
        /// get or set the TFIDF probability of a document being associated with this party.
        /// </summary>
        public double getTFIDF { get { return m_TFIDF; } set { m_TFIDF = value; } }

        /// <summary>
        /// The Ngrams probability of the document being associated with this party.
        /// </summary>
        private double m_Ngrams;
        /// <summary>
        /// get or set the Ngram probability.
        /// </summary>
        public double getNgrams { get { return m_Ngrams; } set { m_Ngrams = value; } }

        /// <summary>
        /// Creates a new party.
        /// </summary>
        /// <param name="partyName">Name of the party</param>
        public Party(string partyName)
        {
            m_Name = partyName;
        }
        //needed for serializer.
        public Party()
        {
        }
    }
}
