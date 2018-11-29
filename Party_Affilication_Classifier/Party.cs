/*
 * Project: Party Classifer
 * Filename: Party.cs
 * Created: 28/10/2018
 * Edited: 21/11/2018
 */
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
        /// The name of the party.
        /// </summary>
        private string m_Name = "";
        /// <summary>
        /// get or set the name of the party
        /// </summary>
        public string getName { get { return m_Name; } set { m_Name = value; } }

        /// <summary>
        /// The list of speeches associated with the party
        /// </summary>
        private List<Speech> m_SpeechList = new List<Speech>();
        /// <summary>
        /// get or set the speech list
        /// </summary>
        public List<Speech> getSpeechList { get { return m_SpeechList; } set { m_SpeechList = value; } }

        /// <summary>
        /// The likelyhood of a document being associated with this party.
        /// </summary>
        private double m_DocumentProbability = 0;
        /// <summary>
        /// get or set the document probability
        /// </summary>
        public double getDocumentProbability { get { return m_DocumentProbability; } set { m_DocumentProbability = value; } }

        /// <summary>
        /// The P(cata) of the party.
        /// </summary>
        private double m_Pcata = 0;
        /// <summary>
        /// get or set the P(cata) of the party
        /// </summary>
        public double getPcata { get { return m_Pcata; } set { m_Pcata = value; } }

        //The list of words associated with the party.
        private List<Word> m_WordList = new List<Word>();
        public List<Word> getWordList { get { return m_WordList; } set { m_WordList = value; } }

        //the TFIDF probability of a document being associated with this party.
        private double m_TFIDF;
        public double getTFIDF { get { return m_TFIDF; } set { m_TFIDF = value; } }

        private double m_Ngrams;
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
