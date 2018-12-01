using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Assertions;

public enum EInputState
{
    Down,
    Up,
    Held,
}

public class PlayerInputGameEvent : GameEvent
{
    public PlayerInputGameEvent (string input, EInputState state) : base ("Player", EProtocol.Instant)
    {
        m_Input = input;
        m_State = state;
    }

    public string GetInput ()
    {
        return m_Input;
    }

    public EInputState GetInputState ()
    {
        return m_State;
    }

    private string m_Input;
    private EInputState m_State;
}

public class InputManager
{
    private Dictionary<string, KeyCode> m_KeyCodes;
    private static string ms_InputFileName = "/Input.txt";

    public InputManager ()
    {
        this.RegisterToUpdate (EUpdatePass.BeforeAI);
        m_KeyCodes = new Dictionary<string, KeyCode> ();
        FillKeyCodes (ms_InputFileName);
    }

    public void UpdateBeforeAI()
    {
        foreach(string inputName in m_KeyCodes.Keys)
        {
            if(Input.GetKeyDown(m_KeyCodes[inputName]))
            {
               new PlayerInputGameEvent (inputName, EInputState.Down).Push();
            }
            if (Input.GetKeyUp (m_KeyCodes[inputName]))
            {
                new PlayerInputGameEvent (inputName, EInputState.Up).Push ();
            }
            if (Input.GetKey (m_KeyCodes[inputName]))
            {
                new PlayerInputGameEvent (inputName, EInputState.Held).Push ();
            }
        }
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown (KeyCode.Space))
        {
            new PlayerInputGameEvent ("Submit", EInputState.Down).Push ();
        }
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            new PlayerInputGameEvent ("Escape", EInputState.Down).Push ();
        }
    }

    public Dictionary<string, KeyCode> GetInputs ()
    {
        return m_KeyCodes;
    }

    public void ChangeKeyCode(string inputName, KeyCode newKeyCode)
    {
        if (m_KeyCodes.ContainsKey (inputName))
        {
            m_KeyCodes[inputName] = newKeyCode;
            PlayerPrefs.SetString (inputName, newKeyCode.ToString ());
        }
        else
        {
            Assert.IsTrue (false, "Cannot find input " + inputName);
        }
    }

    private void FillKeyCodes (string filename)
    {
        char[] separators = { ':' };
        filename = Application.streamingAssetsPath + filename;

        string[] lines = File.ReadAllLines (filename);

        for (int i = 0; i < lines.Length; i++)
        {
            string[] datas = lines[i].Split (separators);

            // If there is an error in print a debug message
            if (datas.Length != 2)
            {
                this.DebugLog ("Invalid number of data line " + i + " expecting 2, got " + datas.Length);
                return;
            }

            string inputName = datas[0];
            string defaultKey = datas[1];
            string keyCode = PlayerPrefs.GetString (inputName, defaultKey);
            keyCode = (!keyCode.Equals("None")) ? keyCode : defaultKey;
            KeyCode inputKeyCode = (KeyCode)System.Enum.Parse (typeof (KeyCode), keyCode);
            
            m_KeyCodes.Add (inputName, inputKeyCode);
        }
    }
}

public class InputManagerProxy : UniqueProxy<InputManager>
{
}