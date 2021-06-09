using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop : MonoBehaviour
{
    [SerializeField] List<Item> items;
    [SerializeField] Transform itemsParent;
    [SerializeField] ShopItemSlot[] shopItemSlots;

    public event Action<Item> OnItemLeftClickedEvent;

    private void Awake()
    {
        for (int i = 0; i < shopItemSlots.Length; i++)
        {
            shopItemSlots[i].OnLeftClickEvent += OnItemLeftClickedEvent;
        }
    }

    private void OnValidate()
    {
        if (itemsParent != null)
        {
            shopItemSlots = itemsParent.GetComponentsInChildren<ShopItemSlot>();
        }
        RefreshUI();
    }

    private void RefreshUI()
    {
        int i = 0;
        for (; i < items.Count && i < shopItemSlots.Length; i++)
        {
            shopItemSlots[i].Item = items[i];
        }

        for (; i < shopItemSlots.Length; i++)
        {
            shopItemSlots[i].Item = null;
        }
    }

    public void SetItems(List<Item> newItems)
    {
        items = newItems;
        RefreshUI();
    }
}
