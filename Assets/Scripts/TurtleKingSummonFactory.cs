using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleKingSummonFactory : MonoBehaviour, BossSummonFactory
{
    protected string[] Skills = { "ShootBullet1", "ShootBullet2", "ShootBullet3" };

    public void Summon(string skill)
    {
        switch (skill)
        {
            case "ShootBullet1":
                gameObject.GetComponent<TurtleKingSummonerBullet1>().Summon();
                break;
            case "ShootBullet2":
                gameObject.GetComponent<TurtleKingSummonerBullet2>().Summon();
                break;
            case "ShootBullet3":
                gameObject.GetComponent<TurtleKingSummonerBullet3>().Summon();
                break;
        }
    }

    public string[] GetSkills()
    {
        return Skills;
    }
}
