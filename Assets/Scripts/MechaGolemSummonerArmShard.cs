using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaGolemSummonerArmShard : BossSummoner
{
    protected const string MECHA_GOLEM_SHOOT = "Shoot";
    public override void Summon()
    {
        StartCoroutine(SummonArmShard());
    }

    private IEnumerator SummonArmShard()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 1; i < 6; i++)
        {
            yield return new WaitForSeconds(1 + Random.Range(1,10)/10);
            gameObject.GetComponent<MechaGolemAnimation>().ChangeAnimationState(MECHA_GOLEM_SHOOT);
            Instantiate(prefab, new Vector3((float)(transform.position.x + Mathf.Sign(transform.localScale.x) * 2), transform.position.y - 0.8f, transform.position.z), Quaternion.identity);
        }
        gameObject.GetComponent<MechaGolemAttack>().SummonComplete();
    }
}
