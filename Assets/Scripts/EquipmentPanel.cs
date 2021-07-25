using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;
    [SerializeField] List<EquipableItem> equippedItems;

    public event Action<Item> OnItemRightClickedEvent;
    public event Action<Item> OnItemLeftClickedEvent;


    private void Start()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnRightClickEvent += OnItemRightClickedEvent;
            equipmentSlots[i].OnLeftClickEvent += OnItemLeftClickedEvent;
        }
    }

    private void OnValidate()
    {
        //equipmentSlots = Data.equipmentSlots;
        //equippedItems = Data.equippedItems;
        //Debug.Log(Data.equippedItems.Count);
        if (equipmentSlotsParent != null)
        {
            equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
        }
        //RefreshUI();
    }

    //public void RefreshUI()
    //{
    //    foreach (EquipableItem item in equippedItems)
    //    {
    //        Debug.Log(item);
    //        for (int i = 0; i < equipmentSlots.Length; i++)
    //        {
    //            if (equipmentSlots[i].EquipmentType == item.EquipmentType)
    //            {
    //                Debug.Log("Added");
    //                equipmentSlots[i].Item = item;
    //            }
    //            else
    //            {
    //                Debug.Log("nothing");
    //                equipmentSlots[i].Item = null;
    //            }
    //        }
    //    }
    //}


    public bool AddItem(EquipableItem item, out EquipableItem previousItem)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].EquipmentType == item.EquipmentType)
            {
                equippedItems.Add(item);
                previousItem = (EquipableItem)equipmentSlots[i].Item;
                if (previousItem != null) equippedItems.Remove(previousItem);
                equipmentSlots[i].Item = item;
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(EquipableItem item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].Item == item)
            {
                equippedItems.Remove(item);
                equipmentSlots[i].Item = null;
                return true;
            }
        }
        return false;
    }

    public EquipmentSlot[] GetEquipmentSlots()
    {
        return equipmentSlots;
    }

    public List<EquipableItem> GetEquippedItems()
    {
        return equippedItems;
    }
}
