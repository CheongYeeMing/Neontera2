using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField] public GameObject AcceptedQuestWindow;

    private bool isOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleQuestList();
        }
    }

    public void ToggleQuestList()
    {
        FindObjectOfType<AudioManager>().StopEffect("Open");
        FindObjectOfType<AudioManager>().PlayEffect("Open");
        isOpen = !isOpen;
        if (FindObjectOfType<SelectedQuestWindow>())
            FindObjectOfType<SelectedQuestWindow>().gameObject.SetActive(false);
        AcceptedQuestWindow.SetActive(isOpen);
    }

    public void Close()
    {
        ToggleQuestList();
    }
}
