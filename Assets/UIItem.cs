using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    private InventoryItem invItem;
    private InventoryUIManager manager;
    [SerializeField]
    private Button button;
    [SerializeField]
    private TextMeshProUGUI tmpText;

    public void SetInvManager(InventoryUIManager manager)
    {
        this.manager = manager;
    }

    public void SetInvItem(InventoryItem item)
    {
        invItem = item;
        UpdateUI();
    }

    private void UpdateUI()
    {
        tmpText.text = invItem.GetItemName();
        if (!invItem.GetEquippable()) button.gameObject.SetActive(false);
    }

    public void EquipItem()
    {
        if (manager) manager.EquipItem(invItem);
    }
}
