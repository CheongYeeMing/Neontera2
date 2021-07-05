using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingSummonerSlimeBall : BossSummoner
{
    public override void Summon()
    {
        StartCoroutine(SummonSlimeBalls());
    }

    private IEnumerator SummonSlimeBalls()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(prefab, new Vector3((float)(transform.position.x + Mathf.Sign(transform.localScale.x) * 1), transform.position.y - 1, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        Instantiate(prefab, new Vector3((float)(transform.position.x + Mathf.Sign(transform.localScale.x) * 1), transform.position.y - 1, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        Instantiate(prefab, new Vector3((float)(transform.position.x + Mathf.Sign(transform.localScale.x) * 1), transform.position.y - 1, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<SlimeKingAttack>().SummonComplete();
    }
}
