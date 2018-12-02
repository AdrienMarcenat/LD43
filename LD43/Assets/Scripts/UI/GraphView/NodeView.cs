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

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
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

    public void OnEnter ()
    {
        if (!m_IsVisited)
        {
            string tag = m_Resource.GetDialogueTag ();
            if (tag != "")
            {
                DialogueManagerProxy.Get ().TriggerDialogue (tag);
            }
            m_IsVisited = true;
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
