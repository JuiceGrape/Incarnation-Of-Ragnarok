using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ranged", menuName = "ScriptableObjects/Weapons/Ranged")]
public class RangedWeapon : Weapon
{
    public float Range = 5.0f;

    public Projectile projectilePrefab;

    public override float GetRange()
    {
        return Range;
    }

    public Projectile GetProjectile()
    {
        return projectilePrefab;
    }

}
