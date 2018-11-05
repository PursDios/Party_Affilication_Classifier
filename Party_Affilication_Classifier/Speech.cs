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
        private string m_Name;
        public string getName { get { return m_Name; } set { m_Name = value; } }
        private string m_Content;
        public string getContent { get { return m_Content; } set { m_Content = value; } }

        public Speech(string p_Name, string p_Content)
        {
            m_Name = p_Name;
            m_Content = p_Content;
        }
        public Speech()
        {
            //Required for serialization.
        }
    }
}
