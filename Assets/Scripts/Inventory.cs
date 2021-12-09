using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items;
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;
    [SerializeField] QuestList questList;

    [SerializeField] ItemList itemList;

    public event Action<Item> OnItemRightClickedEvent;
    public event Action<Item> OnItemLeftClickedEvent;

    public void Start()
    {
        foreach (int item in Data.items) AddItem(itemList.GetItem(item));
        //itemSlots = Data.itemSlot;
        //items = Data.items;
        //items = Data.items;
        RefreshUI();
        //Debug.Log(Data.items.Count);
        //foreach (Item item in Data.items) AddItem(item);
        //RefreshUI();
    }

    private void Awake()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnRightClickEvent += OnItemRightClickedEvent;
            itemSlots[i].OnLeftClickEvent += OnItemLeftClickedEvent;
        }
    }

    private void OnValidate()
    {
        
        
        if (itemsParent != null)
        {
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
        }
        //items = Data.items;
        //RefreshUI();
    }

    private void RefreshUI()
    {
        int i = 0;
        //for (; i < Data.items.Count && i < itemSlots.Length; i++)
        //{
        //    itemSlots[i].Item = Data.items[i];
        //}
        for (; i < items.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = items[i];
        }

        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }
    }

    public bool AddItem(Item item)
    {
        if (IsFull() || item == null)
        {
            return false;
        }
        //Data.items.Add(item);
        items.Add(item);
        foreach (Quest quest in questList.questList)
        {
            if (quest.questCriteria.criteriaType == CriteriaType.Collect)
            {
                if (quest.questCriteria.Target == item.ItemName)
                {
                    quest.questCriteria.UpdateCollectedCount(1);
                    quest.Update();
                }
            }
        }
        //Data.items.Add(item);
        RefreshUI();
        return true;
    }
    public bool RemoveItemHelper(Item item)
    {
        //foreach (Item i in Data.items)
        foreach (Item i in items)
        {
            if (i.ItemName == item.ItemName)
            {
                //Data.items.Remove(i);
                items.Remove(i);
                return true;
            }
        }
        return false;
    }
    public bool RemoveItem(Item item)
    {
        if (ContainsItem(item) && RemoveItemHelper(item))
        {
            RefreshUI();
            return true;
        }
        foreach (Quest quest in questList.questList)
        {
            if (quest.questCriteria.criteriaType == CriteriaType.Collect)
            {
                if (quest.questCriteria.Target == item.ItemName)
                {
                    quest.questCriteria.UpdateCollectedCount(-1);
                    quest.Update();
                }
            }
        }
        //Data.items.Remove(item);
        return false;
    }

    public bool IsFull()
    {
        //return Data.items.Count >= itemSlots.Length;
        return items.Count >= itemSlots.Length;
    }

    public bool ContainsItem(Item item)
    {
        //foreach (Item i in Data.items)
        foreach (Item i in items)
        {
            if (i.ItemName == item.ItemName)
            {
                return true;
            }
        }
        return false;
    }

    public int ItemCount(String item)
    {
        int count = 0;
        //foreach (Item i in Data.items)
        foreach (Item i in items)
        {
            if (i.ItemName == item)
            {
                count++;
            }
        }
        return count;
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public ItemSlot[] GetItemSlots()
    {
        return itemSlots;
    }
}
