using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public interface ITextInterface
    {
        string GetName ();
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
            return m_Name;
        }
    }

    public struct SubDialogue
    {
        public string m_Name;
        public Dialogue m_Text;

        public SubDialogue(string name, Dialogue text)
        {
            m_Name = name;
            m_Text = text;
        }
    }

    public bool m_IsSubDialogues = false;
    public List<SubDialogue> m_SubDialogues;
    public List<ITextInterface> m_Texts;

    public Dialogue ()
    {
        m_SubDialogues = new List<SubDialogue> ();
        m_Texts = new List<ITextInterface> ();
    }
}

