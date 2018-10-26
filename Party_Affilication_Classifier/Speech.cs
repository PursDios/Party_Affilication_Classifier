using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    public class Speech
    {
        private string m_Name;
        public string getName { get { return m_Name; } set { m_Name = value; } }
        private string m_Content;
        public string getContent { get { return m_Content; } set { m_Content = value; } }

        public Speech(string p_Name, string p_Content)
        {
            m_Name = p_Name;
            m_Content = p_Name;
        }
        public Speech()
        {
            //Required for serialization.
        }
    }
}
