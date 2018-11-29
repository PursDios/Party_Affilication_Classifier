/*
 * Project: Party Classifer
 * Filename: Lementize.cs
 * Created: 15/11/2018
 * Edited: 21/11/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Affilication_Classifier
{
    //source:
    //http://snowballstem.org/algorithms/english/stemmer.html 
    /// <summary>
    /// Shortens words (Stems/Lementizes words)
    /// </summary>
    class Lementize
    {
        /// <summary>
        /// Gets the alphabet
        /// </summary>
        private char[] m_Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();
        /// <summary>
        /// array of vowels
        /// </summary>
        private char[] m_Vowels = "aeiou".ToCharArray();
        /// <summary>
        /// array of endings that need to be replaced with Li
        /// </summary>
        private char[] m_LiEnds = "cdeghkmnrt".ToCharArray();
        /// <summary>
        /// Contains all of the occurances of double characters.
        /// </summary>
        private  string[] m_DoubleChars = { "bb", "dd", "ff", "gg", "mm", "nn", "pp", "rr", "tt" };

        /// <summary>
        /// Calls all of the methods to remove or shorten words.
        /// </summary>
        /// <param name="str">The word you want to Lementize/Stem</param>
        /// <returns>Returns the word after Lementization/Stemming</returns>
        public string LementizeWord(string str)
        {
            if (str.Length <= 2)
            {
                return str;
            }

            str = str.ToLower();

            int r1 = FindVowel(str, 0);
            int r2 = FindVowel(str, r1);

            str = RemoveSSuffix(str);
            str = RemoveMoreSSuffix(str);
            str = RemoveLySuffix(str, r1);
            str = YISuffix(str);
            str = ReplaceSuffixes(str, r1);
            str = ReplaceMoreSuffixes(str, r1, r2);
            str = RemoveR2Suffix(str, r2);
            str = RemoveEorLSuffixes(str, r1, r2);
            return str;
        }

        /// <summary>
        /// Checks if the character is a vowel
        /// </summary>
        /// <param name="c">The character being checked</param>
        /// <returns>True or False</returns>
        private bool CheckVowel(char c)
        {
            return m_Vowels.Contains(c);
        }

        /// <summary>
        /// Checks if the character is not a vowel
        /// </summary>
        /// <param name="c">The character being checked</param>
        /// <returns>True or False</returns>
        private bool CheckConsonant(char c)
        {
            return !m_Vowels.Contains(c);
        }

        /// <summary>
        /// Checks if r1 is equal to or less than the length of the word minus the suffix
        /// </summary>
        /// <param name="word">The word being checked</param>
        /// <param name="r1">The first occurance of a vowel in the word</param>
        /// <param name="suffix">The suffix being minused.</param>
        /// <returns>True or False</returns>
        private bool R1Suffix(string word, int r1, string suffix)
        {
            return r1 <= word.Length - suffix.Length;
        }

        /// <summary>
        /// Checks if r2 is equal to or less than the length of the word
        /// </summary>
        /// <param name="word">The word being checked</param>
        /// <param name="r2"></param>
        /// <param name="suffix">The suffix being minused</param>
        /// <returns>True or False</returns>
        private bool R2Suffix(string word, int r2, string suffix)
        {
            return r2 <= word.Length - suffix.Length;
        }

        /// <summary>
        /// Finds the first vowel after the begin variable (That is also followed by a consonant)
        /// </summary>
        /// <param name="word"></param>
        /// <param name="begin"></param>
        /// <returns></returns>
        private int FindVowel(string word, int begin)
        {
            bool vowel = false;

            for (int i = begin; i < word.Length; i++)
            {
                if (CheckVowel(word[i]))
                {
                    vowel = true;
                    continue;
                }

                if (vowel && CheckConsonant(word[i]))
                {
                    return i + 1;
                }
            }

            return word.Length;
        }

        /// <summary>
        /// Checks if the word is a short word.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool CheckShortWord(string word)
        {
            return IsShort(word) && FindVowel(word, 0) == word.Length;
        }

        private bool IsShort(string word)
        {
            if (word.Length < 2)
            {
                return false;
            }

            
            if (word.Length == 2)
            {
                return CheckVowel(word[0]) && CheckConsonant(word[1]);
            }

            //Consonant followed by Vowel followed by Consonant
            return CheckVowel(word[word.Length - 2]) && CheckConsonant(word[word.Length - 1])
                                                       && CheckConsonant(word[word.Length - 3]);
        }

        /// <summary>
        /// Attempts to call the method to replace the words old suffix with a new suffix.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="oldSuffix"></param>
        /// <param name="newSuffix"></param>
        /// <param name="final"></param>
        /// <returns></returns>
        private bool TryReplace(string word, string oldSuffix, string newSuffix, out string final)
        {
            if (word.Contains(oldSuffix))
            {
                final = ReplaceSuffix(word, oldSuffix, newSuffix);
                return true;
            }

            final = word;
            return false;
        }

        /// <summary>
        /// Actually replaced the words old suffix with the new suffix.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="oldSuffix"></param>
        /// <param name="newSuffix"></param>
        /// <returns></returns>
        private string ReplaceSuffix(string word, string oldSuffix, string newSuffix = null)
        {
            if (oldSuffix != null)
            {
                word = word.Substring(0, word.Length - oldSuffix.Length);
            }

            if (newSuffix != null)
            {
                word += newSuffix;
            }

            return word;
        }

        /// <summary>
        /// Removes the S suffix.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private string RemoveSSuffix(string word)
        {
            string[] plurals = { "'s", "s", "s'" };
            foreach (string suffix in plurals)
            {
                if (word.EndsWith(suffix))
                {
                    return ReplaceSuffix(word, suffix);
                }
            }

            return word;
        }

        /// <summary>
        /// Removes other S suffixes.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private string RemoveMoreSSuffix(string word)
        {
            if (word.EndsWith("sses"))
            {
                return ReplaceSuffix(word, "sses", "ss");
            }

            if (word.EndsWith("ied") || word.EndsWith("ies"))
            {
                string wordWithoutEnd = word.Substring(0, word.Length - 3);
                if (word.Length > 4)
                {
                    return wordWithoutEnd + "i";
                }

                return wordWithoutEnd + "ie";
            }

            if (word.EndsWith("us") || word.EndsWith("ss"))
            {
                return word;
            }

            if (word.EndsWith("s"))
            {
                if (word.Length < 3)
                {
                    return word;
                }

                for (int i = 0; i < word.Length - 2; i++)
                {
                    if (this.CheckVowel(word[i]))
                    {
                        return word.Substring(0, word.Length - 1);
                    }
                }
            }

            return word;
        }

        /// <summary>
        /// Removes suffixes that contain e and ly.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="r1"></param>
        /// <returns></returns>
        private string RemoveLySuffix(string word, int r1)
        {
            string[] endingString1 = { "eedly", "eed" };
            foreach (string suffix in endingString1.Where(word.EndsWith))
            {
                if (R1Suffix(word, r1, suffix))
                {
                    return ReplaceSuffix(word, suffix, "ee");
                }

                return word;
            }

            string[] endingString2 = { "ed", "edly", "ing", "ingly" };

            foreach (string suffix in endingString2.Where(word.EndsWith))
            {
                string trunc = this.ReplaceSuffix(word, suffix);
                if (trunc.Any(this.CheckVowel))
                {
                    if (new string[] { "at", "bl", "iz" }.Any(trunc.EndsWith))
                    {
                        return trunc + "e";
                    }

                    if (this.m_DoubleChars.Any(trunc.EndsWith))
                    {
                        return trunc.Substring(0, trunc.Length - 1);
                    }

                    if (this.CheckShortWord(trunc))
                    {
                        return trunc + "e";
                    }

                    return trunc;
                }
                return word;
            }
            return word;
        }

        /// <summary>
        /// Replace Y's with an I.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private string YISuffix(string word)
        {
            if (word.EndsWith("y") && word.Length > 2 && this.CheckConsonant(word[word.Length - 2]))
            {
                return word.Substring(0, word.Length - 1) + "i";
            }

            return word;
        }

        /// <summary>
        /// Removes any of the suffixes that were found.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="r1"></param>
        /// <returns></returns>
        private string ReplaceSuffixes(string word, int r1)
        {
            Dictionary<string, string> suffixes = new Dictionary<string, string>(){{ "ization", "ize" },{ "ational", "ate" },{ "ousness", "ous" },{ "iveness", "ive" },{ "fulness", "ful" },{ "tional", "tion" },{ "lessli", "less" },{ "biliti", "ble" },{ "entli", "ent" },{ "ation", "ate" },{ "alism", "al" },{ "aliti", "al" },{ "fulli", "ful" },{ "ousli", "ous" },{ "iviti", "ive" },{ "enci", "ence" },{ "anci", "ance" },{ "abli", "able" },{ "izer", "ize" },{ "ator", "ate" },{ "alli", "al" },{ "bli", "ble" }};

            foreach (KeyValuePair<string, string> suffix in suffixes)
            {
                if (word.EndsWith(suffix.Key))
                {
                    if (R1Suffix(word, r1, suffix.Key) && TryReplace(word,suffix.Key,suffix.Value,out string final))
                    {
                        return final;
                    }
                    return word;
                }
            }

            if (word.EndsWith("ogi") && R1Suffix(word, r1, "ogi") && word[word.Length - 4] == 'l')
            {
                return ReplaceSuffix(word, "ogi", "og");
            }

            if (word.EndsWith("li") & R1Suffix(word, r1, "li"))
            {
                if (m_LiEnds.Contains(word[word.Length - 3]))
                {
                    return ReplaceSuffix(word, "li");
                }
            }

            return word;
        }

        /// <summary>
        /// Removes any of the suffixes that were found (part 2)
        /// </summary>
        /// <param name="word"></param>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        private string ReplaceMoreSuffixes(string word, int r1, int r2)
        {
            Dictionary<string, string> suffixes = new Dictionary<string, string>{{ "ational", "ate" },{ "tional", "tion" },{ "alize", "al" },{ "icate", "ic" },{ "iciti", "ic" },{ "ical", "ic" },{ "ful", null },{ "ness", null }};

            foreach (KeyValuePair<string, string> suffix in suffixes.Where(s => word.EndsWith(s.Key)))
            {
                if (R1Suffix(word, r1, suffix.Key) && TryReplace(word,suffix.Key,suffix.Value,out string final))
                {
                    return final;
                }
            }

            if (word.EndsWith("ative"))
            {
                if (this.R1Suffix(word, r1, "ative") && R2Suffix(word, r2, "ative"))
                {
                    return ReplaceSuffix(word, "ative");
                }
            }

            return word;
        }

        /// <summary>
        /// Removes any of the suffixes that were found (part 3)
        /// </summary>
        /// <param name="word"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        private string RemoveR2Suffix(string word, int r2)
        {
            string[] suffixes ={"al", "ance", "ence", "er", "ic", "able", "ible", "ant", "ement", "ment", "ent", "ism", "ate","iti", "ous", "ive", "ize"};
            foreach (string s in suffixes)
            {
                if (word.EndsWith(s))
                {
                    if (R2Suffix(word, r2, s))
                    {
                        return ReplaceSuffix(word, s);
                    }

                    return word;
                }
            }

            if (word.EndsWith("ion") && this.R2Suffix(word, r2, "ion") && new[] { 's', 't' }.Contains(word[word.Length - 4]))
            {
                return ReplaceSuffix(word, "ion");
            }

            return word;
        }

        /// <summary>
        /// Removes any of the suffixes that were found (part 4)
        /// </summary>
        /// <param name="word"></param>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        private string RemoveEorLSuffixes(string word, int r1, int r2)
        {
            if (word.EndsWith("e") && (R2Suffix(word, r2, "e") || (R1Suffix(word, r1, "e") && !IsShort(ReplaceSuffix(word, "e")))))
            {
                return ReplaceSuffix(word, "e");
            }

            if (word.EndsWith("l") && R2Suffix(word, r2, "l") && word.Length > 1 && word[word.Length - 2] == 'l')
            {
                return ReplaceSuffix(word, "l");
            }
            return word;
        }
    }
}
