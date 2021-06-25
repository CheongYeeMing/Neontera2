using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items;
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;
    [SerializeField] QuestList questList;

    public event Action<Item> OnItemRightClickedEvent;
    public event Action<Item> OnItemLeftClickedEvent;

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
        RefreshUI();
    }

    private void RefreshUI()
    {
        int i = 0;
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
        items.Add(item);
        RefreshUI();
        foreach (Quest quest in questList.quests)
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
        return true;
    }

    public bool RemoveItem(Item item)
    {
        Debug.Log(items.Contains(item));
        if (items.Contains(item) && items.Remove(item))
        {
            RefreshUI();
            return true;
        }
        foreach (Quest quest in questList.quests)
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
        return false;
    }

    public bool IsFull()
    {
        return items.Count >= itemSlots.Length;
    }

    public bool ContainsItem(Item item)
    {
        foreach (Item i in items)
        {
            if (i.ItemName == item.ItemName)
            {
                return true;
            }
        }
        return false;
    }
}
