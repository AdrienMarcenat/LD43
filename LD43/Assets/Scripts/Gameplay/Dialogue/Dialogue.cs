using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public struct Sentence
    {
        public string m_Name;
        public string m_Sentence;

        public Sentence (string name, string sentence)
        {
            m_Name = name;
            m_Sentence = sentence;
        }
    }

    public List<Sentence> m_Sentences;

    public Dialogue ()
    {
        m_Sentences = new List<Sentence> ();
    }
}

