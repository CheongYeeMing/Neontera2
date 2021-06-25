using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWindow : MonoBehaviour
{
    [SerializeField] public List<ConsumableItem> consumableItems;
    [SerializeField] Transform buffParent;
    [SerializeField] public Buff[] buffSlots;

    private void OnValidate()
    {
        if (buffParent != null)
        {
            buffSlots = buffParent.GetComponentsInChildren<Buff>();
        }
        RefreshUI();
    }

    private void RefreshUI()
    {
        int i = 0;
        for (; i < consumableItems.Count && i < buffSlots.Length; i++)
        {
            buffSlots[i].SetItem(consumableItems[i]);
        }

        for (; i < buffSlots.Length; i++)
        {
            buffSlots[i].NoItem();
        }
    }

    public bool AddItem(ConsumableItem item)
    {
        if (IsFull() || item == null)
        {
            return false;
        }
        consumableItems.Add(item);
        RefreshUI();
        return true;
    }

    public bool RemoveItem(ConsumableItem item)
    {
        if (consumableItems.Contains(item) && consumableItems.Remove(item))
        {
            RefreshUI();
            return true;
        }
        return false;
    }

    public bool IsFull()
    {
        return consumableItems.Count >= buffSlots.Length;
    }
}
