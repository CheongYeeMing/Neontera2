using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterStat Attack;
    public CharacterStat Health;
    public CharacterStat Speed;

    [SerializeField] public Inventory inventory;
    [SerializeField] public EquipmentPanel equipmentPanel;
    [SerializeField] public StatPanel statPanel;
    [SerializeField] public SelectedItemPanel selectedItemPanel;
    [SerializeField] public QuestList questList;
    [SerializeField] public SelectedQuestWindow selectedQuestWindow;

    private void Awake()
    {
        statPanel.SetStats(Attack, Health, Speed);
        statPanel.UpdateStatValues();
        inventory.OnItemRightClickedEvent += EquipFromInventory;
        equipmentPanel.OnItemRightClickedEvent += UnequipFromEquipPanel;
        inventory.OnItemLeftClickedEvent += ShowInSelectedItemPanel;
        equipmentPanel.OnItemLeftClickedEvent += ShowInSelectedItemPanel;
        questList.OnItemLeftClickedEvent += ShowInSelectedQuestWindow;
    }

    public void ShowInSelectedQuestWindow(Quest quest)
    {
        selectedQuestWindow.gameObject.SetActive(true);
        selectedQuestWindow.QuestSelected(quest);
    }

    private void ShowInSelectedItemPanel(Item item)
    {
        selectedItemPanel.item = item;
        if (item.itemType == Item.ItemType.Equipment && item is EquipableItem)
        {
            selectedItemPanel.SelectedEquipableItem((EquipableItem)item);
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
        selectedItemPanel.gameObject.SetActive(false);
        if (item is EquipableItem)
        {
            Equip((EquipableItem)item);
        }
    }

    private void UnequipFromEquipPanel(Item item)
    {
        selectedItemPanel.gameObject.SetActive(false);
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

    public void Delete(EquipableItem item)
    {
        equipmentPanel.RemoveItem(item);
        item.Unequip(this);
        statPanel.UpdateStatValues();
    }
}
