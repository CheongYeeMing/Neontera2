using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleKingAttack : BossAttack
{
    protected const string BOSS_ATTACK_2 = "Attack2";
    protected const string BOSS_ATTACK_3 = "Attack3";
    protected const string BOSS_ATTACK_4 = "Attack4";

    protected TurtleKingSummonFactory turtleKingSummonFactory;
    protected float summonCooldown;
    protected float summonCooldownTimer;

    public void Start()
    {
        turtleKingSummonFactory = gameObject.GetComponent<TurtleKingSummonFactory>();
        summonCooldown = 5f;
        summonCooldownTimer = 0f;
    }

    // Update is called once per frame
    public void Update()
    {
        if (GetComponent<TurtleKingHealth>().IsDead()) return;
        if (!GetComponent<TurtleKingPathfindingAI>().GetIsChasingTarget()) return;
        if (summonCooldownTimer > summonCooldown && isAttacking == false)
        {
            Attack();
        }
        summonCooldownTimer += Time.deltaTime;
    }

    public void Attack()
    {
        isAttacking = true;
        GetComponent<TurtleKingHealth>().SetIsInvulnerable(true);
        GetComponent<TurtleKingMovement>().StopPatrol();
        summonCooldown = Random.Range(3f, 5f);
        string[] skills = turtleKingSummonFactory.GetSkills();
        turtleKingSummonFactory.Summon(skills[Random.Range(0, skills.Length)]);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            isAttacking = true;
            GetComponent<TurtleKingMovement>().StopPatrol();
            Debug.Log("attack animation called");
            GetComponent<TurtleKingAnimation>().ChangeAnimationState(BOSS_ATTACK);
            collision.gameObject.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
            Invoke("AttackComplete", attackDelay);
        }
    }

    //public string GetAttackState()
    //{
    //    if (GetComponent<TurtleKingHealth>().IsEnraged())
    //    {
    //        return BOSS_ENRAGED_ATTACK;
    //    }
    //    else
    //    {
    //        return BOSS_ATTACK;
    //    }
    //}

    public void SummonComplete()
    {
        isAttacking = false;
        summonCooldownTimer = 0f;
        GetComponent<TurtleKingHealth>().SetIsInvulnerable(false);
    }
}
