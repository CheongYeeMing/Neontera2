using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NPC file", menuName = "NPC Files Archive")]
public class NPC : ScriptableObject
{
    public string npcName;
    public Sprite icon;
    public List<Sequence> Sequences;
    public int sequenceNumber;

    [System.Serializable]
    public class Sequence
    {
        public List<Item> Items;
        public Quest Quest;
        public bool hasShop;
        public bool hasQuest;
        public bool isStory;
        public bool waitingQuest;
        public bool justDialogue;
        [TextArea(3, 15)]
        public string[] dialogue;
        [TextArea(3, 15)]
        public string[] characterDialogue;
    }
}
