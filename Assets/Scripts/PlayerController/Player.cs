using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour //Acts like a layer between equipment and logic to provide calculations for damage / range / other
{
    [SerializeField]
    private InventoryWeapon EquippedWeapon = null;

    public float GetAttackRange()
    {
        if(EquippedWeapon != null)
            return EquippedWeapon.GetAttackRange();
        else
		{
            Debug.LogError("No Weapon, range is 0");
            return 0.0f;
		}            
    }

    public void EquipItem(InventoryItem item)
    {
        if (item is InventoryWeapon)
        {
            EquipWeapon(item as InventoryWeapon);
        }
        else if (item is InventoryEquipment)
        {
            //stuff for equipping equipment
        }
    }

    private void EquipWeapon(InventoryWeapon weapon)
    {
        EquippedWeapon = weapon;
    }

    public float GetAttackDelay() //Delay between attack in seconds
    {
        return 1.0f / EquippedWeapon.GetBaseAttackSpeed();
    }

    public float GetDamage()
    {
        return EquippedWeapon.GetBaseDamage();
    }

    public Enums.Element GetDamageType()
    {
        return EquippedWeapon.GetElement();
    }

    public Projectile GetProjectile()
    {
        if (EquippedWeapon.GetWeaponType().Equals(Enums.WeaponType.Ranged))
            return EquippedWeapon.GetProjectile();
        else
            return null;
    }
}
