using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MovingObject))]
public class MovingEnemy : Enemy
{
    private MovingObject m_Body;

    void Start ()
    {
        m_Body = GetComponent<MovingObject> ();
    }

    void Update ()
    {
        MoveEnemy ();
    }

    private void MoveEnemy ()
    {
        float horizontal = m_Target.transform.position.x - transform.position.x;
        float vertical = m_Target.transform.position.y - transform.position.y;

        m_Body.Move (horizontal, vertical);
    }
}
