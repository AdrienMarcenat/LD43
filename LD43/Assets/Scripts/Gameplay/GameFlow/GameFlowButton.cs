using UnityEngine;

public class GameFlowButton : MonoBehaviour
{
    [SerializeField] EGameFlowAction m_Action;

    public void GameFlowAction ()
    {
        new GameFlowEvent (m_Action).Push ();
    }
}
