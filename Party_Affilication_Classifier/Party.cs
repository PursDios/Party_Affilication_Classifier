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
        //The name of the party.
        private string m_Name="";
        public string getName { get { return m_Name; } set { m_Name = value; } }

        //The list of speeches associated with the party
        private List<Speech> m_SpeechList = new List<Speech>();
        public List<Speech> getSpeechList { get { return m_SpeechList; } set { m_SpeechList = value; } }

        //The likelyhood of a document being associated with this party.
        private double m_DocumentProbability = 0;
        public double getDocumentProbability { get { return m_DocumentProbability; } set { m_DocumentProbability = value; } }

        //The P(cata) of the party.
        private double m_Proability=0;
        public double getProbability { get { return m_Proability; } set { m_Proability = value; } }

        //The list of words associated with the party.
        private List<Word> m_WordList = new List<Word>();
        public List<Word> getWordList { get { return m_WordList; } set { m_WordList = value; } }

        //the TFIDF probability of a document being associated with this party.
        private double m_TFIDF;
        public double getTFIDF { get { return m_TFIDF; } set { m_TFIDF = value; } }

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
