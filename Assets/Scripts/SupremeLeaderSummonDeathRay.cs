using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupremeLeaderSummonDeathRay : BossSummoner
{
    protected const string SUPREME_LEADER_DEATHRAY = "DeathRay";
    public override void Summon()
    {
        StartCoroutine(SummonDeathRay());
    }

    private IEnumerator SummonDeathRay()
    {
        gameObject.GetComponent<SupremeLeaderAnimation>().ChangeAnimationState(SUPREME_LEADER_DEATHRAY);
        yield return new WaitForSeconds(0.3f);
        for (int i = 1; i < 5; i++)
        {
            Transform target = GameObject.FindGameObjectWithTag("Character").transform;
            gameObject.GetComponent<SupremeLeaderAnimation>().ChangeAnimationState(SUPREME_LEADER_DEATHRAY);
            yield return new WaitForSeconds(0.1f);
            Instantiate(prefab, new Vector3(target.position.x, target.position.y, transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }
        gameObject.GetComponent<SupremeLeaderAttack>().SummonComplete();
    }
}
