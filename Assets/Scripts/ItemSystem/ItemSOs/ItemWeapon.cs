using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon", fileName = "New Weapon")]
public class ItemWeapon : ItemBaseObject
{
    [Header("Weapon Attributes")]
    [SerializeField]
    private GameObject weaponModel;
    [SerializeField]
    private Enums.WeaponType weaponType = Enums.WeaponType.Melee;
    [SerializeField]
    private float attackRange = 3.0f;
    [SerializeField]
    private float baseAttackSpeed = 1.0f; // Attacks per second
    [SerializeField]
    private float baseAttackDamage = 1.0f; // Damage per hit
    [SerializeField]
    private Enums.Element weaponElement = Enums.Element.Physical; // Weapon Element
    [SerializeField]
    private Projectile projectilePrefab;

    public GameObject GetWeaponModel()
    {
        return weaponModel;
    }

    public Enums.WeaponType GetWeaponType()
    {
        return weaponType;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetBaseAttackSpeed()
    {
        return baseAttackSpeed;
    }

    public float GetBaseAttackDamage()
    {
        return baseAttackDamage;
    }

    public Enums.Element GetWeaponElement()
    {
        return weaponElement;
    }

    public Projectile GetProjectile()
    {
        return projectilePrefab;
    }
}
