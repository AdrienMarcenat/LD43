using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public interface ITextInterface
    {
        string GetName ();
        string GetText ();
    };

    public struct Sentence : ITextInterface
    {
        private string m_Name;
        public string m_Sentence;

        public Sentence (string name, string sentence)
        {
            m_Name = name;
            m_Sentence = sentence;
        }

        public string GetName ()
        {
            return m_Name;
        }

        public string GetText ()
        {
            return m_Sentence;
        }
    }

    public struct Choice : ITextInterface
    {
        private string m_Name;
        public List<KeyValuePair<string, string>> m_Choices;

        public Choice(string name, List<KeyValuePair<string, string>> choices)
        {
            m_Name = name;
            m_Choices = choices;
        }

        public string GetName ()
        {
            return "Choice";
        }

        public string GetText ()
        {
            return "Choice";
        }
    }

    public bool m_IsSubDialogues = false;
    public List<Dialogue> m_SubDialogues;
    public List<ITextInterface> m_Texts;
    public string m_Tag;

    public Dialogue (string tag)
    {
        m_Tag = tag;
        m_SubDialogues = new List<Dialogue> ();
        m_Texts = new List<ITextInterface> ();
    }
}

