using UnityEngine;

public class OnEdgeClick : GameEvent
{
    public OnEdgeClick (string tag, EdgeView edge)
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

public class OnEdgeActionEvent : GameEvent
{
    public OnEdgeActionEvent (string tag, EdgeView edge)
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

[RequireComponent(typeof(EdgeCollider2D))]
public class EdgeView : MonoBehaviour
{
    private GameEdge m_Edge;
    private SpriteRenderer m_Sprite;

    [SerializeField] private EdgeResource m_Resource;
    [SerializeField] private NodeView m_Start;
    [SerializeField] private NodeView m_End;
    [SerializeField] private bool m_IsOriented;
    [SerializeField] private bool m_IsVisible = false;
    [SerializeField] private float m_Width = 5;

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

    public void OnGameEvent (OnNodeEnterEvent nodeEvent)
    {
        if (m_Edge.UseNode(nodeEvent.GetNode ().GetNode()))
        {
            SetIsVisible (true);
        }
    }

    public void ResizeCollider ()
    {
        if (IsValid ())
        {
            EdgeCollider2D collider = GetComponent<EdgeCollider2D> ();
            collider.points = new Vector2[]{ m_Start.transform.position, m_End.transform.position };
            collider.edgeRadius = 2f;
            collider.isTrigger = true;
            Transform sprite = transform.Find("Sprite");
            if(sprite != null)
                StrechSprite (sprite.gameObject, m_Start.transform.position, m_End.transform.position, m_Width);
        }
    }

    private void StrechSprite (GameObject _sprite, Vector3 _initialPosition, Vector3 _finalPosition, float yScale)
    {
        Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
        _sprite.transform.position = centerPos;
        Vector3 direction = _finalPosition - _initialPosition;
        direction = Vector3.Normalize (direction);
        _sprite.transform.right = direction;
        Vector3 scale = new Vector3 (1, 1, 1);
        scale.x = Vector3.Distance (_initialPosition, _finalPosition);
        scale.y = yScale;
        _sprite.transform.localScale = scale;
    }

    public void BuildEdge()
    {
        if(m_Edge != null)
        {
            m_Edge.Shutdown ();
        }
        if (IsValid ())
        {
            m_Edge = new GameEdge (null, m_Start.GetNode (), m_End.GetNode (), m_IsOriented);
        }
    }

    public GameEdge GetEdge()
    {
        return m_Edge;
    }

    public EdgeResource GetEdgeResource ()
    {
        return m_Resource;
    }

    public NodeView GetStart()
    {
        return m_Start;
    }

    public NodeView GetEnd ()
    {
        return m_End;
    }
    
    public void SetStart (NodeView start)
    {
        m_Start = start;
    }

    public void SetEnd (NodeView end)
    {
        m_End = end;
    }

    public bool IsOriented()
    {
        return m_IsOriented;
    }

    public void SetIsOriented (bool isOriented)
    {
        m_IsOriented = isOriented;
    }

    public bool IsVisible()
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

    public bool IsValid()
    {
        return m_Start != null && m_End != null;
    }

    private void OnMouseUp ()
    {
        if (m_IsVisible && !UpdaterProxy.Get ().IsPaused())
        {
            new OnEdgeClick ("Game", this).Push ();
        }
    }

    private void OnMouseEnter ()
    {
        if (!UpdaterProxy.Get ().IsPaused ())
        {
            m_Sprite.color = Color.green;
        }
    }

    private void OnMouseExit ()
    {
        m_Sprite.color = Color.white;
    }
}
