using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItemPanel : MonoBehaviour
{
    [SerializeField] public Text ItemName;
    [SerializeField] public Image Icon;
    [SerializeField] public Text DescriptionText;
    [SerializeField] public Text StatsText;
    [SerializeField] public Button InteractButton;
    [SerializeField] public Button DeleteButton;
    [SerializeField] public DeleteWindow DeleteWindow;

    public Item item;
    public bool active;

    public void SelectedEquipableItem(EquipableItem item)
    {
        ItemName.text = item.ItemName;
        Icon.sprite = item.Icon;
        DescriptionText.text = item.descriptionText;
        StatsText.text = "";
        StatsText.text += "Attack: +" + item.AttackBonus.ToString() + "\n";
        StatsText.text += "Health: +" + item.HealthBonus.ToString() + "\n";
        StatsText.text += "Speed: +" + item.SpeedBonus.ToString() + "\n";
        StatsText.text += "AttackPercent: " + item.AttackPercentBonus.ToString() + "\n";
        StatsText.text += "HealthPercent: " + item.HealthPercentBonus.ToString() + "\n";
        StatsText.text += "SpeedPercent: " + item.SpeedPercentBonus.ToString();
        if (item.isEquipped)
        {
            InteractButton.GetComponentInChildren<Text>().text = "Unequip";
        } 
        else
        {
            InteractButton.GetComponentInChildren<Text>().text = "Equip";
        }
        DeleteButton.GetComponentInChildren<Text>().text = "Delete";
        DeleteWindow.item = item;
        StatsText.gameObject.SetActive(true);
        InteractButton.gameObject.SetActive(true);
        DeleteButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void SelectedConsumableItem(ConsumableItem item)
    {
        ItemName.text = item.ItemName;
        Icon.sprite = item.Icon;
        DescriptionText.text = item.descriptionText;
        StatsText.text = "";
        if (item.consumableType == ConsumableType.Instant)
        {
            StatsText.text += "Heal Amount: " + item.healAmount.ToString();
        }
        if (item.consumableType == ConsumableType.FadeOverTime)
        {
            StatsText.text += "AttackPercent: " + item.AttackPercentBonus.ToString() + "\n";
            StatsText.text += "HealthPercent: " + item.HealthPercentBonus.ToString() + "\n";
            StatsText.text += "SpeedPercent: " + item.SpeedPercentBonus.ToString() + "\n" + "\n";
            StatsText.text += "Duration: " + item.duration + " seconds";
        }
        InteractButton.GetComponentInChildren<Text>().text = "Use";
        DeleteWindow.item = item;
        StatsText.gameObject.SetActive(true);
        InteractButton.gameObject.SetActive(true);
        DeleteButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void SelectedQuestItem(Item item)
    {
        ItemName.text = item.ItemName;
        Icon.sprite = item.Icon;
        DescriptionText.text = item.descriptionText;
        StatsText.gameObject.SetActive(false);
        InteractButton.gameObject.SetActive(false);
        DeleteButton.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Interact()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        if (item is EquipableItem && ((EquipableItem)item).isEquipped)
        {
            FindObjectOfType<Character>().Unequip((EquipableItem)item);
            gameObject.SetActive(false);
        }
        else if (item is EquipableItem && !((EquipableItem)item).isEquipped)
        {
            FindObjectOfType<Character>().Equip((EquipableItem)item);
            gameObject.SetActive(false);
        }
        else if (item is ConsumableItem)
        {
            FindObjectOfType<Character>().Consume((ConsumableItem)item);
            gameObject.SetActive(false);
        }
    }

    public void ConfirmDelete()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        FindObjectOfType<AudioManager>().StopEffect("Warning");
        FindObjectOfType<AudioManager>().PlayEffect("Warning");
        DeleteWindow.gameObject.SetActive(true);
    }
}
