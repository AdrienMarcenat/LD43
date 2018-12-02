using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class OnEdgeGameEvent : GameEvent
{
    public OnEdgeGameEvent (string tag, EdgeView edge, bool enter)
        : base(tag)
    {
        m_Edge = edge;
        m_Enter = enter;
    }

    public EdgeView GetEdge ()
    {
        return m_Edge;
    }

    public bool IsEntering()
    {
        return m_Enter;
    }

    private EdgeView m_Edge;
    private bool m_Enter;
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
        this.RegisterAsListener ("Player", typeof (OnEdgeGameEvent));
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Player");
    }

    public void OnGameEvent (OnEdgeGameEvent edgeEvent)
    {
        EdgeView edge = edgeEvent.GetEdge ();
        if (edgeEvent.IsEntering ())
        {
            MoveToEdge (edge);
        }
        else
        {
            Assert.IsTrue (edge.GetEdge ().UseNode (m_CurrentNode.GetNode ()));
            MoveToNode (m_CurrentNode == edge.GetStart () ? edge.GetEnd () : edge.GetStart ());
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
        StartCoroutine (NotifyNode(node));
    }

    IEnumerator NotifyNode (NodeView node)
    {
        yield return new WaitForSecondsRealtime (1f);
        node.OnEnter ();
    }

    public void MoveToEdge (EdgeView edge)
    {
        if (CanMoveToEdge(edge))
        {
            Vector3 start = edge.GetStart ().transform.position;
            Vector3 end = edge.GetEnd ().transform.position;
            m_TargetPos = (start + end) / 2;
            StartCoroutine (MoveRoutine ());
            StartCoroutine (PushEvent (new OnEdgeActionEvent ("Game", edge)));
            StartCoroutine (PushEvent (new GameFlowEvent (EGameFlowAction.EnterEdge)));
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
