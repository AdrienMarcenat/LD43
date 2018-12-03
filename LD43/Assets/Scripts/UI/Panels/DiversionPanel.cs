using UnityEngine;
using UnityEngine.UI;

public class OnDiversionEvent : GameEvent
{
    public OnDiversionEvent () : base ("Game")
    {

    }
}

public class OnCharacterDiversionEvent : GameEvent
{
    public OnCharacterDiversionEvent () : base ("Game")
    {

    }
}

public class DiversionPanel : MonoBehaviour
{
    [SerializeField] private Text m_Warning;
    
	void Awake () {
        this.RegisterAsListener ("Game", typeof (OnDiversionEvent));
        this.RegisterAsListener ("Game", typeof (OnCharacterDiversionEvent));
        gameObject.SetActive (false);
	}
	
    public void OnGameEvent (OnDiversionEvent diversionEvent)
    {
        gameObject.SetActive (true);
        UpdaterProxy.Get ().SetPause (true);
        TeamManagerProxy.Get ().WaitForDiversion ();
    }

    public void OnGameEvent (OnCharacterDiversionEvent characterDiversionEvent)
    {
        gameObject.SetActive (false);
        UpdaterProxy.Get ().SetPause (false);
    }

    public void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
    }
}
