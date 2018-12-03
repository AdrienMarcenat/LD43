using UnityEngine;
using UnityEngine.UI;

public class EdgePanel : MonoBehaviour
{
    private EdgeView m_CurrentEdge;
    private OverworldPlayerController m_Player;
    [SerializeField] private Button m_ChoosePathButton;
    [SerializeField] private Text m_Description;
    private EEdgeType m_MoveType;

    private void Awake ()
    {
        m_Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<OverworldPlayerController> ();
        this.RegisterAsListener ("Game", typeof (OnEdgeClick));
        gameObject.SetActive (false);
    }
    
    public void OnGameEvent (OnEdgeClick edgeEvent)
    {
        gameObject.SetActive (true);
        UpdaterProxy.Get ().SetPause (true);
        m_CurrentEdge = edgeEvent.GetEdge ();
        m_ChoosePathButton.interactable = m_Player.CanMoveToEdge(m_CurrentEdge);

        EdgeResource tempEdgeResource = m_CurrentEdge.GetEdgeResource ();

        switch (tempEdgeResource.GetEdgeType ())
        {
            case EEdgeType.Normal:
                m_MoveType = EEdgeType.Normal;
                break;
            case EEdgeType.Combat:
                if (TeamManagerProxy.Get ().IsNotTooMuchCharacters (tempEdgeResource.GetEdgeCharacterNumber ()))
                {
                    m_MoveType = EEdgeType.Normal;
                }
                else
                {
                    m_MoveType = EEdgeType.Combat;
                }
                break;
            case EEdgeType.Obstacle:
                if (TeamManagerProxy.Get ().IsCharacterClass (tempEdgeResource.GetEdgeCharacterClass ()))
                {
                    m_MoveType = EEdgeType.Normal;
                }
                else
                {
                    m_MoveType = EEdgeType.Obstacle;
                    m_ChoosePathButton.interactable = false;
                }
                break;
            case EEdgeType.Diversion:
                if (TeamManagerProxy.Get ().IsNotTooMuchCharacters (tempEdgeResource.GetEdgeCharacterNumber ()))
                {
                    m_MoveType = EEdgeType.Normal;
                }
                else
                {
                    m_MoveType = EEdgeType.Diversion;
                }
                break;
        }

        m_Description.text = tempEdgeResource.GetDescription ();
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
    }

    public void Cancel()
    {
        m_CurrentEdge = null;
        gameObject.SetActive (false);
        UpdaterProxy.Get ().SetPause (false);
    }

    public void ChoosePath()
    {
        switch (m_MoveType )
        {
            case EEdgeType.Normal:
                m_Player.OnNormalEdge(m_CurrentEdge);
                break;
            case EEdgeType.Combat:
                new OnEdgeBattleGameEvent (true, m_CurrentEdge).Push ();
                m_Player.OnEdge (m_CurrentEdge, true);
                break;
            case EEdgeType.Obstacle:
                break;
            case EEdgeType.Diversion:
                new OnDiversionEvent ();
                m_Player.OnEdge (m_CurrentEdge, true);
                break;
        }

        gameObject.SetActive (false);
        UpdaterProxy.Get ().SetPause (false);
    }
}

