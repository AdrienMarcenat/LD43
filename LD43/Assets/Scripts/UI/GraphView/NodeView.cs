using UnityEngine;

public class OnNodeEnterEvent : GameEvent
{
    public OnNodeEnterEvent (string tag, NodeView node)
        : base (tag)
    {
        m_Node = node;
    }

    public NodeView GetNode ()
    {
        return m_Node;
    }

    private NodeView m_Node;
}

public class NodeView : MonoBehaviour
{
    private GameNode m_Node;
    private SpriteRenderer m_Sprite;
    [SerializeField] private bool m_IsVisited = false;
    [SerializeField] private NodeResource m_Resource;
    [SerializeField] private string m_Title;
    [SerializeField] private bool m_IsVisible = false;

    private void Awake ()
    {
        m_Sprite = GetComponentInChildren<SpriteRenderer> ();
        SetIsVisible (m_IsVisible);
        this.RegisterAsListener ("Game", typeof (OnNodeEnterEvent));
    }

    private void Start ()
    {
        if (m_IsVisible)
        {
            new OnNodeEnterEvent ("Game", this).Push ();
        }
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
    }

    public void OnGameEvent (OnNodeEnterEvent nodeEvent)
    {
        if (nodeEvent.GetNode () == this)
        {
            SetIsVisible (true);
            OnEnter ();
        }
        else if(nodeEvent.GetNode().GetNode().IsNeighbor(m_Node))
        {
            SetIsVisible (true);
        }
    }

    public void BuildNode()
    {
        m_Node = new GameNode ();
    }

    public string GetTitle ()
    {
        return m_Title;
    }

    public void SetTitle (string title)
    {
        m_Title = title;
    }

    public GameNode GetNode ()
    {
        return m_Node;
    }

    public NodeResource GetNodeResource ()
    {
        return m_Resource;
    }

    private void OnEnter ()
    {
        if (!m_IsVisited)
        {
            string tag = m_Resource.GetDialogueTag ();
            m_IsVisited = true;

            switch (m_Resource.GetNodeType ())
            {
                case ENodeType.Normal:
                    if (tag != "")
                    {
                        DialogueManagerProxy.Get ().TriggerDialogue (tag);
                    }
                    break;
                case ENodeType.Battle:
                    new OnNodeBattleGameEvent (true, this).Push ();
                    break;
                case ENodeType.Recruitment:
                    DialogueManagerProxy.Get ().TriggerDialogue (tag);
                    break;
                case ENodeType.Key:
                    DialogueManagerProxy.Get ().TriggerDialogue ("Key Found");
                    TeamManagerProxy.Get ().GetKey ();
                    break;
                case ENodeType.End:
                    if (m_Resource.NeedKey ())
                    {
                        if (TeamManagerProxy.Get ().HasKey ())
                        {
                            DialogueManagerProxy.Get ().TriggerDialogue (tag);
                            new GameFlowEvent (EGameFlowAction.LevelWon).Push ();
                        }
                        else
                        {
                            DialogueManagerProxy.Get ().TriggerDialogue ("No Key");
                            m_IsVisited = false;
                        }
                    }
                    else
                    {
                        DialogueManagerProxy.Get ().TriggerDialogue (tag);
                        new GameFlowEvent (EGameFlowAction.LevelWon).Push ();
                    }
                    break;
                default:
                    if (tag != "")
                    {
                        DialogueManagerProxy.Get ().TriggerDialogue (tag);
                    }
                    break;
            }
        }
    }

    public bool IsVisible ()
    {
        return m_IsVisible;
    }

    public void SetIsVisible (bool visible)
    {
        m_IsVisible = visible;
        if (m_IsVisible)
        {
            m_Sprite.enabled = true;
        }
        else
        {
            m_Sprite.enabled = false;
        }
    }
}
