using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestCriteria
{
    public CriteriaType criteriaType;

    public string action;
    public string Target;
    
    public int requiredAmount;
    public int currentAmount;



    public bool IsFufilled()
    {
        return currentAmount >= requiredAmount;
    }

    public void Execute()
    {
        if (criteriaType == CriteriaType.Kill)
        {
            UpdateKillCount();
        }
        else if (criteriaType == CriteriaType.Collect)
        {
            UpdateCollectedCount();
        }
        else if (criteriaType == CriteriaType.Talk)
        {
            UpdateTalkCount();
        }
    }

    public void UpdateKillCount()
    {
        currentAmount++;
    }

    public void UpdateCollectedCount()
    {
        currentAmount++;
    }

    public void UpdateTalkCount()
    {
        currentAmount++;
    }

}

public enum CriteriaType
{
    Kill, 
    Collect,
    Talk
}
