using UnityEngine;

public class CameraFollowEvent : GameEvent
{
    public CameraFollowEvent () : base ("Game")
    {

    }
}

public class CameraUnfollowEvent : GameEvent
{
    public CameraUnfollowEvent () : base ("Game")
    {

    }
}

public class CameraControllerScript : MonoBehaviour
{
    [SerializeField] private float minX;
    [SerializeField] private float minY;
    [SerializeField] private float maxX;
    [SerializeField] private float maxY;
    [SerializeField] private float m_CameraMoveSpeed;

    private float m_ScreenBorderMargin = 40;
    private bool m_FollowPlayer = false;
    
    void Start ()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
        this.RegisterAsListener ("Game", typeof (CameraFollowEvent), typeof (CameraUnfollowEvent));
	}

    void Update ()
    {
        if(UpdaterProxy.Get().IsPaused())
        {
            return;
        }

        if (m_FollowPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag ("Player");
            transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
        }
    }

    private void LateUpdate ()
    {
        var v3 = transform.position;
        v3.x = Mathf.Clamp (v3.x, minX, maxX);
        v3.y = Mathf.Clamp (v3.y, minY, maxY);
        transform.position = v3;
    }

    public void OnGameEvent (CameraFollowEvent gameEvent)
    {
        FollowPlayer ();
    }

    public void OnGameEvent (CameraUnfollowEvent gameEvent)
    {
        UnfollowPlayer ();
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Game");
    }

    public void FollowPlayer ()
    {
        m_FollowPlayer = true;
    }

    public void UnfollowPlayer ()
    {
        m_FollowPlayer = false;
    }
}
