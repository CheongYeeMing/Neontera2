using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonManager : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private HelpManager helpManager;

    [SerializeField] Button pauseButton;
    [SerializeField] Button inventoryButton;
    [SerializeField] Button questButton;
    [SerializeField] Button helpButton;

    public void ClickPauseButton()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        pauseMenu.TogglePause();
    }

    public void ClickInventoryButton()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        inventorySystem.ToggleInventory();
    }

    public void ClickQuestButton()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        questManager.ToggleQuestList();
    }

    public void ClickHelpButton()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        helpManager.ToggleHelpMenu();
    }
}
