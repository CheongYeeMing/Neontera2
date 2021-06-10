using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NPC file", menuName = "NPC Files Archive")]
public class NPC : ScriptableObject
{
    public string npcName;
    public Sprite icon;
    public List<Item> items;
    public List<Quest> quests;
    [TextArea(3, 15)]
    public string[] dialogue;
    [TextArea(3, 15)]
    public string[] characterDialogue;
}
