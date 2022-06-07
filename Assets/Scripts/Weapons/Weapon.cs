using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public GameObject WeaponModel;

    public float AttackSpeed = 1.0f; //Attacks per second
    public float AttackDamage = 1.0f; //Damage per hit (sorta, based on the type of attack, modifiers might be added)
    public Enums.Element DamageType = Enums.Element.Physical;

    public abstract float GetRange();

    public float GetAttackSpeed() //Attacks per sercond
    {
        return AttackSpeed;
    }

    public Enums.Element GetDamageType()
    {
        return DamageType;
    }

    

}
