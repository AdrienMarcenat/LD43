using System;
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

public class CameraControllerScript : MonoBehaviour {

    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    public float m_CameraMoveSpeed;
    private float m_ScreenBorderMargin = 40;

    private bool m_FollowPlayer = false;


    // Use this for initialization
    void Start ()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
        this.RegisterAsListener ("Game", typeof (CameraFollowEvent), typeof (CameraUnfollowEvent));
	}

    // Update is called once per frame
    void Update ()
    {
        if(UpdaterProxy.Get().IsPaused())
        {
            return;
            }

        if (!m_FollowPlayer)
        {
            // Mouse movement
            if (Input.mousePosition.y <= 20 && Input.mousePosition.y >= 0)
            {
                transform.Translate (Vector3.down * m_CameraMoveSpeed * Time.deltaTime, Space.Self);
            }
            if (Input.mousePosition.y <= Screen.height && Input.mousePosition.y >= Screen.height - m_ScreenBorderMargin)
            {
                transform.Translate (Vector3.up * m_CameraMoveSpeed * Time.deltaTime, Space.Self);
            }

            if (Input.mousePosition.x <= 20 && Input.mousePosition.x >= 0)
            {
                transform.Translate (Vector3.left * m_CameraMoveSpeed * Time.deltaTime, Space.Self);
            }
            if (Input.mousePosition.x <= Screen.width && Input.mousePosition.x >= Screen.width - m_ScreenBorderMargin)
            {
                transform.Translate (Vector3.right * m_CameraMoveSpeed * Time.deltaTime, Space.Self);
            }

            //Keyboard movement
            transform.Translate ((new Vector3 (0, Input.GetAxis ("Vertical"), 0) * m_CameraMoveSpeed * Time.deltaTime), Space.Self);
            transform.Translate ((new Vector3 (Input.GetAxis ("Horizontal"), 0, 0) * m_CameraMoveSpeed * Time.deltaTime), Space.Self);
        }
        else
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
