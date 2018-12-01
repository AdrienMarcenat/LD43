using UnityEngine;
using UnityEngine.UI;

public class InputConfigurationMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_ConfigurationButton;

    void Start ()
    {
        InputManager inputManager = InputManagerProxy.Get ();
        int buttonCount = 0;
        foreach (string inputName in inputManager.GetInputs ().Keys)
        {
            GameObject inputConfigurationButtonObject = Instantiate (m_ConfigurationButton);
            inputConfigurationButtonObject.transform.SetParent (transform, false);
            inputConfigurationButtonObject.GetComponent<InputConfigurationButton> ().SetInputName (inputName);
            Text[] texts = inputConfigurationButtonObject.GetComponentsInChildren<Text> ();
            texts[1].text = inputManager.GetInputs ()[inputName].ToString ();
            texts[0].text = inputName;
            buttonCount++;
        }
    }
}