using UnityEngine;

[RequireComponent(typeof(MovingObject))]
[RequireComponent (typeof (WeaponManager))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 5;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;

    private MovingObject m_Mover;
    private WeaponManager m_WeaponManager;
    private Transform m_GroundCheck;
    private const float ms_GroundedRadius = 0.2f;
    private bool m_Grounded;
    private Vector3 m_FacingDirection;

    private static Vector3 ms_Right = new Vector3 (1, 0, 0);
    private static Vector3 ms_Left = new Vector3 (-1, 0, 0);

    void Awake ()
    {
        m_Mover = GetComponent<MovingObject> ();
        m_WeaponManager = GetComponent<WeaponManager> ();
        m_GroundCheck = transform.Find ("GroundCheck");
        m_FacingDirection = new Vector3 (1, 0, 0);
        this.RegisterAsListener ("Player", typeof(PlayerInputGameEvent));
    }

    private void OnDestroy ()
    {
        this.UnregisterAsListener ("Player");
    }

    private void FixedUpdate ()
    {
        m_Grounded = false;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll (m_GroundCheck.position, ms_GroundedRadius, m_WhatIsGround);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
                m_Grounded = true;
        }
    }

    public void OnGameEvent(PlayerInputGameEvent inputEvent)
    {
        string input = inputEvent.GetInput ();
        EInputState state = inputEvent.GetInputState ();
        switch (input)
        {
            case "Jump":
                if (state == EInputState.Down)
                {
                    Jump ();
                }
                break;
            case "Right":
                Move (state == EInputState.Held ? 1 : 0);
                m_FacingDirection = ms_Right;
                break;
            case "Left":
                Move (state == EInputState.Held ? -1 : 0);
                m_FacingDirection = ms_Left;
                break;
            case "Fire":
                if (state == EInputState.Held)
                {
                    Fire ();
                }
                break;
            default:
                break;
        }
    }

    private void Jump()
    {
        if (m_Grounded)
        {
            // Don't wait fo the physic to set m_Grounded to false to ensure applying the force only once
            m_Grounded = false;
            m_Mover.ApplyForce (new Vector2 (0f, m_JumpForce));
        }
    }

    private void Move (float xDir)
    {
        if (m_Grounded || m_AirControl)
        {
            m_Mover.MoveHorizontal (xDir);
        }
    }

    private void Fire ()
    {
        m_WeaponManager.Fire (0, m_FacingDirection);
    }
}
