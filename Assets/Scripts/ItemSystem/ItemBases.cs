using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ItemBases : MonoBehaviour
{
    public List<ItemEquipment> EquipmentBases = new List<ItemEquipment>();
    public List<ItemWeapon> WeaponBases = new List<ItemWeapon>();

    [ContextMenu("Retrieve missing SO's")]
    private void GetMissingSOs()
    {
        if (EditorApplication.isPlaying) return;
        foreach (ItemEquipment itemEquipment in GetAllInstances<ItemEquipment>())
        {
            if(!EquipmentBases.Contains(itemEquipment))
            {
                EquipmentBases.Add(itemEquipment);
            }
        }

        foreach (ItemWeapon itemWeapon in GetAllInstances<ItemWeapon>())
        {
            if (!WeaponBases.Contains(itemWeapon))
            {
                WeaponBases.Add(itemWeapon);
            }
        }
    }



    private static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;
    }
}
