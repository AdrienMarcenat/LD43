using UnityEngine;

public class EdgeEventPanel : MonoBehaviour
{
    private EdgeView m_CurrentEdge;

    private void Awake ()
    {
        this.RegisterAsListener ("Game", typeof (OnEdgeActionEvent));
        gameObject.SetActive (false);
    }

    public void OnGameEvent (OnEdgeActionEvent edgeEvent)
    {
        gameObject.SetActive (true);
        m_CurrentEdge = edgeEvent.GetEdge ();
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
    }

    public void Continue ()
    {
        new OnEdgeGameEvent ("Player", m_CurrentEdge, false).Push ();
        m_CurrentEdge = null;
        gameObject.SetActive (false);
    }
}

