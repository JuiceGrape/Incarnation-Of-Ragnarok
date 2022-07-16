using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject invUIItemPrefab;
    public GameObject content;
    public Inventory inventory;



    public void EquipItem(InventoryItem item)
    {
        if (inventory) inventory.EquipItem(item);
    }

    public void AddItemsToUI(List<InventoryItem> items)
    {
        foreach (InventoryItem item in items)
        {
            GameObject obj = Instantiate(invUIItemPrefab);
            obj.transform.SetParent(content.transform);
            UIItem uiItem = obj.GetComponent<UIItem>();
            uiItem.SetInvManager(this);
            uiItem.SetInvItem(item);
        }
    }
}
