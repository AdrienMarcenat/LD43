using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    protected float m_StartTime;
    protected float m_CurrentTime;

    public List<LevelSequence> m_OrderSequences;
    protected int m_SequenceId;
    protected int m_OrderId;

    void Awake ()
    {
        Reset ();
    }

    void Start ()
    {
        this.ParseFile ("Assets/Levels/Level01_Tutorial.lvl");
    }

    public void Reset ()
    {
        m_SequenceId = 0;
        m_OrderId = 0;
    }

    void Update ()
    {
        m_CurrentTime += Time.deltaTime;
        if (m_SequenceId >= m_OrderSequences.Count)
        {
            return;
        }
        LevelSequence currentSequence = m_OrderSequences[m_SequenceId];
        while (m_OrderId < currentSequence.m_Orders.Count)
        {
            LevelOrder currentOrder = currentSequence.m_Orders[m_OrderId];
            if (currentOrder.GetTime() > m_CurrentTime)
            {
                break;
            }
            currentOrder.ExecuteOrder ();
            m_OrderId++;
        }
    }
}
