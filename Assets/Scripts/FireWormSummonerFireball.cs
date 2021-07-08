using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWormSummonerFireball : MobSummoner
{
    protected const string FIREBALL = "Attack";
    public override void Summon()
    {
        StartCoroutine(SummonFireball());
    }

    private IEnumerator SummonFireball()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 1; i < 4; i++)
        {
            gameObject.GetComponent<MobAnimation>().ChangeAnimationState(FIREBALL);
            Instantiate(prefab, new Vector3((float)(transform.position.x + Mathf.Sign(transform.localScale.x) * 2), transform.position.y, transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
        gameObject.GetComponent<FireWormAttack>().SummonComplete();
    }
}
