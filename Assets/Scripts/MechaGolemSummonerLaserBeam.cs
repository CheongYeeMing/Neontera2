using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaGolemSummonerLaserBeam : BossSummoner
{
    protected const string MECHA_GOLEM_LASER = "Laser";
    public override void Summon()
    {
        StartCoroutine(SummonArmShard());
    }

    private IEnumerator SummonArmShard()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 1; i < 3; i++)
        {
            CinemachineShake.Instance.Shaking = true;
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
            gameObject.GetComponent<MechaGolemAnimation>().ChangeAnimationState(MECHA_GOLEM_LASER);
            FindObjectOfType<AudioManager>().StopEffect("DeathRay");
            FindObjectOfType<AudioManager>().PlayEffect("DeathRay");
            Instantiate(prefab, new Vector3((float)(transform.position.x + Mathf.Sign(transform.localScale.x) * 1), transform.position.y + 0.3f, transform.position.z), angleAxis);
            if (target.position.x < transform.position.x)
            {
                prefab.transform.localScale = new Vector2(-prefab.transform.localScale.x, prefab.transform.localScale.y);
            }
            yield return new WaitForSeconds(1.5f);
            CinemachineShake.Instance.Shaking = false;
        }
        gameObject.GetComponent<MechaGolemAttack>().SummonComplete();
    }
}
