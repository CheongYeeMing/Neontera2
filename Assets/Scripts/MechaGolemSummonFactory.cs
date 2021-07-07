using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaGolemSummonFactory : MonoBehaviour, BossSummonFactory
{
    protected string[] Skills = { "Heal", "Laser", "ArmShard" };

    public void Summon(string skill)
    {
        switch (skill)
        {
            case "Heal":
                gameObject.GetComponent<MechaGolemSummonerHeal>().Summon();
                break;
            case "Laser":
                gameObject.GetComponent<TurtleKingSummonerBullet2>().Summon();
                break;
            case "ArmShard":
                gameObject.GetComponent<MechaGolemSummonerArmShard>().Summon();
                break;
        }
    }

    public string[] GetSkills()
    {
        return Skills;
    }
}
