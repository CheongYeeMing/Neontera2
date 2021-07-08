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
            Transform target = GameObject.FindGameObjectWithTag("Character").transform;
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (target.position.x < transform.position.x)
            {
                prefab.transform.localScale = new Vector2(-prefab.transform.localScale.x, prefab.transform.localScale.y);
                angle = 180 - angle;
            }
            else
            {
                angle *= -1;
            }
            Quaternion angleAxis = Quaternion.AngleAxis(-angle, Vector3.forward);
            gameObject.GetComponent<MechaGolemAnimation>().ChangeAnimationState(MECHA_GOLEM_SHOOT);
            Instantiate(prefab, new Vector3((float)(transform.position.x + Mathf.Sign(transform.localScale.x) * 2), transform.position.y - 0.8f, transform.position.z), angleAxis);
            if (target.position.x < transform.position.x)
            {
                prefab.transform.localScale = new Vector2(-prefab.transform.localScale.x, prefab.transform.localScale.y);
            }
            yield return new WaitForSeconds(1 + Random.Range(1, 10) / 10);
        }
        gameObject.GetComponent<MechaGolemAttack>().SummonComplete();
    }
}
