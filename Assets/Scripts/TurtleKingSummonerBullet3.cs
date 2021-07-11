using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleKingSummonerBullet3 : BossSummoner
{
    protected const string ATTACK_BULLET_3 = "Attack3";
    public override void Summon()
    {
        StartCoroutine(SummonBullet3());
    }

    private IEnumerator SummonBullet3()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 1; i < 9; i++)
        {
            yield return new WaitForSeconds(0.5f);
            FindObjectOfType<AudioManager>().StopEffect("TurtleBullet");
            FindObjectOfType<AudioManager>().PlayEffect("TurtleBullet");
            gameObject.GetComponent<TurtleKingAnimation>().ChangeAnimationState(ATTACK_BULLET_3);
            Instantiate(prefab, new Vector3((float)(transform.position.x + Mathf.Sign(transform.localScale.x) * 0.5f), transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
        }
        gameObject.GetComponent<TurtleKingAttack>().SummonComplete();
    }
}
