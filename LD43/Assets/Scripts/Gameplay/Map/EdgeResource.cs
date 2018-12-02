using System.IO;
using UnityEngine;

[System.Serializable]
public class EdgeResource
{
    [SerializeField] private int m_EdgeType;
    [SerializeField] private int m_EdgeId;
    [SerializeField] private string m_EdgeDescriptionID;

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

    public string GetDescription()
    {
        FillDescription ();
        return m_Description;
    }
}
