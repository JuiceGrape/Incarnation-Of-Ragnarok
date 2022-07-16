using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBaseObject : ScriptableObject
{
    [Header("Generic Attributes")]
    [SerializeField]
    private Enums.ItemType itemType;
    [SerializeField]
    private string itemName;
    [SerializeField]
    [TextArea(3, 10)]
    private string itemDescription;
    [SerializeField]
    private Sprite itemSprite;

    public Enums.ItemType GetItemType()
    {
        return itemType;
    }

    public void SetItemType(Enums.ItemType itemType)
    {
        this.itemType = itemType;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public void SetItemName(string itemName)
    {
        this.itemName = itemName;
    }

    public string GetItemDescription()
    {
        return itemDescription;
    }

    public void SetItemDescription(string itemDescription)
    {
        this.itemDescription = itemDescription;
    }

    public Sprite GetItemSprite()
    {
        return itemSprite;
    }

    public void SetItemSprite(Sprite sprite)
    {
        itemSprite = sprite;
    }
}
