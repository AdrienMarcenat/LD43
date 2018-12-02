﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Assertions;

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
    [SerializeField] private List<Button> m_ChoicesButton;

    private Queue<Dialogue.ITextInterface> m_Sentences;
    private Dialogue m_CurrentDialogue;
    private bool m_IsInDialogue = false;
    private static string ms_DialogueFileName = "/Dialogues.txt";

    void Awake ()
    {
        DialogueManagerProxy.Open (this);
        m_Sentences = new Queue<Dialogue.ITextInterface> ();
        this.RegisterAsListener ("Player", typeof (PlayerInputGameEvent));
        this.RegisterAsListener ("Dialogue", typeof (DialogueEvent));
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Dialogue");
        this.UnregisterAsListener ("Player");
        DialogueManagerProxy.Close ();
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
        Reset ();
        m_IsInDialogue = true;
        m_Animator.SetBool ("IsOpen", true);
        m_Sentences.Clear ();

        if (dialogue.m_IsSubDialogues)
        {
            m_CurrentDialogue = dialogue;
            StartDialogue (m_CurrentDialogue.m_SubDialogues[0]);
        }
        else
        {
            foreach (Dialogue.ITextInterface sentence in dialogue.m_Texts)
            {
                m_Sentences.Enqueue (sentence);
            }

            DisplayNextSentence ();
        }
    }

    private void Reset()
    {
        m_Sentences.Clear ();
        foreach (Button b in m_ChoicesButton)
        {
            b.onClick.RemoveAllListeners ();
            b.gameObject.SetActive (false);
        }

    }

    private Dialogue FindSubdialogue (string tag)
    {
        foreach(Dialogue d in m_CurrentDialogue.m_SubDialogues)
        {
            if(d.m_Tag == tag)
            {
                return d;
            }
        }
        return null;
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
        if (speakerName == "Choice")
        {
            Dialogue.Choice choice = (Dialogue.Choice)sentence;
            Assert.IsTrue (choice.m_Choices.Count <= m_ChoicesButton.Count);
            
            for (int i = 0; i < choice.m_Choices.Count; ++i)
            {
                KeyValuePair<string, string> pair = choice.m_Choices[i];
                Button button = m_ChoicesButton[i];
                button.gameObject.SetActive(true);
                button.GetComponentInChildren<Text> ().text = pair.Key;
                button.onClick.AddListener (delegate 
                {
                    Dialogue d = FindSubdialogue (pair.Value);
                    if(d != null)
                    {
                        StartDialogue (d);
                    }
                });
            }
        }
        else
        {
            if (m_NameText.text != speakerName)
            {
                m_NameText.text = speakerName;
                //m_Thumbnail.sprite = RessourceManager.LoadSprite (speakerName, 0);
            }
            StartCoroutine (TypeSentence (sentence.GetText ()));
        }
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
        m_CurrentDialogue = null;
        new GameFlowEvent (EGameFlowAction.EndDialogue).Push ();
    }

    public void TriggerDialogue (string tag)
    {
        new GameFlowEvent (EGameFlowAction.StartDialogue).Push ();
        Dialogue dialogue = new Dialogue (tag);

        char[] separators = { ':' };
        string filename = ms_DialogueFileName;
        filename = Application.streamingAssetsPath + filename;

        string[] lines = File.ReadAllLines (filename);

        int countBrackets = 0;

        int dialogueBeginning = 0;
        int dialogueEnd = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] datas = lines[i].Split (separators, System.StringSplitOptions.RemoveEmptyEntries);

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
        string[] temp = lines[dialogueBeginning].Split (separators, System.StringSplitOptions.RemoveEmptyEntries);
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
            char[] trimChar = { ' ', '\t' };
            bool subDialogueStart = false;
            string subDialogueName = null;
            Dialogue currSubDialogue = null;

            for (int i = dialogueBeginning; i < dialogueEnd; i++)
            {
                string line = lines[i].Trim (trimChar);
                string[] datas = line.Split (newSeparators, System.StringSplitOptions.RemoveEmptyEntries);

                // Find beginning of subdialog
                if (!subDialogueStart && datas.Length == 1)
                {
                    subDialogueStart = true;
                    subDialogueName = datas[0].Trim ();
                    currSubDialogue = new Dialogue (subDialogueName);
                    i++;
                    continue;
                }

                // Find ending of subdialog and add it to dialog
                if (subDialogueStart && line == "]")
                {
                    subDialogueStart = false;
                    dialogue.m_SubDialogues.Add (currSubDialogue);
                    currSubDialogue = null;
                    continue;
                }

                // Find Sentence and Choice of subdialog and add it to subdialog
                if (subDialogueStart && datas.Length > 0)
                {
                    if (string.Equals(datas[0], "Sentence"))
                    {
                        Assert.IsTrue (datas.Length == 3);
                        currSubDialogue.m_Texts.Add (new Dialogue.Sentence (datas[1].Trim (), datas[2]));
                    }
                    if (string.Equals(datas[0], "Choice"))
                    {
                        Assert.IsTrue (datas.Length == 2);
                        currSubDialogue.m_Texts.Add (new Dialogue.Choice ("Choice", ParseChoice (datas[1], i)));
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

        string[] data = choice.Split (separators, System.StringSplitOptions.RemoveEmptyEntries);
        if (data.Length % 2 != 0)
        {
            this.DebugLog ("Invalid count of data for choice at line " + line);
            return null;
        }

        for (int i = 0; i < (data.Length / 2); i++)
        {
            choices.Add (new KeyValuePair<string, string> (data[2 * i], data[2 * i + 1].Trim('"')));
        }

        return choices;
    }
}

public class DialogueManagerProxy : UniqueProxy<DialogueManager>
{
}

