using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IAttackProvider, IDamageTaker
{
    [SerializeField]
    private Weapon EquippedWeapon = null;
    public void EquipWeapon(Weapon weapon)
    {
        throw new System.NotImplementedException("Can't equip weapons yet");
    }

    //IAttackProvider
    public float GetAttackRange()
    {
        if (EquippedWeapon != null)
            return EquippedWeapon.GetRange();
        else
        {
            Debug.LogError("No weapon, range is 0");
            return 0.0f;
        }
            
    }

    public float GetAttackDelay() //Delay between attack in seconds
    {
        return 1.0f / EquippedWeapon.GetAttackSpeed();
    }

    public float GetDamage()
    {
        return EquippedWeapon.AttackDamage;
    }

    public Enums.Element GetDamageType()
    {
        return EquippedWeapon.DamageType;
    }

    public Projectile GetProjectile()
    {
        if (EquippedWeapon is RangedWeapon)
            return ((RangedWeapon)EquippedWeapon).GetProjectile();
        else
            return null;
    }

    //IDamageTaker
    public void TakeHit(float damage, Enums.Element damageType)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetProjectileTargetPosition()
    {
        throw new System.NotImplementedException();
    }
}
