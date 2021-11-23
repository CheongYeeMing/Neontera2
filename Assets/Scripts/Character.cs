using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public Inventory inventory;
    [SerializeField] public EquipmentPanel equipmentPanel;
    [SerializeField] public StatPanel statPanel;
    [SerializeField] public SelectedItemPanel selectedItemPanel;
    [SerializeField] public QuestList questList;
    [SerializeField] public SelectedQuestWindow selectedQuestWindow;
    [SerializeField] public BuffWindow buffWindow;

    public CharacterStat Attack;
    public CharacterStat Health;
    public CharacterStat Speed;

    [SerializeField] ItemList itemList;

    private void Awake()
    {
        Attack.SetBaseValue(Data.baseAttack);
        Health.SetBaseValue(Data.baseHealth);
        Speed.SetBaseValue(6);
        statPanel.SetStats(Attack, Health, Speed);
        statPanel.UpdateStatValues();
        inventory.OnItemRightClickedEvent += EquipFromInventory;
        inventory.OnItemLeftClickedEvent += ShowInSelectedItemPanel;
        equipmentPanel.OnItemRightClickedEvent += UnequipFromEquipPanel;
        equipmentPanel.OnItemLeftClickedEvent += ShowInSelectedItemPanel;
        questList.OnItemLeftClickedEvent += ShowInSelectedQuestWindow;
    }

    public void Start()
    {
        foreach (int item in Data.equippedItems) LoadEquip((EquipableItem)(itemList.GetItem(item)));
    }

    public void ShowInSelectedQuestWindow(Quest quest)
    {
        Debug.Log("Here???");
        FindObjectOfType<AudioManager>().StopEffect("SelectQuest");
        FindObjectOfType<AudioManager>().PlayEffect("SelectQuest");
        selectedQuestWindow.QuestSelected(quest);
    }

    private void ShowInSelectedItemPanel(Item item)
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        selectedItemPanel.item = item;
        if (item.itemType == Item.ItemType.Equipment && item is EquipableItem)
        {
            selectedItemPanel.SelectedEquipableItem((EquipableItem)item);
        }
        if (item.itemType == Item.ItemType.Consumables)
        {
            selectedItemPanel.SelectedConsumableItem((ConsumableItem)item);
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

    public void LoadEquip(EquipableItem item)
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
    }

    public void Equip(EquipableItem item)
    {
        FindObjectOfType<AudioManager>().PlayEffect("EquipItem");
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

    public void Consume(ConsumableItem item)
    {
        if (!buffWindow.IsFull() && inventory.RemoveItem(item))
        {
            
            item.Consume(this);
            if (item.consumableType == ConsumableType.FadeOverTime)
            {
                buffWindow.AddItem(item);
            }
            statPanel.UpdateStatValues();
        }
    }

    public void ConsumeEffectFaded(ConsumableItem item)
    {
        item.Debuff(this);
        buffWindow.RemoveItem(item);
        statPanel.UpdateStatValues();
    }

    public CharacterStat GetAttack()
    {
        return Attack;
    }

    public CharacterStat GetHealth()
    {
        return Health;
    }

    public CharacterStat GetSpeed()
    {
        return Speed;
    }
}
