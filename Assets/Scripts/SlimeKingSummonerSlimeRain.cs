using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingSummonerSlimeRain : BossSummoner
{
    public override void Summon()
    {
        StartCoroutine(SummonSlimeRain());
    }

    private IEnumerator SummonSlimeRain()
    {
        yield return new WaitForSeconds(1f);
        SummonRainWave();
        yield return new WaitForSeconds(0.5f);
        SummonRainWave();
        yield return new WaitForSeconds(0.5f);
        SummonRainWave();
        yield return new WaitForSeconds(0.5f);
        SummonRainWave();
        yield return new WaitForSeconds(0.5f);
        SummonRainWave();
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<SlimeKingAttack>().SummonComplete();
    }

    private void SummonRainWave()
    {
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            Instantiate(prefab, new Vector3(transform.position.x - 12, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x - 10, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x - 8, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x - 6, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x - 4, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x - 2, transform.position.y + 5, transform.position.z), Quaternion.identity);

            Instantiate(prefab, new Vector3(transform.position.x + 2, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x + 4, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x + 6, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x + 8, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x + 10, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x + 12, transform.position.y + 5, transform.position.z), Quaternion.identity);
        }
        else
        {
            Instantiate(prefab, new Vector3(transform.position.x - 13, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x - 11, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x - 9, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x - 7, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x - 5, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x - 3, transform.position.y + 5, transform.position.z), Quaternion.identity);
            
            Instantiate(prefab, new Vector3(transform.position.x + 3, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x + 5, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x + 7, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x + 9, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x + 11, transform.position.y + 5, transform.position.z), Quaternion.identity);
            Instantiate(prefab, new Vector3(transform.position.x + 13, transform.position.y + 5, transform.position.z), Quaternion.identity);
        }
    }
}
