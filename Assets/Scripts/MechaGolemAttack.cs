using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaGolemAttack : BossAttack
{
    protected const string BOSS_ATTACK_2 = "Attack2";
    protected const string BOSS_ATTACK_3 = "Attack3";
    protected const string BOSS_ATTACK_4 = "Attack4";

    protected MechaGolemSummonFactory mechaGolemSummonFactory;
    protected float summonCooldown;
    protected float summonCooldownTimer;

    public void Start()
    {
        mechaGolemSummonFactory = gameObject.GetComponent<MechaGolemSummonFactory>();
        summonCooldown = 5f;
        summonCooldownTimer = 0f;
    }

    // Update is called once per frame
    public void Update()
    {
        if (GetComponent<MechaGolemHealth>().IsDead()) return;
        if (!GetComponent<MechaGolemPathfindingAI>().GetIsChasingTarget()) return;
        if (summonCooldownTimer > summonCooldown && isAttacking == false)
        {
            Attack();
        }
        summonCooldownTimer += Time.deltaTime;
    }

    public void Attack()
    {
        isAttacking = true;
        GetComponent<MechaGolemHealth>().SetIsInvulnerable(true);
        GetComponent<MechaGolemMovement>().StopPatrol();
        summonCooldown = Random.Range(2f, 5f);
        string[] skills = mechaGolemSummonFactory.GetSkills();
        mechaGolemSummonFactory.Summon(skills[Random.Range(0, skills.Length)]);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            isAttacking = true;
            GetComponent<MechaGolemMovement>().StopPatrol();
            Debug.Log("attack animation called");
            GetComponent<MechaGolemAnimation>().ChangeAnimationState(BOSS_ATTACK);
            collision.gameObject.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
            Invoke("AttackComplete", attackDelay);
        }
    }

    public void SummonComplete()
    {
        isAttacking = false;
        summonCooldownTimer = 0f;
        GetComponent<MechaGolemHealth>().SetIsInvulnerable(false);
    }
}
