using UnityEngine;

public class EndLevelPanel : MonoBehaviour
{
    private void Awake ()
    {
        gameObject.SetActive (false);
        this.RegisterAsListener ("Game", typeof (GameFlowEvent));
    }

    public void OnGameEvent (GameFlowEvent gameFlowEvent)
    {
        if(gameFlowEvent.GetAction() == EGameFlowAction.EndLevelPanel)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
    }
}

