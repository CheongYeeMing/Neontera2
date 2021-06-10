using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public int questNumber;

    public Character character;

    public SelectedQuestWindow selectedQuestWindow;
    public Text titleText;
    public Text descriptionText;
    public Text expTest;
    public Text goldText;
    public Text itemText;

    public void Start()
    {
        questNumber = 0; // To be replaced with saved data
    }

    public void OpenQuestWindow()
    {
        selectedQuestWindow.gameObject.SetActive(true);
        selectedQuestWindow.QuestSelected(quest);
    }

    public void CloseQuestWindow()
    {
        selectedQuestWindow.gameObject.SetActive(false);
    }
}
