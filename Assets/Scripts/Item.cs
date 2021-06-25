using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))] // Adds BoxCollider2D to whatever object this script is attached to 
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, PickUp, Examine}
    public enum ItemType { Consumables, Equipment, Currency, Quest}
    [Header("Attributes")]
    public InteractionType interactType;
    public ItemType itemType;

    [Header("Examine")]
    public string examineText;
    
    [Header("Custom Events")]
    public UnityEvent customEvent; //for quests
    public UnityEvent consumeEvent;

    [SerializeField] public string ItemName;
    [SerializeField] public Sprite Icon;
    [SerializeField] public string descriptionText;
    [SerializeField] public int cost;

    // Called in editor only
    // Used for setting default values of Object
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true; // BoxCollider2D is type of Collider2D
        gameObject.layer = 10; // Set layer to item
    }

    public void Interact()
    {
        switch (interactType)
        {
            case InteractionType.PickUp:
                // Add the object to the PickedUpItems 
                FindObjectOfType<InventorySystem>().PickUp(gameObject);
                // Delete the object
                gameObject.SetActive(false);
                break;
            //case InteractionType.Examine:
            //    // Call the Examine item in the interaction system
            //    FindObjectOfType<Interactable>().ExamineItem(this);
            //    break;
            default:
                Debug.Log("NULL ITEM");
                break;
        }
        // Invoke(call) custom event
        customEvent.Invoke();
    }
}
