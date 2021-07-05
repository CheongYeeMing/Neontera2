using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


public class QuestList : MonoBehaviour
{
    [SerializeField] public List<Quest> quests;
    [SerializeField] Transform questsParent;
    [SerializeField] QuestSlot[] questSlots;

    public event Action<Quest> OnItemLeftClickedEvent;

    public void Start()
    {
        quests = Data.quests;
    }
    private void Awake()
    {
        for (int i = 0; i < questSlots.Length; i++)
        {
            questSlots[i].OnLeftClickEvent += OnItemLeftClickedEvent;
        }
    }

    private void OnValidate()
    {
        if (questsParent != null)
        {
            questSlots = questsParent.GetComponentsInChildren<QuestSlot>();
        }
        RefreshUI();
    }

    private void RefreshUI()
    {
        int i = 0;
        for (; i < quests.Count && i < questSlots.Length; i++)
        {
            questSlots[i].Quest = quests[i];
        }

        for (; i < questSlots.Length; i++)
        {
            questSlots[i].Quest = null;
        }
    }

    public bool AddQuest(Quest quest)
    {
        if (IsFull())
        {
            return false;
        }
        quests.Add(quest);
        RefreshUI();
        Data.quests = quests;
        return true;
    }

    public bool RemoveQuest(Quest quest)
    {
        if (quests.Remove(quest))
        {
            RefreshUI();
            return true;
        }
        Data.quests = quests;
        return false;
    }

    public bool IsFull()
    {
        return quests.Count >= questSlots.Length;
    }
}
