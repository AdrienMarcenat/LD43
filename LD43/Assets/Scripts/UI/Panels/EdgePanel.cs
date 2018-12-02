using UnityEngine;
using UnityEngine.UI;

public class EdgePanel : MonoBehaviour
{
    private EdgeView m_CurrentEdge;
    [SerializeField] private Button m_ChoosePathButton;

    private void Awake ()
    {
        gameObject.SetActive (false);
        this.RegisterAsListener ("Game", typeof (OnEdgeClick));
    }

    public void OnGameEvent (OnEdgeClick edgeEvent)
    {
        gameObject.SetActive (true);
        m_CurrentEdge = edgeEvent.GetEdge ();
        m_ChoosePathButton.interactable = false;
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
    }

    public void Cancel()
    {
        m_CurrentEdge = null;
        gameObject.SetActive (false);
    }

    public void ChoosePath()
    {
        new OnEdgeGameEvent ("Player", m_CurrentEdge, true).Push ();
        gameObject.SetActive (false);
    }
}

