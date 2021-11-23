using UnityEngine;

public class Character : MonoBehaviour
{
    private const string AUDIO_CLICK = "Click";
    private const string AUDIO_EQUIP_ITEM = "EquipItem";
    private const string AUDIO_SELECT_QUEST = "SelectQuest";
    private const float BASE_SPEED = 6;

    [SerializeField] private BuffWindow buffWindow;
    [SerializeField] public EquipmentPanel equipmentPanel;
    [SerializeField] public Inventory inventory;
    [SerializeField] public QuestList questList;
    [SerializeField] public StatPanel statPanel;
    [SerializeField] private SelectedItemPanel selectedItemPanel;
    [SerializeField] private SelectedQuestWindow selectedQuestWindow;

    public CharacterStat Attack;
    public CharacterStat Health;
    public CharacterStat Speed;

    [SerializeField] ItemList itemList;

    private void Awake()
    {
        Attack.SetBaseValue(Data.baseAttack);
        Health.SetBaseValue(Data.baseHealth);
        Speed.SetBaseValue(BASE_SPEED);
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
        foreach (int item in Data.equippedItems)
        {
            LoadEquip((EquipableItem)(itemList.GetItem(item)));
        }
    }

    public void ShowInSelectedQuestWindow(Quest quest)
    {
        FindObjectOfType<AudioManager>().StopEffect(AUDIO_SELECT_QUEST);
        FindObjectOfType<AudioManager>().PlayEffect(AUDIO_SELECT_QUEST);
        selectedQuestWindow.QuestSelected(quest);
    }

    private void ShowInSelectedItemPanel(Item item)
    {
        FindObjectOfType<AudioManager>().StopEffect(AUDIO_CLICK);
        FindObjectOfType<AudioManager>().PlayEffect(AUDIO_CLICK);
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
        FindObjectOfType<AudioManager>().PlayEffect(AUDIO_EQUIP_ITEM);
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
