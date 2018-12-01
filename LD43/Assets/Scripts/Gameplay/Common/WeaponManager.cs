using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class WeaponManager : MonoBehaviour
{
    private Dictionary<int, Weapon> m_Weapons;

    void Start ()
    {
        m_Weapons = new Dictionary<int, Weapon> ();
        foreach (Weapon w in GetComponentsInChildren<Weapon> ())
        {
            m_Weapons.Add (w.GetWeaponType (), w);
        }
    }

    public void Fire (int weaponType, Vector3 target, float sizeModifier = 1f)
    {
        if (m_Weapons.ContainsKey (weaponType))
        {
            m_Weapons[weaponType].Fire (target, sizeModifier);
        }
        else
        {
            Assert.IsTrue (false, "Wrong weapon Type");
        }
    }

    public void AddFireCommand (int weaponType, float numberOfShots, float sizeModifier, Vector3 target)
    {
        if (m_Weapons.ContainsKey (weaponType))
        {
            m_Weapons[weaponType].AddFireCommand (numberOfShots, sizeModifier, target);
        }
        else
        {
            Assert.IsTrue (false, "Wrong weapon Type");
        }
    }
}

