using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Armor", fileName = "New Armor")]
public class ItemEquipment : ItemBaseObject
{
    [SerializeField]
    private Enums.SlotType slotType;

    public Enums.SlotType GetSlotType()
    {
        return slotType;
    }
    public void SetSlotType(Enums.SlotType slotType)
    {
        this.slotType = slotType;
    }

    private void OnValidate()
    {
        if (slotType.Equals(Enums.SlotType.Weapon))
        {
            slotType = Enums.SlotType.OffHand;
            Debug.LogWarning("Equipment can not have SlotType Weapon");
        }
    }
}
