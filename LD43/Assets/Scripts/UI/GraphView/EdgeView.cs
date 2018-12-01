using UnityEngine;

public class EdgeView : MonoBehaviour
{
    [SerializeField] private NodeView m_Start;
    [SerializeField] private NodeView m_End;
    [SerializeField] private bool m_IsOriented;

    [SerializeField] private Sprite m_Sprite;

    public NodeView GetStart()
    {
        return m_Start;
    }

    public NodeView GetEnd ()
    {
        return m_End;
    }

    public bool IsOriented()
    {
        return m_IsOriented;
    }
}
