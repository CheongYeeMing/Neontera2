using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteWindow : MonoBehaviour
{
    [SerializeField] public Button yesButton; 
    [SerializeField] public Button noButton;

    public Item item;
    
    public void Yes()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        if (item is EquipableItem && ((EquipableItem)item).isEquipped)
        {
            FindObjectOfType<Character>().Delete((EquipableItem)item);
        }
        else
        {
            FindObjectOfType<Inventory>().RemoveItem(item);
        }
        gameObject.SetActive(false);
        FindObjectOfType<SelectedItemPanel>().gameObject.SetActive(false);

    }

    public void No()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        this.gameObject.SetActive(false);
    }
}
