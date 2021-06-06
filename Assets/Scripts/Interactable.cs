using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [Header("Detection Parameters")]
    // Detection Point
    public Transform detectionPoint;
    // Detection Redius
    private const float detectionRadius = 0.2f;
    // Detection Layer
    public LayerMask detectionLayer;
    // Cached Trigger Object
    public GameObject detectedObject;
    [Header("Examine Fields")]
    // Examine Window Object
    public GameObject examineWindow;
    public Image examineImage;
    public Text examineText;
    public bool isExamining = false;

    // Update is called once per frame
    void Update()
    {
        if (DetectObject())
        {
            if (InteractInput())
            {
                detectedObject.GetComponent<Item>().Interact();
            }
        } else
        {
            // Hide the Examine Window when walk past game object
            examineWindow.SetActive(false);
            // Disable the boolean
            isExamining = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }

    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.T);
    }

    bool DetectObject()
    {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
        if (obj == null)
        {
            detectedObject = null;
            return false;
        }
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }
    
    public void ExamineItem(Item item)
    {
        if (isExamining)
        {
            // Hide the Examine Window
            examineWindow.SetActive(false);
            // Disable the boolean
            isExamining = false;
        }
        else
        {
            // Show the item's image on the left side
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            // Write description text on the right side of image
            examineText.text = item.examineText;
            // Display an Examine Window
            examineWindow.SetActive(true);
            // Enable the boolean
            isExamining = true;
        }
    }
}
