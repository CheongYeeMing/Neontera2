using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupremeLeaderSummonFactory : MonoBehaviour, BossSummonFactory
{
    protected string[] Skills = { "Laser", "DeathRay" };//, "LastSkill" };

    public void Summon(string skill)
    {
        switch (skill)
        {
            case "Laser":
                gameObject.GetComponent<SupremeLeaderSummonLaser>().Summon();
                break;
            case "DeathRay":
                gameObject.GetComponent<SupremeLeaderSummonDeathRay>().Summon();
                break;
            //case "ArmShard":
            //    gameObject.GetComponent<SupremeLeaderSummonLaser>().Summon();
            //    break;
        }
    }

    public string[] GetSkills()
    {
        return Skills;
    }
}
