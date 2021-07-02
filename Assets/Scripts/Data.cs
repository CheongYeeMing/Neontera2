using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static int playerId;

    // Character Stats
    public static int level = 1;
    public static float gold = 10;
    public static float currentExp;
    public static float currentHealth;
    public static float maxHealth = 100;
    

    public static float Xcoordinate;
    public static float Ycoordinate;
    public static string location = "Forest";

    // Character Equipped Items
    public static EquipmentSlot[] equipmentSlots = new EquipmentSlot[4];

    // Character Inventory
    public static List<Item> items = new List<Item>();

    // Character Quests
    public static List<Quest> quests = new List<Quest>();

    // Character Buffs
    public static List<ConsumableItem> consumableItems = new List<ConsumableItem>();

}
