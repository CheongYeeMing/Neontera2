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
        this.gameObject.SetActive(false);
    }
}
