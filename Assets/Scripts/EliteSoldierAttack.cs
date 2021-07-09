using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteSoldierAttack : MobAttack
{
    protected EliteSoldierSummonLaser eliteSoldierSummonLaser;
    protected float summonCooldown;
    protected float summonCooldownTimer;

    public void Start()
    {
        eliteSoldierSummonLaser = gameObject.GetComponent<EliteSoldierSummonLaser>();
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
        GetComponent<EliteSoldierMovement>().StopPatrol();
        summonCooldown = Random.Range(1f, 2f);
        eliteSoldierSummonLaser.Summon();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            isAttacking = true;
            GetComponent<EliteSoldierMovement>().StopPatrol();
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
