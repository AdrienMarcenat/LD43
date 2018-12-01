using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class PatternEnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform m_SpawningLocation;
    [SerializeField] private Transform m_ShootDirection;
    [SerializeField] private List<GameObject> m_EnemyPrefabs;
    [SerializeField] private float m_EnemySpawningDelay;
    [SerializeField] private BezierCurve m_Curve;

    private float m_Delay;
    private int m_CurrentIndex;

    void Start ()
    {
        m_Delay = m_EnemySpawningDelay;
        m_CurrentIndex = 0;
    }

    void Update ()
    {
        if (m_Delay < m_EnemySpawningDelay)
        {
            m_Delay += Time.deltaTime;
        }
        else if (m_CurrentIndex < m_EnemyPrefabs.Count)
        {
            m_Delay = 0;
            GameObject enemy = Instantiate (m_EnemyPrefabs[m_CurrentIndex]);
            if (m_ShootDirection)
            {
                EnemyAI ai = enemy.GetComponent<EnemyAI> ();
                ai.SetShootDirection (m_ShootDirection);
            }
            if (m_Curve)
            {
                EnemyPath path = enemy.GetComponent<EnemyPath> ();
                path.SetPath (m_Curve);
            }
            enemy.transform.position = m_SpawningLocation.position;
            m_CurrentIndex++;
        }
        else
        {
            Destroy (this);
        }
    }
}

