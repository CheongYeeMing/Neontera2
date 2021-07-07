using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleKingSummonerBullet1 : BossSummoner
{
    protected const string ATTACK_BULLET_1 = "Attack4";
    public override void Summon()
    {
        StartCoroutine(SummonBullet1());
    }

    private IEnumerator SummonBullet1()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 1; i < 5; i++)
        {
            yield return new WaitForSeconds(Random.Range(5,20)/10);
            gameObject.GetComponent<TurtleKingAnimation>().ChangeAnimationState(ATTACK_BULLET_1);
            Instantiate(prefab, new Vector3((float)(transform.position.x + Mathf.Sign(transform.localScale.x) * 2), transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
        }
        gameObject.GetComponent<TurtleKingAttack>().SummonComplete();
    }
}
