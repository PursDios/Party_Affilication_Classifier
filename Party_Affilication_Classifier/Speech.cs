/*
 * Project: Party Classifer
 * Filename: Speech.cs
 * Created: 29/10/2018
 * Edited: 21/11/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    /// <summary>
    /// Contains the name of the speech and the content in the speech.
    /// </summary>
    public class Speech
    {
        /// <summary>
        /// The Name of the speech
        /// </summary>
        private string m_Name;
        /// <summary>
        /// get or set the name of the speech
        /// </summary>
        public string getName { get { return m_Name; } set { m_Name = value; } }
        /// <summary>
        /// The content of the document
        /// </summary>
        private string m_Content;
        /// <summary>
        /// get or set the content of the document
        /// </summary>
        public string getContent { get { return m_Content; } set { m_Content = value; } }

        /// <summary>
        /// Constructor for Speech
        /// </summary>
        /// <param name="p_Name">The filename of the speech file</param>
        /// <param name="p_Content">the string content of the speech file</param>
        public Speech(string p_Name, string p_Content)
        {
            m_Name = p_Name;
            m_Content = p_Content;
        }
        /// <summary>
        /// Required for serialization.
        /// </summary>
        public Speech()
        {
        }
        /// <summary>
        /// Creates a List of Ngrams from the words in the Speech
        /// </summary>
        /// <returns>a list of strings which contain the Ngrams of the speeches words</returns>
        public List<string> NgramFromSpeech()
        {
            List<string> SpeechWords = new List<string>();
            List<string> SpeechWordsNew = new List<string>();
            SpeechWords = m_Content.ToLower().Split(' ', '\n').ToArray().ToList();
            string temp;
            //add two words together.
            for (int i = 0; i < SpeechWords.Count(); i++)
            {
                temp = SpeechWords[i];
                i++;
                if (!(SpeechWords.Count() - 1 < i))
                {
                    temp += " " + SpeechWords[i];

                }
                SpeechWordsNew.Add(temp);
            }
            return SpeechWordsNew;
        }
    }
}
