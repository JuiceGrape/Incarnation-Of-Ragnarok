using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryWeapon : InventoryItem
{
    [SerializeField]
    private ItemWeapon itemBase;

    public InventoryWeapon(ItemWeapon itemBase, Enums.Rarity rarity, bool equippable) : base(rarity, equippable)
    {
        this.itemBase = itemBase;
    }

    public ItemWeapon GetItemWeapon()
    {
        if (itemBase is ItemWeapon) return itemBase as ItemWeapon;
        else return null;
    }

    public Enums.SlotType GetSlotType()
    {
        if (itemBase == null) return Enums.SlotType.Invalid;
        return Enums.SlotType.Weapon;
    }

    public Enums.WeaponType GetWeaponType()
    {
        return itemBase.GetWeaponType();
    }

    public float GetBaseAttackSpeed()
	{
        return itemBase.GetBaseAttackSpeed();
	}

	public float GetBaseDamage ()
	{
        return itemBase.GetBaseAttackDamage();
	}

    public Enums.Element GetElement()
	{
        return itemBase.GetWeaponElement();
	}

    public float GetAttackRange ()
    {
        return itemBase.GetAttackRange();
    }

    public Projectile GetProjectile()
    {
        return itemBase.GetProjectile();
    }

    public override string GetItemName()
    {
        return itemBase.GetItemName();
    }

    public override string GetDescription()
    {
        return itemBase.GetItemDescription();
    }
}
