using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Buff : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemEffect;
    [SerializeField] TextMeshProUGUI durationLeft;

    public ConsumableItem Item;
    public float duration;

    public void Update()
    {
        if (Item != null)
        {
            durationLeft.text = "Duration Left: " + Mathf.Round(duration) + "secs";
            if (Item.isConsumed && Item.consumableType == ConsumableType.FadeOverTime)
            {
                duration -= Time.deltaTime;
                if (duration <= 0)
                {
                    duration = 0;
                    FindObjectOfType<Character>().ConsumeEffectFaded(Item);
                }
            }
        }
    }
    public void SetItem(ConsumableItem item)
    {
        image.enabled = true;
        itemName.enabled = true;
        itemEffect.enabled = true;
        durationLeft.enabled = true;
        gameObject.GetComponentInParent<Image>().enabled = true;
        Item = item;
        image.sprite = Item.Icon;
        itemName.text = Item.ItemName;
        itemEffect.text = "";
        if (Item.AttackPercentBonus != 0)
        {
            itemEffect.text += "Atk +" + Item.AttackPercentBonus + "% ";
        }
        if (Item.HealthPercentBonus != 0)
        {
            itemEffect.text += "Hp +" + Item.HealthPercentBonus + "% ";
        }
        if (Item.SpeedPercentBonus != 0)
        {
            itemEffect.text += "Spd +" + Item.SpeedPercentBonus + "% ";
        }
        duration = Item.duration;
        durationLeft.text = "Duration Left: " + duration + "secs";
    }

    public void NoItem()
    {
        image.enabled = false;
        itemName.enabled = false;
        itemEffect.enabled = false;
        durationLeft.enabled = false;
        gameObject.GetComponentInParent<Image>().enabled = false;
    }
}
