using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent (typeof (MovingObject))]
public class OverworldPlayerController : MonoBehaviour
{
    [SerializeField] private float m_MoveSpeed = 50f;
    [SerializeField] private NodeView m_CurrentNode;

    private bool m_IsMoving = false;
    private Vector3 m_TargetPos;
    private EdgeView m_CurrentEdge;
    //private Animator m_Animator;

    void Start ()
    {
        m_TargetPos = m_CurrentNode.transform.position;
        transform.position = m_TargetPos;
        //m_Animator = GetComponent<Animator> ();
    }

    private void OnDestroy ()
    {
    }

    public void OnEdge (EdgeView edge, bool enter)
    {
        if (enter)
        {
            m_CurrentEdge = edge;
            MoveToEdge (edge);
        }
        else
        {
            Assert.IsTrue (m_CurrentEdge.GetEdge ().UseNode (m_CurrentNode.GetNode ()));
            MoveToNode (m_CurrentNode == m_CurrentEdge.GetStart () ? m_CurrentEdge.GetEnd () : m_CurrentEdge.GetStart ());
        }
    }

    public bool CanMoveToEdge(EdgeView edge)
    {
        if(m_CurrentNode == edge.GetStart())
        {
            return m_CurrentNode.GetNode ().IsNeighbor (edge.GetEnd ().GetNode());
        }
        else if (m_CurrentNode == edge.GetEnd ())
        {
            return m_CurrentNode.GetNode ().IsNeighbor (edge.GetStart ().GetNode ());
        }
        return false;
    }

    public void MoveToNode (NodeView node)
    {
        m_CurrentNode = node;
        m_TargetPos = m_CurrentNode.transform.position;
        StartCoroutine (MoveRoutine ());
        StartCoroutine (PushEvent (new OnNodeEnterEvent("Game", node)));
    }
    
    public void MoveToEdge (EdgeView edge)
    {
        if (CanMoveToEdge(edge))
        {
            Vector3 start = edge.GetStart ().transform.position;
            Vector3 end = edge.GetEnd ().transform.position;
            m_TargetPos = (start + end) / 2;
            StartCoroutine (MoveRoutine ());
            new GameFlowEvent (EGameFlowAction.EnterEdge).Push();
        }
    }

    IEnumerator PushEvent (GameEvent e)
    {
        yield return new WaitForSecondsRealtime (1f);
        e.Push ();
    }

    IEnumerator MoveRoutine ()
    {
        SetIsMoving (true);
        while (transform.position != m_TargetPos)
        {
            transform.position = Vector3.MoveTowards (transform.position, m_TargetPos, Time.deltaTime * m_MoveSpeed);
            yield return null;
        }
        SetIsMoving (false);
    }

    private void SetIsMoving (bool isMoving)
    {
        m_IsMoving = isMoving;
        //m_Animator.SetBool ("IsMoving", isMoving);
    }
}
