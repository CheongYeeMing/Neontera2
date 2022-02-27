using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [Header("General Fields")]
    public bool isOpen;

    // Inventory System Window
    public GameObject ui_Window;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isOpen || !isOpen && CanOpen())
                ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        FindObjectOfType<AudioManager>().StopEffect("Open");
        FindObjectOfType<AudioManager>().PlayEffect("Open");
        isOpen = !isOpen;
        if (FindObjectOfType<SelectedItemPanel>())
            FindObjectOfType<SelectedItemPanel>().gameObject.SetActive(false);
        ui_Window.SetActive(isOpen);
    }

    // Add item to items list
    public void PickUp(GameObject item)
    {
        Item item2 = item.GetComponent<Item>();
        if (item2.itemType == Item.ItemType.Currency)
        {
            FindObjectOfType<CharacterWallet>().AddGold(1);
        }
        else
        {
            inventory.AddItem(item2);
        }
    }

    public bool CanOpen()
    {
        bool can = true;
        Monologue[] monologues = FindObjectsOfType<Monologue>();
        foreach (Monologue mono in monologues)
        {
            if (mono.IsExamining())
            {
                can = false;
                break;
            }
        }
        DialogueManager[] npc = FindObjectsOfType<DialogueManager>();
        for (int i = 0; i < npc.Length; i++)
        {
            if (npc[i].isTalking)
            {
                can = false;
                break;
            }
        }
        return can;
    }
}
