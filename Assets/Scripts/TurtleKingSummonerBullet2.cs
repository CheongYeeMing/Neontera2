using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleKingSummonerBullet2 : BossSummoner
{
    protected const string ATTACK_BULLET_2 = "Attack2";
    public override void Summon()
    {
        StartCoroutine(SummonBullet1());
    }

    private IEnumerator SummonBullet1()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 1; i < 4; i++)
        {
            yield return new WaitForSeconds(0.5f);
            for (int j = 1; j < 5; j++)
            {
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponent<TurtleKingAnimation>().ChangeAnimationState(ATTACK_BULLET_2);
                Instantiate(prefab, new Vector3((float)(transform.position.x + Mathf.Sign(transform.localScale.x) * 2), transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
            }
        }
        gameObject.GetComponent<TurtleKingAttack>().SummonComplete();
    }
}
