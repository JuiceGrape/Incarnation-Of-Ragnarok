using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryEquipment : InventoryItem
{
    [SerializeField]
    private ItemEquipment itemBase;
    //defence stats maybe?

    public InventoryEquipment(ItemEquipment itemBase, Enums.Rarity rarity, bool equippable) : base(rarity, equippable)
    {
        this.itemBase = itemBase;
    }

    public Enums.SlotType GetSlotType()
    {
        if (itemBase == null) return Enums.SlotType.Invalid;
        return Enums.SlotType.Weapon;
    }

    public ItemEquipment GetItemArmor()
    {
        if (itemBase is ItemEquipment) return itemBase;
        else return null;
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
