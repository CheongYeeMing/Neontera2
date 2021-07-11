using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingSummonerSlimeFloor : BossSummoner
{
    public override void Summon()
    {
        StartCoroutine(SummonSlimeFloor());
    }

    private IEnumerator SummonSlimeFloor()
    {
        yield return new WaitForSeconds(1f);
        CinemachineShake.Instance.Shaking = true;
        for (int i = 1; i < 20; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Instantiate(prefab, new Vector3(transform.position.x - i, -25.3f, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x + i, transform.position.y - 1.5f, transform.position.z), Quaternion.identity);
        }
        CinemachineShake.Instance.Shaking = false;
        gameObject.GetComponent<SlimeKingAttack>().SummonComplete();
    }
}
