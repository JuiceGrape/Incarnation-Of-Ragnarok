using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee", menuName = "ScriptableObjects/Weapons/Melee")]
public class MeleeWeapon : Weapon
{
    private static float MeleeRange = 3f;
    public override float GetRange()
    {
        return MeleeRange;
    }

}
