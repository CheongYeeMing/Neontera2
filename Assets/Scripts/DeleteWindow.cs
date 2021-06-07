using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteWindow : MonoBehaviour
{
    [SerializeField] public Button yesButton; 
    [SerializeField] public Button noButton;
    
    public void Yes()
    {

    }

    public void No()
    {
        this.gameObject.SetActive(false);
    }
}
