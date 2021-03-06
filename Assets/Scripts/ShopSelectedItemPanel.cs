using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSelectedItemPanel : MonoBehaviour
{
    [SerializeField] public Text ItemName;
    [SerializeField] public Image Icon;
    [SerializeField] public Text DescriptionText;
    [SerializeField] public Text StatsText;
    [SerializeField] public Text ItemPrice;
    [SerializeField] public Button BuyButton;
    [SerializeField] public GameObject InsufficientGoldWindow;

    [SerializeField] public Inventory inventory;


    public Item item;

    public void ShopSelectedEquipableItem(EquipableItem item)
    {
        this.item = item;
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
        ItemPrice.text = "Price: " + item.cost + " gold";
        BuyButton.GetComponentInChildren<Text>().text = "Buy";
        gameObject.SetActive(true);
    }

    public void ShopSelectedConsumableItem(ConsumableItem item)
    {
        this.item = item;
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
        ItemPrice.text = "Price: " + item.cost + " gold";
        BuyButton.GetComponentInChildren<Text>().text = "Buy";
        gameObject.SetActive(true);
    }

    public void BuyItem()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        if (FindObjectOfType<CharacterWallet>().HasEnoughGold(item.cost) && !inventory.IsFull())
        {
            FindObjectOfType<AudioManager>().StopEffect("BuyItem");
            FindObjectOfType<AudioManager>().PlayEffect("BuyItem");
            FindObjectOfType<CharacterWallet>().MinusGold(item.cost);
            if (item is EquipableItem)
            {
                ((EquipableItem)item).isEquipped = false;
                inventory.AddItem(item);
            }
            else
            {
                inventory.AddItem(item);
            }
            gameObject.SetActive(false);
        }
        else
        {
            FindObjectOfType<AudioManager>().StopEffect("Warning");
            FindObjectOfType<AudioManager>().PlayEffect("Warning");
            InsufficientGoldWindow.gameObject.SetActive(true);
        }
    }

    public void CloseInsufficientGoldWindow()
    {
        FindObjectOfType<AudioManager>().StopEffect("Warning");
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        InsufficientGoldWindow.gameObject.SetActive(false);
    }
}
