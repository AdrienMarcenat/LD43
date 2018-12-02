using UnityEngine;

public class NodeView : MonoBehaviour
{
    private GameNode m_Node;
    [SerializeField] private Sprite m_Sprite;
    [SerializeField] private string m_Title;
    [SerializeField] private bool m_IsVisible = false;

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

    public void OnMouseUp ()
    {
    }
}
