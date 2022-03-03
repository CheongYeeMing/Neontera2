using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighEliteSoldierAttack : MobAttack
{
    protected HighEliteSoldierSummonLaser highEliteSoldierSummonLaser;
    protected float summonCooldown;
    protected float summonCooldownTimer;

    public override void Start()
    {
        highEliteSoldierSummonLaser = gameObject.GetComponent<HighEliteSoldierSummonLaser>();
        summonCooldown = 5f;
        summonCooldownTimer = 0f;
    }

    // Update is called once per frame
    public void Update()
    {
        if (GetComponent<MobHealth>().IsDead()) return;
        if (!GetComponent<MobPathfindingAI>().GetIsChasingTarget()) return;
        if (summonCooldownTimer > summonCooldown && isAttacking == false)
        {
            Attack();
        }
        summonCooldownTimer += Time.deltaTime;
    }

    public void Attack()
    {
        isAttacking = true;
        GetComponent<HighEliteSoldierMovement>().StopPatrol();
        summonCooldown = Random.Range(1f, 2f);
        highEliteSoldierSummonLaser.Summon();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            isAttacking = true;
            GetComponent<HighEliteSoldierMovement>().StopPatrol();
            Debug.Log("attack animation called");
            GetComponent<MobAnimation>().ChangeAnimationState(MOB_ATTACK);
            collision.gameObject.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
            Invoke("AttackComplete", attackDelay);
        }
    }

    public void SummonComplete()
    {
        isAttacking = false;
        summonCooldownTimer = 0f;
    }
}
