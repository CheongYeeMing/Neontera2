using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] public GameObject ShopWindow;
    [SerializeField] Shop shop;
    [SerializeField] ShopSelectedItemPanel shopSelectedItemPanel;

    public void Awake()
    {
        shop.OnItemLeftClickedEvent  += ShowInShopSelectedItem;
    }

    public void ShowInShopSelectedItem(Item item)
    {
        shopSelectedItemPanel.item = item;
        if (item.itemType == Item.ItemType.Equipment && item is EquipableItem)
        {
            shopSelectedItemPanel.ShopSelectedEquipableItem((EquipableItem)item);
        }
        if (item.itemType == Item.ItemType.Consumables)
        {
            shopSelectedItemPanel.ShopSelectedConsumableItem(item);
        }
    }
}
