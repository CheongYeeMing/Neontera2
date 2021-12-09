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
    public static float baseAttack = 10;
    public static float currentExp = 0;
    public static float currentHealth = 0;
    public static float baseHealth = 100;
    public static float Xcoordinate = -10.2f;
    public static float Ycoordinate = -1.39f;
    public static string location = "Intro";

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

    public static int Nicole = 0;
    public static int Wilson = 0;

    // GameObject Items
    public static List<int> Item = new List<int>();

    // GameObject Monologues
    public static List<int> Monologue = new List<int>();

    // GameObject Portals
    public static List<int> Portal = new List<int>();

    public static void SaveGame()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("effectsVolume", effectsVolume);

        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetFloat("gold", gold);
        PlayerPrefs.SetFloat("baseAttack", baseAttack);
        PlayerPrefs.SetFloat("currentExp", currentExp);
        PlayerPrefs.SetFloat("currentHealth", currentHealth);
        PlayerPrefs.SetFloat("baseHealth", baseHealth);
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

        // NPC
        PlayerPrefs.SetInt("JohnsonIntro", JohnsonIntro);
        PlayerPrefs.SetInt("DrAaronIntro", DrAaronIntro);
        PlayerPrefs.SetInt("JohnsonTown", JohnsonTown);
        PlayerPrefs.SetInt("DrAaronTown", DrAaronTown);
        PlayerPrefs.SetInt("JeanneTown", JeanneTown);
        PlayerPrefs.SetInt("MartinTown", MartinTown);
        PlayerPrefs.SetInt("LloydForest", LloydForest);
        PlayerPrefs.SetInt("TrinaCave", TrinaCave);

        PlayerPrefs.SetInt("Nicole", Nicole);
        PlayerPrefs.SetInt("Wilson", Wilson);

        PlayerPrefs.SetInt("gameObjectItem", Item.Count);
        for (int i = 0; i < Item.Count; i++) PlayerPrefs.SetInt("gameObjectItem" + (i + 1), Item[i]);

        PlayerPrefs.SetInt("gameObjectMonologue", Monologue.Count);
        for (int i = 0; i < Monologue.Count; i++) PlayerPrefs.SetInt("gameObjectMonologue" + (i + 1), Monologue[i]);

        PlayerPrefs.SetInt("gameObjectPortal", Portal.Count);
        for (int i = 0; i < Portal.Count; i++) PlayerPrefs.SetInt("gameObjectPortal" + (i + 1), Portal[i]);

        //PlayerPrefs.Save();
    }

    public static void LoadGame()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        effectsVolume = PlayerPrefs.GetFloat("effectsVolume");

        level = PlayerPrefs.GetInt("level");
        gold = PlayerPrefs.GetFloat("gold");
        baseAttack = PlayerPrefs.GetFloat("baseAttack");
        currentExp = PlayerPrefs.GetFloat("currentExp");
        currentHealth = PlayerPrefs.GetFloat("currentHealth");
        baseHealth = PlayerPrefs.GetFloat("baseHealth");
        Xcoordinate = PlayerPrefs.GetFloat("Xcoordinate");
        Ycoordinate = PlayerPrefs.GetFloat("Ycoordinate");

        location = PlayerPrefs.GetString("location", location);

        
        for (int i = 0; i < PlayerPrefs.GetInt("numEquippedItems"); i++) equippedItems.Add(PlayerPrefs.GetInt("equippedItem" + (i + 1)));

        for (int i = 0; i < PlayerPrefs.GetInt("numItems"); i++) items.Add(PlayerPrefs.GetInt("items" + (i + 1)));
        
        for (int i = 0; i < PlayerPrefs.GetInt("numQuests"); i++) quests.Add(PlayerPrefs.GetInt("quests" + (i + 1)));

        for (int i = 0; i < PlayerPrefs.GetInt("questProgress"); i++) questProgress.Add(PlayerPrefs.GetInt("questProgress" + (i + 1)));

        //NPC
        JohnsonIntro = PlayerPrefs.GetInt("JohnsonIntro");
        DrAaronIntro = PlayerPrefs.GetInt("DrAaronIntro");
        JohnsonTown = PlayerPrefs.GetInt("JohnsonTown");
        DrAaronTown = PlayerPrefs.GetInt("DrAaronTown");
        JeanneTown = PlayerPrefs.GetInt("JeanneTown");
        MartinTown = PlayerPrefs.GetInt("MartinTown");
        LloydForest = PlayerPrefs.GetInt("LloydForest");
        TrinaCave = PlayerPrefs.GetInt("TrinaCave");

        Nicole = PlayerPrefs.GetInt("Nicole");
        Wilson = PlayerPrefs.GetInt("Wilson");

        for (int i = 0; i < PlayerPrefs.GetInt("gameObjectItem"); i++) Item.Add(PlayerPrefs.GetInt("gameObjectItem" + (i + 1)));

        for (int i = 0; i < PlayerPrefs.GetInt("gameObjectMonologue"); i++) Monologue.Add(PlayerPrefs.GetInt("gameObjectMonologue" + (i + 1)));

        for (int i = 0; i < PlayerPrefs.GetInt("gameObjectPortal"); i++) Portal.Add(PlayerPrefs.GetInt("gameObjectPortal" + (i + 1)));
    }

    public static void NewGame()
    {
        PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();

        musicVolume = 0.5f;
        effectsVolume = 0.5f;

        // Character
        level = 1;
        gold = 10;
        baseAttack = 10;
        currentExp = 0;
        currentHealth = 0;
        baseHealth = 100;
        Xcoordinate = -10.2f;
        Ycoordinate = -1.39f;
        location = "Intro";


        // Character Equipped Items
        equippedItems = new List<int>();

        // Character Inventory
        items = new List<int>();

        // Character Quests
        quests = new List<int>();
        questProgress = new List<int>();

        // NPC Sequence Number
        JohnsonIntro = 0;
        DrAaronIntro = 0;
        JohnsonTown = 0;
        DrAaronTown = 0;
        JeanneTown = 0;
        MartinTown = 0;
        LloydForest = 0;
        TrinaCave = 0;
        Nicole = 0;
        Wilson = 0;

        // GameObject Items
        Item = new List<int>();

        // GameObject Monologues
        Monologue = new List<int>();

        // GameObject Portals
        Portal = new List<int>();
    }
}
