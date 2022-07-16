using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemBases ItemBases;
    public Player Player;
    public InventoryUIManager uiManager;

    [SerializeField]
    private List<InventoryItem> items = new List<InventoryItem>();

    // Start is called before the first frame update
    void Start()
    {
        // debug purposes
        if (ItemBases.WeaponBases.Count > 0)
        {
            InventoryWeapon itemWeapon = new InventoryWeapon(ItemBases.WeaponBases[0], Enums.Rarity.Fixed, true);
            items.Add(itemWeapon);
            Player.EquipItem(itemWeapon);
        }
        for (int i = 0; i < 10; i++)
        {
            items.Add(CreateRandomWeapon());
        }

        uiManager.AddItemsToUI(items);
    }

    private InventoryWeapon CreateRandomWeapon()
    {
        return new InventoryWeapon(ItemBases.WeaponBases[Random.Range(0, ItemBases.WeaponBases.Count)], (Enums.Rarity)Random.Range(0, 4), true);
    }

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
    }

    public void EquipItem(InventoryItem item)
    {
        if (item.GetEquippable())
        {
            Player.EquipItem(item);
        }
    }

    //[Button("Debug Equip")]
    //public void EquipWeapon(InventoryWeapon weapon)
    //{
    //    if (Player != null)
    //        Player.EquipItem(weapon);
    //}
}
