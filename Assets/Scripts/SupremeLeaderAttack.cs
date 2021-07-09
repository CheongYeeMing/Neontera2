using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupremeLeaderAttack : BossAttack
{
    protected SupremeLeaderSummonFactory supremeLeaderSummonFactory;
    protected float summonCooldown;
    protected float summonCooldownTimer;

    public void Start()
    {
        supremeLeaderSummonFactory = gameObject.GetComponent<SupremeLeaderSummonFactory>();
        summonCooldown = 5f;
        summonCooldownTimer = 0f;
    }

    // Update is called once per frame
    public void Update()
    {
        if (GetComponent<SupremeLeaderHealth>().IsDead()) return;
        if (!GetComponent<SupremeLeaderPathfindingAI>().GetIsChasingTarget()) return;
        if (summonCooldownTimer > summonCooldown && isAttacking == false)
        {
            Attack();
        }
        summonCooldownTimer += Time.deltaTime;
    }

    public void Attack()
    {
        isAttacking = true;
        GetComponent<SupremeLeaderHealth>().SetIsInvulnerable(true);
        GetComponent<SupremeLeaderMovement>().StopPatrol();
        summonCooldown = Random.Range(2f, 5f);
        string[] skills = supremeLeaderSummonFactory.GetSkills();
        supremeLeaderSummonFactory.Summon(skills[Random.Range(0, skills.Length)]);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            isAttacking = true;
            GetComponent<SupremeLeaderMovement>().StopPatrol();
            Debug.Log("attack animation called");
            GetComponent<SupremeLeaderAnimation>().ChangeAnimationState(BOSS_ATTACK);
            collision.gameObject.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
            Invoke("AttackComplete", attackDelay);
        }
    }

    public void SummonComplete()
    {
        isAttacking = false;
        summonCooldownTimer = 0f;
        GetComponent<SupremeLeaderHealth>().SetIsInvulnerable(false);
    }
}
