using UnityEngine;

public class Command
{
    public Command (GameObject actor)
    {
        m_Actor = actor;
    }

    public virtual void Execute () { }
    public virtual void Undo () { }

    protected GameObject m_Actor;
};

