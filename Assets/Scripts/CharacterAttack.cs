using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] public Transform attackPoint;
    [SerializeField] public float attackDelay;
    [SerializeField] private GameObject fireball;
    [SerializeField] public float KnockbackX;
    [SerializeField] public float KnockbackY;
    
    public LayerMask mobLayer;
    private CharacterMovement playerMovement;

    private float attack;
    private float cooldownTimer = Mathf.Infinity;
    private float attackRange = 0.5f;

    private bool isAttacking;

    // Character Animation States
    private const string CHARACTER_ATTACK = "Attack";
    private const string CHARACTER_SPECIAL_ATTACK = "AttackFireball";

    private void Awake()
    {
        playerMovement = GetComponent<CharacterMovement>();
        attack = GetComponent<Character>().GetAttack().CalculateFinalValue();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) && playerMovement.canAttack() && !isAttacking && cooldownTimer > attackDelay)
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.S) && playerMovement.canAttack() && cooldownTimer > attackDelay)
        {
            SpecialAttack();
        }
        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        isAttacking = true;
        gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_ATTACK);
        FindObjectOfType<AudioManager>().PlayEffect("CharacterNormalAttack");
        cooldownTimer = 0;

        Collider2D[] hitMobs = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, mobLayer);
        attack = gameObject.GetComponent<Character>().GetAttack().CalculateFinalValue();

        foreach (Collider2D mob in hitMobs)
        {
            MobHealth mobHealth;
            if (mob.TryGetComponent<MobHealth>(out mobHealth))
            {
                if (!mob.GetComponent<MobHealth>().IsHurting())
                {
                    mob.GetComponent<MobHealth>().SetAttackedBy(gameObject);
                    mob.GetComponent<MobHealth>().TakeDamage(attack);
                }
            }
            BossHealth bossHealth;
            if (mob.TryGetComponent<BossHealth>(out bossHealth))
            {
                if (!mob.GetComponent<BossHealth>().IsHurting())
                {
                    mob.GetComponent<BossHealth>().SetAttackedBy(gameObject);
                    mob.GetComponent<BossHealth>().TakeDamage(attack);
                }
            }
        }
        foreach (Collider2D mob in hitMobs)
        {
            MobHealth mobHealth;
            if (mob.TryGetComponent<MobHealth>(out mobHealth))
            {
                if (mobHealth.IsDead() && mob.GetComponent<MobReward>().GetIsRewardGiven() == false)
                {
                    foreach (Quest quest in gameObject.GetComponent<Character>().questList.quests)
                    {
                        if (quest.questCriteria.criteriaType == CriteriaType.Kill)
                        {
                            if (quest.questCriteria.Target == mobHealth.mobName)
                            {
                                quest.questCriteria.Execute();
                                quest.Update();
                            }
                        }
                    }
                    // Rewards for Mob kill
                    mob.GetComponent<MobReward>().GetReward(gameObject.GetComponent<CharacterLevel>(), gameObject.GetComponent<CharacterWallet>());
                }
            }
            BossHealth bossHealth;
            if (mob.TryGetComponent<BossHealth>(out bossHealth))
            {
                if (bossHealth.IsDead() && mob.GetComponent<BossReward>().GetIsRewardGiven() == false)
                {
                    foreach (Quest quest in gameObject.GetComponent<Character>().questList.quests)
                    {
                        if (quest.questCriteria.criteriaType == CriteriaType.Kill)
                        {
                            if (quest.questCriteria.Target == bossHealth.mobName)
                            {
                                quest.questCriteria.Execute();
                                quest.Update();
                            }
                        }
                    }
                    // Rewards for Mob kill
                    mob.GetComponent<BossReward>().GetReward(gameObject.GetComponent<CharacterLevel>(), gameObject.GetComponent<CharacterWallet>());
                }
            }
        }
        Invoke("AttackComplete", attackDelay);
    }
    public void AttackComplete()
    {
        isAttacking = false;
        FindObjectOfType<AudioManager>().StopEffect("CharacterNormalAttack");
    }

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(firePoint.position, attackRange);
    }

    private void SpecialAttack()
    {
        gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_SPECIAL_ATTACK);
        cooldownTimer = 0;
        FindObjectOfType<AudioManager>().StopEffect("CharacterLaser");
        FindObjectOfType<AudioManager>().PlayEffect("CharacterLaser");
        GameObject fireBall = Instantiate(fireball, firePoint.transform.position, Quaternion.identity) as GameObject;
        fireBall.GetComponent<Projectile>().damage = (float)(gameObject.GetComponent<Character>().GetAttack().CalculateFinalValue()*0.75);
        fireBall.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 15.0f, 0);

        Invoke("AttackComplete", attackDelay);
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }
}
