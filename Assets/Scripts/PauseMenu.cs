using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string mainMenuScene;
    public GameObject pauseMenu;
    public bool isPaused;

    public GameObject character;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    }
    public void Save()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");

        Data.level = character.GetComponent<CharacterLevel>().GetLevel();
        Data.gold = character.GetComponent<CharacterWallet>().GetGoldAmount();
        Data.currentExp = character.GetComponent<CharacterLevel>().GetCurrentExp();
        Data.currentHealth = character.GetComponent<CharacterHealth>().GetCurrentHealth();
        Data.maxHealth = character.GetComponent<CharacterHealth>().GetMaxHealth();

        Data.Xcoordinate = character.transform.position.x;
        Data.Ycoordinate = character.transform.position.y;
        Data.location = character.GetComponent<CharacterMovement>().location;

        foreach (Item item in character.GetComponent<Character>().inventory.GetItems()) Data.items.Add(item.id);
        //Data.itemSlot = character.GetComponent<Character>().inventory.GetItemSlots();

        foreach (Item item in character.GetComponent<Character>().equipmentPanel.GetEquippedItems()) Data.equippedItems.Add(item.id);
            //Data.equippedItems = character.GetComponent<Character>().equipmentPanel.GetEquippedItems();
    }
    public void Quit()
    {
        FindObjectOfType<AudioManager>().StopEffect("Click");
        FindObjectOfType<AudioManager>().PlayEffect("Click");
        Time.timeScale = 1;
        SceneManager.LoadScene(1); // Return to Start Menu
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
