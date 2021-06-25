using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingAttack : BossAttack
{
    protected const string BOSS_ENRAGED_ATTACK = "EnragedAttack";

    protected SlimeKingSummonFactory slimeKingSummonFactory;
    protected float summonCooldown;
    protected float summonCooldownTimer;

    public void Start()
    {
        slimeKingSummonFactory = gameObject.GetComponent<SlimeKingSummonFactory>();
        summonCooldown = 5f;
        summonCooldownTimer = 0f;
    }

    public void Update()
    {
        if (GetComponent<SlimeKingHealth>().IsDead()) return;
        if (!GetComponent<SlimeKingPathfindingAI>().GetIsChasingTarget()) return;
        if (summonCooldownTimer > summonCooldown && isAttacking == false)
        {
            Attack();
        }
        summonCooldownTimer += Time.deltaTime;
    }

    public void Attack()
    {
        isAttacking = true;
        GetComponent<SlimeKingHealth>().SetIsInvulnerable(true);
        GetComponent<SlimeKingMovement>().StopPatrol();
        summonCooldown = Random.Range(3f, 7f);
        string[] skills = slimeKingSummonFactory.GetSkills();
        slimeKingSummonFactory.Summon(skills[Random.Range(0, skills.Length)]);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            isAttacking = true;
            GetComponent<SlimeKingMovement>().StopPatrol();
            Debug.Log("attack animation called");
            GetComponent<SlimeKingAnimation>().ChangeAnimationState(GetAttackState());
            collision.gameObject.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
            Invoke("AttackComplete", attackDelay);
        }
    }

    public string GetAttackState()
    {
        if (GetComponent<SlimeKingHealth>().IsEnraged())
        {
            return BOSS_ENRAGED_ATTACK;
        }
        else
        {
            return BOSS_ATTACK;
        }
    }

    public void SummonComplete()
    {
        isAttacking = false;
        summonCooldownTimer = 0f;
        GetComponent<SlimeKingHealth>().SetIsInvulnerable(false);
    }
}
