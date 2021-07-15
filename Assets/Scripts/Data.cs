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
    public static float Xcoordinate = -10.2f;
    public static float Ycoordinate = -1.39f;
    //public static float Xcoordinate = 140.47f;
    //public static float Ycoordinate = -18.02f;
    //public static float Xcoordinate = 552.08f;
    //public static float Ycoordinate = -20.19f;
    public static string location = "Intro";

    // Character Equipped Items
    public static List<int> equippedItems = new List<int>();

    // Character Inventory
    public static List<int> items = new List<int>();

    // Character Quests
    public static List<Quest> quests = new List<Quest>();
}
