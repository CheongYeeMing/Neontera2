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
        else if (criteriaType == CriteriaType.Talk)
        {
            UpdateTalkCount();
        }
    }

    public void UpdateKillCount()
    {
        if (currentAmount < requiredAmount)
            currentAmount++;
    }

    public void UpdateCollectedCount(int amouunt)
    {
        currentAmount += amouunt;
    }

    public void UpdateTalkCount()
    {
        if (currentAmount < requiredAmount)
            currentAmount++;
    }

}

public enum CriteriaType
{
    Kill, 
    Collect,
    Talk
}
