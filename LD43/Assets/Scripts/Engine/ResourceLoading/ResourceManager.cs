using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManager
{
    public static GameObject LoadPrefab (string name)
    {
        GameObject prefab = (GameObject)Resources.Load ("Prefabs/" + name, typeof (GameObject));
        if (!prefab)
        {
            Debug.LogWarning ("Prefab " + name + " could not be loaded");
        }
        return prefab;
    }

    public static Sprite LoadSprite (string name, int index)
    {
        Sprite[] sprite = Resources.LoadAll<Sprite> ("Sprites/" + name);
        if (index >= sprite.Length)
        {
            Debug.LogWarning ("Sprite " + name + " could not be loaded");
        }
        return sprite[index];
    }
}
