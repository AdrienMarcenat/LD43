using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField] private int m_NodeType;
    private int m_NodeId;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetNodeId (int newNodeId)
    {
        m_NodeId = newNodeId;
    }

    public int GetNodeType ()
    {
        return m_NodeType;
    }

    public int GetNodeId ()
    {
        return m_NodeId;
    }
}
