using System.Collections;
using UnityEngine;

public class EnterNodeGameEvent : GameEvent
{
    public EnterNodeGameEvent (string tag, NodeView node)
        : base(tag)
    {
        m_Node = node;
    }

    public NodeView GetNode()
    {
        return m_Node;
    }

    private NodeView m_Node;
}

public class EnterEdgeGameEvent : GameEvent
{
    public EnterEdgeGameEvent (string tag, EdgeView edge)
        : base (tag)
    {
        m_Edge = edge;
    }

    public EdgeView GetEdge ()
    {
        return m_Edge;
    }

    private EdgeView m_Edge;
}

[RequireComponent (typeof (MovingObject))]
public class OverworldPlayerController : MonoBehaviour
{
    [SerializeField] private float m_MoveSpeed = 50f;
    [SerializeField] private NodeView m_CurrentNode;

    private bool m_IsMoving = false;
    private Vector3 m_TargetPos;
    //private Animator m_Animator;

    void Start ()
    {
        m_TargetPos = m_CurrentNode.transform.position;
        transform.position = m_TargetPos;
        //m_Animator = GetComponent<Animator> ();
        this.RegisterAsListener ("Player", typeof (EnterNodeGameEvent), typeof (EnterEdgeGameEvent));
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Player");
    }

    public void OnGameEvent (EnterNodeGameEvent enterNodeEvent)
    {
        MoveToNode (enterNodeEvent.GetNode ());
    }

    public void OnGameEvent (EnterEdgeGameEvent enterEdgeEvent)
    {
        MoveToEdge (enterEdgeEvent.GetEdge ());
    }

    public void MoveToNode (NodeView node)
    {
        if (m_CurrentNode.GetNode ().IsNeighbor (node.GetNode()))
        {
            m_CurrentNode = node;
            m_TargetPos = m_CurrentNode.transform.position;
            StartCoroutine (MoveRoutine ());
        }
    }

    public void MoveToEdge (EdgeView edge)
    {
        Vector3 start = edge.GetStart ().transform.position;
        Vector3 end = edge.GetEnd ().transform.position;
        m_TargetPos = (start + end) / 2;
        StartCoroutine (MoveRoutine ());
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
