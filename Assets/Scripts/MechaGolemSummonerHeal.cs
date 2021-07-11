using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaGolemSummonerHeal : BossSummoner
{
    protected const string MECHA_GOLEM_HEAL = "Heal";
    public override void Summon()
    {
        StartCoroutine(SummonHeal());
    }

    private IEnumerator SummonHeal()
    {
        gameObject.GetComponent<MechaGolemAnimation>().ChangeAnimationState(MECHA_GOLEM_HEAL);
        yield return new WaitForSeconds(1.35f);
        for (int i = 1; i < 4; i++)
        {
            GetComponent<MechaGolemHealth>().Heal();
            FindObjectOfType<AudioManager>().StopEffect("MechaGolemHeal");
            FindObjectOfType<AudioManager>().PlayEffect("MechaGolemHeal");
            yield return new WaitForSeconds(1.9f);
        }
        gameObject.GetComponent<MechaGolemAttack>().SummonComplete();
    }
}
