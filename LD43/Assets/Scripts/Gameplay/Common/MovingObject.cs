using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingObject : MonoBehaviour
{
    [SerializeField] private float m_SmoothSpeed = 5f;
    private Rigidbody2D m_RigidBody;

	void Start ()
	{
        m_RigidBody = GetComponent <Rigidbody2D> ();
	}

	public void Move (float xDir, float yDir)
	{
        m_RigidBody.velocity = m_SmoothSpeed * (new Vector2 (xDir, yDir).normalized);
	}

    public void ApplyImpulse(Vector2 impulse)
    {
        m_RigidBody.AddForce (impulse, ForceMode2D.Impulse);
    }

    public void ApplyForce (Vector2 force)
    {
        m_RigidBody.AddForce (force, ForceMode2D.Impulse);
    }

    public void MoveHorizontal (float xDir)
    {
        m_RigidBody.velocity = new Vector2 (xDir * m_SmoothSpeed, m_RigidBody.velocity.y);
    }
}