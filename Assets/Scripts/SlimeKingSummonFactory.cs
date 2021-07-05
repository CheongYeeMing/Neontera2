using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingSummonFactory : MonoBehaviour, BossSummonFactory
{
    protected string[] Skills = { "slimeBall", "slimeRain", "slimeFloor" };

    public void Summon(string skill)
    {
        switch (skill)
        {
            case "slimeBall":
                gameObject.GetComponent<SlimeKingSummonerSlimeBall>().Summon();
                break;
            case "slimeRain":
                gameObject.GetComponent<SlimeKingSummonerSlimeRain>().Summon();
                break;
            case "slimeFloor":
                gameObject.GetComponent<SlimeKingSummonerSlimeFloor>().Summon();
                break;
        }
    }

    public string[] GetSkills()
    {
        return Skills;
    }
}
