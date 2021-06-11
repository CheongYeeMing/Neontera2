using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Quest
{
    public bool isActive;
    public bool canRepeat;
    public enum Status { COMPLETED, ONGOING, WAITING}

    public Status status;
    public string title;
    public string description;

    public QuestCriteria questCriteria;

    public int expReward;
    public int goldReward;
    public Item itemReward;

    public void Start()
    {
        status = Status.WAITING;
        questCriteria.currentAmount = 0;
    }
    public void Update()
    {
        if (status == Status.WAITING)
        {
            isActive = false;
        }
        else if (status == Status.ONGOING || status == Status.COMPLETED)
        {
            isActive = true;
        }

        if (questCriteria.IsFufilled())
        {
            status = Status.COMPLETED;
        }
    }

    public void Reset()
    {
        status = Status.WAITING;
        questCriteria.currentAmount = 0;
    }
}
