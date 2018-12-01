using UnityEngine;

[System.Serializable]
public class NodeView : MonoBehaviour
{
    [SerializeField] private Sprite m_Sprite;
    [SerializeField] private string m_Title;

    public string GetTitle ()
    {
        return m_Title;
    }

    public void SetTitle (string title)
    {
        m_Title = title;
    }
}
