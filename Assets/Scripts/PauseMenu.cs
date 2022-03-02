using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] NPCManager npcManager;
    [SerializeField] ItemManager itemManager;
    [SerializeField] MonologueManager monologueManager;
    [SerializeField] PortalManager portalManager;

    public GameObject character;
    public GameObject pauseMenu;

    public bool isPaused;
    public string mainMenuScene;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void TogglePause()
    {
        FindObjectOfType<AudioManager>().StopEffect("Open");
        FindObjectOfType<AudioManager>().PlayEffect("Open");
        if (isPaused)
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Save()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");

        // Character Stats
        Data.level = character.GetComponent<CharacterLevel>().GetLevel();
        Data.gold = character.GetComponent<CharacterWallet>().GetGoldAmount();
        Data.baseAttack = character.GetComponent<CharacterAttack>().GetBaseAttack();
        Data.currentExp = character.GetComponent<CharacterLevel>().GetCurrentExp();
        Data.currentHealth = character.GetComponent<CharacterHealth>().GetCurrentHealth();
        Data.baseHealth = character.GetComponent<CharacterHealth>().GetBaseHealth();

        // Character Position
        Data.Xcoordinate = character.transform.position.x;
        Data.Ycoordinate = character.transform.position.y;
        Data.location = character.GetComponent<CharacterMovement>().GetLocation();

        // Save Inventory
        Data.items.Clear();
        foreach (Item item in character.GetComponent<Character>().inventory.GetItems()) Data.items.Add(item.id);

        // Save Equipped Items
        Data.equippedItems.Clear();
        foreach (Item item in character.GetComponent<Character>().equipmentPanel.GetEquippedItems()) Data.equippedItems.Add(item.id);

        // Save Quest 
        Data.quests.Clear();
        Data.questProgress.Clear();
        foreach (Quest quest in character.GetComponent<Character>().questList.questList)
        {
            Data.quests.Add(quest.ID);
            Data.questProgress.Add(quest.questCriteria.currentAmount);
        }

        // Save NPC Sequence Number
        npcManager.Save();

        // Save Active State of Item GameObjects
        itemManager.Save();

        // Save Active State of Monologue GameObjects
        monologueManager.Save();

        // Save Active State of Portal GameObjects
        portalManager.Save();

        Data.SaveGame();
    }
    public void Quit()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        Time.timeScale = 1;
        SceneManager.LoadScene(0); // Return to Start Menu
    }

    public void Resume()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        FindObjectOfType<AudioManager>().StopEffect("Open");
        FindObjectOfType<AudioManager>().PlayEffect("Open");
        if (isPaused)
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

    }
}
