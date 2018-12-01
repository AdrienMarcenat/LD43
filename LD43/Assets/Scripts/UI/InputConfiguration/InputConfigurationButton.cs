using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InputConfigurationButton : MonoBehaviour
{
    [SerializeField] private Text m_InputButtonText;
    private Event m_KeyEvent;
    private string m_InputName;

    private bool m_WaitingForKey;

    void Start ()
    {
        m_WaitingForKey = false;
    }

    public void StartAssignment ()
    {
        if (!m_WaitingForKey)
        {
            StartCoroutine (AssignKey ());
        }
    }

    public void SetInputName(string inputName)
    {
        m_InputName = inputName;
    }

    private void OnGUI ()
    {
        m_KeyEvent = Event.current;
    }

    private IEnumerator AssignKey ()
    {
        m_WaitingForKey = true;
        
        while (m_KeyEvent.keyCode == KeyCode.None || m_KeyEvent.keyCode == KeyCode.Return)
            yield return null;

        KeyCode newKeyCode = m_KeyEvent.keyCode;
        InputManagerProxy.Get ().ChangeKeyCode (m_InputName, newKeyCode);
        m_InputButtonText.text = newKeyCode.ToString ();

        m_WaitingForKey = false;
    }
}