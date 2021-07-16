using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    // Audio Manager
    public static float musicVolume = 0.5f;
    public static float effectsVolume = 0.5f;

    // Character
    public static int level = 1;
    public static float gold = 10;
    public static float currentExp;
    public static float currentHealth = 0;
    public static float maxHealth;
    //public static float Xcoordinate = -10.2f;
    //public static float Ycoordinate = -1.39f;
    //public static float Xcoordinate = 140.47f;
    //public static float Ycoordinate = -18.02f;
    public static float Xcoordinate = 552.08f;
    public static float Ycoordinate = -20.19f;
    public static string location = "Cave";

    // Character Equipped Items
    public static List<int> equippedItems = new List<int>();

    // Character Inventory
    public static List<int> items = new List<int>();

    // Character Quests
    public static List<int> quests = new List<int>();
    public static List<int> questProgress = new List<int>();

    // NPC Sequence Number
    public static int JohnsonIntro = 0;
    public static int DrAaronIntro = 0;
    public static int JohnsonTown = 0;
    public static int DrAaronTown = 0;
    public static int JeanneTown = 0;
    public static int MartinTown = 0;
    public static int LloydForest = 0;
    public static int TrinaCave = 0;

    public static void SaveGame()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("effectsVolume", effectsVolume);

        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetFloat("gold", gold);
        PlayerPrefs.SetFloat("currentExp", currentExp);
        PlayerPrefs.SetFloat("currentHealth", currentHealth);
        PlayerPrefs.SetFloat("maxHealth", maxHealth);
        PlayerPrefs.SetFloat("Xcoordinate", Xcoordinate);
        PlayerPrefs.SetFloat("Ycoordinate", Ycoordinate);

        PlayerPrefs.SetString("location", location);

        PlayerPrefs.SetInt("numEquippedItems", equippedItems.Count);
        for (int i = 0; i < equippedItems.Count; i++) PlayerPrefs.SetInt("equippedItem" + (i + 1), equippedItems[i]);

        PlayerPrefs.SetInt("numItems", items.Count);
        for (int i = 0; i < items.Count; i++) PlayerPrefs.SetInt("items" + (i + 1), items[i]);

        PlayerPrefs.SetInt("numQuests", quests.Count);
        for (int i = 0; i < quests.Count; i++) PlayerPrefs.SetInt("quests" + (i + 1), quests[i]);

        PlayerPrefs.SetInt("questProgress", questProgress.Count);
        for (int i = 0; i < questProgress.Count; i++) PlayerPrefs.SetInt("questProgress" + (i + 1), questProgress[i]);

        PlayerPrefs.Save();
    }

    public static void LoadGame()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        effectsVolume = PlayerPrefs.GetFloat("effectsVolume");

        level = PlayerPrefs.GetInt("level");
        gold = PlayerPrefs.GetFloat("gold");
        currentExp = PlayerPrefs.GetFloat("currentExp");
        currentHealth = PlayerPrefs.GetFloat("currentHealth");
        maxHealth = PlayerPrefs.GetFloat("maxHealth");
        Xcoordinate = PlayerPrefs.GetFloat("Xcoordinate");
        Ycoordinate = PlayerPrefs.GetFloat("Ycoordinate");

        location = PlayerPrefs.GetString("location", location);

        
        for (int i = 0; i < PlayerPrefs.GetInt("numEquippedItems"); i++) equippedItems.Add(PlayerPrefs.GetInt("equippedItem" + (i + 1)));

        for (int i = 0; i < PlayerPrefs.GetInt("numItems"); i++) items.Add(PlayerPrefs.GetInt("items" + (i + 1)));
        
        for (int i = 0; i < PlayerPrefs.GetInt("numQuests"); i++) quests.Add(PlayerPrefs.GetInt("quests" + (i + 1)));

        for (int i = 0; i < PlayerPrefs.GetInt("questProgress"); i++) questProgress.Add(PlayerPrefs.GetInt("questProgress" + (i + 1)));
    }

    public static void NewGame()
    {
        PlayerPrefs.DeleteAll();
    }
}
