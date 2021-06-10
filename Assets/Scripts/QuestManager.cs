using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField] public GameObject AcceptedQuestWindow;

    public bool isOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleQuestList();
        }
    }

    public void ToggleQuestList()
    {
        isOpen = !isOpen;
        if (FindObjectOfType<SelectedQuestWindow>())
            FindObjectOfType<SelectedQuestWindow>().gameObject.SetActive(false);
        AcceptedQuestWindow.SetActive(isOpen);
    }


}
