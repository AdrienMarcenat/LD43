using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeResource : MonoBehaviour {

    [SerailizeField] private int m_EdgeType;
    private int m_EdgeId;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetEdgeId(int newEdgeId)
    {
        m_EdgeId = newEdgeId;
    }

    public int GetEdgeType ()
    {
        return m_EdgeType;
    }

    public int GetEdgeId ()
    {
        return m_EdgeId;
    }
}
