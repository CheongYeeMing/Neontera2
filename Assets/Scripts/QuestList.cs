using System.Collections.Generic;
using UnityEngine;
using System;


public class QuestList : MonoBehaviour
{
    [SerializeField] GameObject acceptedQuestWindow;
    [SerializeField] Transform questsParent;
    [SerializeField] QuestSlot[] questSlots;
    [SerializeField] SelectedQuestWindow selectedQuestWindow;

    [SerializeField] public List<Quest> questList;

    public event Action<Quest> OnItemLeftClickedEvent;

    public void Start()
    {
        for (int i = 0; i < questSlots.Length; i++)
        {
            questSlots[i].OnLeftClickEvent += OnItemLeftClickedEvent;
        }
    }

    private void Update()
    {
        foreach(Quest quest in questList)
        {
            if (quest.status == Quest.Status.COMPLETED)
            {
                selectedQuestWindow.QuestSelected(quest);
            }
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
        for (; i < questList.Count && i < questSlots.Length; i++)
        {
            questSlots[i].Quest = questList[i];
        }

        for (; i < questSlots.Length; i++)
        {
            questSlots[i].Quest = null;
        }

        
        acceptedQuestWindow.SetActive(false);
        acceptedQuestWindow.SetActive(true);
    }

    public bool AddQuest(Quest quest)
    {
        if (IsFull())
        {
            return false;
        }
        questList.Add(quest);
        RefreshUI();
        return true;
    }

    public bool RemoveQuest(Quest quest)
    {
        if (questList.Remove(quest))
        {
            RefreshUI();
            return true;
        }
        return false;
    }

    public bool IsFull()
    {
        bool questSlotsFull = questList.Count >= questSlots.Length;
        return questSlotsFull;
    }
}
