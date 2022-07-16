using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class InventoryItem
{
    [SerializeField]
    private Enums.Rarity itemRarity;
    [SerializeField]
    private bool equippable;

    public InventoryItem(Enums.Rarity rarity, bool equippable)
    {
        itemRarity = rarity;
        this.equippable = equippable;
    }

    public bool GetEquippable()
    {
        return equippable;
    }

    public void SetEquippable(bool equippable)
    {
        this.equippable = equippable;
    }

    public abstract string GetItemName();
    public abstract string GetDescription();
}
