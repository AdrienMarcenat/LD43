using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DialogueEvent : GameEvent
{
    public DialogueEvent(string dialogueTag) : base("Dialogue")
    {
        m_DialogueTag = dialogueTag;
    }

    public string GetDialogueTag()
    {
        return m_DialogueTag;
    }

    private string m_DialogueTag;
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Text m_DialogeText;
    [SerializeField] private Text m_NameText;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Image m_Thumbnail;

    private Queue<Dialogue.ITextInterface> m_Sentences;
    private bool m_IsInDialogue = false;
    private static string ms_DialogueFileName = "/Dialogues.txt";

    void Awake ()
    {
        m_Sentences = new Queue<Dialogue.ITextInterface> ();
        this.RegisterAsListener ("Player", typeof (PlayerInputGameEvent));
        this.RegisterAsListener ("Dialogue", typeof (DialogueEvent));
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Dialogue");
        this.UnregisterAsListener ("Player");
    }

    public void OnGameEvent (DialogueEvent dialogueEvent)
    {
        TriggerDialogue (dialogueEvent.GetDialogueTag ());
    }

    public void OnGameEvent (PlayerInputGameEvent inputEvent)
    {
        if (inputEvent.GetInput () == "Submit" && inputEvent.GetInputState () == EInputState.Down && m_IsInDialogue)
        {
            DisplayNextSentence ();
        }
    }

    public void StartDialogue (Dialogue dialogue)
    {
        m_IsInDialogue = true;
        m_Animator.SetBool ("IsOpen", true);
        m_Sentences.Clear ();

        foreach (Dialogue.ITextInterface sentence in dialogue.m_Texts)
        {
            m_Sentences.Enqueue (sentence);
        }

        DisplayNextSentence ();
    }

    public void DisplayNextSentence ()
    {
        if (m_Sentences.Count == 0)
        {
            EndDialogue ();
            return;
        }
        StopAllCoroutines ();
        Dialogue.ITextInterface sentence = m_Sentences.Dequeue ();
        string speakerName = sentence.GetName ();
        if (m_NameText.text != speakerName)
        {
            m_NameText.text = speakerName;
            m_Thumbnail.sprite = RessourceManager.LoadSprite (speakerName, 0);
        }
        StartCoroutine (TypeSentence (sentence.GetName ()));
    }

    IEnumerator TypeSentence (string sentence)
    {
        m_DialogeText.text = "";
        foreach (char letter in sentence.ToCharArray ())
        {
            m_DialogeText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue ()
    {
        m_IsInDialogue = false;
        m_Animator.SetBool ("IsOpen", false);
        new GameFlowEvent (EGameFlowAction.EndDialogue).Push ();
    }

    public void TriggerDialogue (string tag)
    {
        new GameFlowEvent (EGameFlowAction.StartDialogue).Push ();
        Dialogue dialogue = new Dialogue ();

        char[] separators = { ':' };
        string filename = ms_DialogueFileName;
        filename = Application.streamingAssetsPath + filename;

        string[] lines = File.ReadAllLines (filename);

        int countBrackets = 0;

        int dialogueBeginning = 0;
        int dialogueEnd = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] datas = lines[i].Split (separators);

            if (datas[0] == "[")
            {
                countBrackets++;
            }

            if (datas[0] == "]")
            {
                if (countBrackets <= 0)
                {
                    this.DebugLog ("Parsing error, ']' unexpected at line " + i);
                    new GameFlowEvent (EGameFlowAction.EndDialogue).Push ();
                }

                countBrackets--;
            }

            // If there is a single word it is a dialog tag
            if (datas.Length == 1 && datas[0] == tag && countBrackets == 0)
            {
                dialogueBeginning = i + 2;
            }
            // We then seek for the a ] that signals the end of the dialogue
            if (dialogueBeginning > 0 && datas.Length == 1 && datas[0] == "]" && countBrackets == 0)
            {
                dialogueEnd = i;
                break;
            }
        }
        if (dialogueBeginning == 0)
        {
            this.DebugLog ("Could not find dialogue with tag " + tag);
            new GameFlowEvent (EGameFlowAction.EndDialogue).Push ();
            return;
        }

        // Check if dialogue is composed of subdialogs or not by checking if there's a subdialog name
        string[] temp = lines[dialogueBeginning].Split (separators);
        if (temp.Length == 1)
        {
            dialogue.m_IsSubDialogues = true;
        }

        if (!dialogue.m_IsSubDialogues)
        {
            // If not composed of subdialogs, parse only sentences
            for (int i = dialogueBeginning; i < dialogueEnd; i++)
            {
                string[] datas = lines[i].Split (separators);
                if (datas.Length != 2)
                {
                    this.DebugLog ("Invalid number of data line " + i + " expecting 2, got " + datas.Length);
                    return;
                }
                dialogue.m_Texts.Add (new Dialogue.Sentence (datas[0].Trim (), datas[1]));
            }
        }
        else
        {
            // If composed of subdialogs, parse subdialogs
            char[] newSeparators = { ':', '[', ']' };
            bool subDialogueStart = false;
            string subDialogueName = null;
            Dialogue currSubDialogue = null;

            for (int i = dialogueBeginning; i < dialogueEnd; i++)
            {
                string[] datas = lines[i].Split (newSeparators);

                // Find beginning of subdialog
                if (!subDialogueStart && datas.Length == 1)
                {
                    subDialogueStart = true;
                    subDialogueName = datas[0].Trim ();
                    currSubDialogue = new Dialogue ();
                    i++;
                    continue;
                }

                // Find ending of subdialog and add it to dialog
                if (subDialogueStart && datas.Length == 1 && string.IsNullOrEmpty (datas[0].Trim ()))
                {
                    subDialogueStart = false;
                    dialogue.m_SubDialogues.Add (new Dialogue.SubDialogue (subDialogueName, currSubDialogue));
                    continue;
                }

                // Find Sentence and Choice of subdialog and add it to subdialog
                if (subDialogueStart && datas.Length == 3)
                {
                    if (string.Equals(datas[1], "Sentence"))
                    {
                        currSubDialogue.m_Texts.Add (new Dialogue.Sentence (datas[2].Trim (), datas[3]));
                    }
                    if (string.Equals(datas[1], "Choice"))
                    {
                        currSubDialogue.m_Texts.Add (new Dialogue.Choice ("You", ParseChoice (datas[3], i)));
                    }
                }
            }
        }

        StartDialogue (dialogue);
    }

    public List<KeyValuePair<string, string>> ParseChoice (string choice, int line)
    {
        List<KeyValuePair<string, string>> choices = new List<KeyValuePair<string, string>> ();

        char[] separators = { '/' };

        string[] data = choice.Split (separators);
        if (data.Length % 2 != 0)
        {
            this.DebugLog ("Invalid count of data for choice at line " + line);
            return null;
        }

        for (int i = 0; i < (data.Length / 2); i++)
        {
            choices.Add (new KeyValuePair<string, string> (data[2 * i], data[2 * i + 1]));
        }

        return choices;
    }
}

public class DialogueManagerProxy : UniqueProxy<DialogueManager>
{
}

