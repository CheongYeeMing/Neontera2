using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighEliteSoldierSummonLaser : MobSummoner
{
    protected const string SHOOT_LASER = "Shoot";
    public override void Summon()
    {
        StartCoroutine(SummonLaser());
    }

    private IEnumerator SummonLaser()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 3; i++)
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
            gameObject.GetComponent<MobAnimation>().ChangeAnimationState(SHOOT_LASER);
            Instantiate(prefab, new Vector3((float)(transform.position.x + Mathf.Sign(transform.localScale.x) * 1f), transform.position.y, transform.position.z), angleAxis);
            if (target.position.x < transform.position.x)
            {
                prefab.transform.localScale = new Vector2(-prefab.transform.localScale.x, prefab.transform.localScale.y);
            }
            yield return new WaitForSeconds(0.5f);
        }
        gameObject.GetComponent<HighEliteSoldierAttack>().SummonComplete();
    }
}
