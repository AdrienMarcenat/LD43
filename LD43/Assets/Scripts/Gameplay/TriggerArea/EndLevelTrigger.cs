using UnityEngine;

public class EndLevelGameEvent : GameEvent
{
    public EndLevelGameEvent () : base ("Level")
    {

    }
}

[RequireComponent (typeof (Collider2D))]
public class EndLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
            new EndLevelGameEvent ();
        }
    }
}

