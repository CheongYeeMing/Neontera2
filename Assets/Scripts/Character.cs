using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterStat Attack;
    public CharacterStat Health;
    public CharacterStat Speed;

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] StatPanel statPanel;
    [SerializeField] SelectedItemPanel selectedItemPanel;

    private void Awake()
    {
        statPanel.SetStats(Attack, Health, Speed);
        statPanel.UpdateStatValues();
        inventory.OnItemRightClickedEvent += EquipFromInventory;
        equipmentPanel.OnItemRightClickedEvent += UnequipFromEquipPanel;
        inventory.OnItemLeftClickedEvent += ShowInSelectedItemPanel;
        equipmentPanel.OnItemLeftClickedEvent += ShowInSelectedItemPanel;
    }

    private void ShowInSelectedItemPanel(Item item)
    {
        if (item.itemType == Item.ItemType.Equipment)
        {
            selectedItemPanel.SelectedEquipableItem(item);
        }
        if (item.itemType == Item.ItemType.Consumables)
        {
            selectedItemPanel.SelectedConsumableItem(item);
        }
        if (item.itemType == Item.ItemType.Quest)
        {
            selectedItemPanel.SelectedQuestItem(item);
        }
    }

    private void EquipFromInventory(Item item)
    {
        if (item is EquipableItem)
        {
            Equip((EquipableItem)item);
        }
    }

    private void UnequipFromEquipPanel(Item item)
    {
        if (item is EquipableItem)
        {
            Unequip((EquipableItem)item);
        }
    }

    public void Equip(EquipableItem item)
    {
        if (inventory.RemoveItem(item))
        {
            EquipableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(this);
                    statPanel.UpdateStatValues();
                }
                item.Equip(this);
                statPanel.UpdateStatValues();
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquipableItem item)
    {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item)){
            item.Unequip(this);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
        }
    }
}
