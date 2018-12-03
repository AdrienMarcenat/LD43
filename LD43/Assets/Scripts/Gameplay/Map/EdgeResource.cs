using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum EEdgeType
{
    Normal,
    Combat,
    Obstacle,
    Diversion
}

[System.Serializable]
public class EdgeResource
{
    [SerializeField] private EEdgeType m_EdgeType;
    [SerializeField] private int m_EdgeId;
    [SerializeField] private string m_EdgeDescriptionID;
    [SerializeField] private int m_EdgeCharacterNumber = int.MaxValue;
    [SerializeField] private ECharacterClass m_EdgeCharacterClass = ECharacterClass.None;
    [SerializeField] private List<ECharacterClass> m_Enemies;

    private static string ms_EdgeDescriptionFileName = "/edgetext.txt";

    private string m_Description = null;

    private void FillDescription ()
    {
        if(m_Description != null || m_EdgeDescriptionID == "")
        {
            return;
        }

        string filename = ms_EdgeDescriptionFileName;
        filename = Application.streamingAssetsPath + filename;

        string[] lines = File.ReadAllLines (filename);

        int descriptionBeginning = 0;
        int descriptionEnd = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string datas = lines[i];

            // If there is a single word it is a dialog tag
            if (datas == m_EdgeDescriptionID)
            {
                descriptionBeginning = i + 2;
            }
            // We then seek for the a ] that signals the end of the description
            if (descriptionBeginning > 0 && datas == "]")
            {
                descriptionEnd = i;
                break;
            }
        }
        if (descriptionBeginning == 0)
        {
            this.DebugLog ("Could not find description with tag " + m_EdgeDescriptionID);
            return;
        }

        for (int i = descriptionBeginning; i < descriptionEnd; i++)
        {
            m_Description = string.Concat (m_Description, lines[i]);
        }
    }

    public EEdgeType GetEdgeType ()
    {
        return m_EdgeType;
    }

    public void SetNormal ()
    {
        m_EdgeType = EEdgeType.Normal;
        m_EdgeDescriptionID = "Normal";
        m_Description = null;
    }

    public int GetEdgeCharacterNumber ()
    {
        return m_EdgeCharacterNumber;
    }

    public ECharacterClass GetEdgeCharacterClass ()
    {
        return m_EdgeCharacterClass;
    }

    public string GetDescription()
    {
        FillDescription ();
        return m_Description;
    }

    public List<ECharacterClass> GetEnemies()
    {
        return m_Enemies;
    }
}
