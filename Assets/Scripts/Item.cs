using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))] // Adds BoxCollider2D to whatever object this script is attached to 
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, PickUp, Examine}
    public InteractionType type;
    [Header("Examine")]
    public string descriptionText;
    public UnityEvent customEvent;

    // Called in editor only
    // Used for setting default values of Object
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true; // BoxCollider2D is type of Collider2D
        gameObject.layer = 10; // Set layer to item
    }

    public void Interact()
    {
        switch (type)
        {
            case InteractionType.PickUp:
                // Add the object to the PickedUpItems 
                FindObjectOfType<Interactable>().PickUpItem(gameObject);
                // Delete the object
                gameObject.SetActive(false);
                break;
            case InteractionType.Examine:
                // Call the Examine item in the interaction system
                FindObjectOfType<Interactable>().ExamineItem(this);
                break;
            default:
                Debug.Log("NULL ITEM");
                break;
        }
        // Invoke(call) custom event
        customEvent.Invoke();
    }
}
