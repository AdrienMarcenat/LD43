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

    private Queue<Dialogue.Sentence> m_Sentences;
    private bool m_IsInDialogue = false;
    private static string ms_DialogueFileName = "/Dialogues.txt";

    void Awake ()
    {
        m_Sentences = new Queue<Dialogue.Sentence> ();
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

        foreach (Dialogue.Sentence sentence in dialogue.m_Sentences)
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
        Dialogue.Sentence sentence = m_Sentences.Dequeue ();
        string speakerName = sentence.m_Name;
        if (m_NameText.text != speakerName)
        {
            m_NameText.text = speakerName;
            m_Thumbnail.sprite = RessourceManager.LoadSprite (speakerName, 0);
        }
        StartCoroutine (TypeSentence (sentence.m_Sentence));
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

        int dialogueBeginning = 0;
        int dialogueEnd = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] datas = lines[i].Split (separators);

            // If there is a single word it is a dialog tag
            if (datas.Length == 1 && datas[0] == tag)
            {
                dialogueBeginning = i + 2;
            }
            // We then seek for the a ] that signals the end of the dialogue
            if (dialogueBeginning > 0 && datas.Length == 1 && datas[0] == "]")
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

        for (int i = dialogueBeginning; i < dialogueEnd; i++)
        {
            string[] datas = lines[i].Split (separators);
            if (datas.Length != 2)
            {
                this.DebugLog ("Invalid number of data line " + i + " expecting 2, got " + datas.Length);
                return;
            }
            dialogue.m_Sentences.Add (new Dialogue.Sentence (datas[0].Trim(), datas[1]));
        }

        StartDialogue (dialogue);
    }
}

public class DialogueManagerProxy : UniqueProxy<DialogueManager>
{
}

