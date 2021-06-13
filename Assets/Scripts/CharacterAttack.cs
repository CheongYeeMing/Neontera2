using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private Transform firePoint;

    [SerializeField] public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask mobLayer;
    public bool isAttacking;
    [SerializeField] public float attackDelay = 1f;

    [SerializeField] private GameObject fireball;

    private Animator animator;
    private CharacterMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    public float attack;

    // Animation States
    const string PLAYER_ATTACK = "Attack";
    const string PLAYER_SPECIAL_ATTACK = "AttackFireball";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<CharacterMovement>();
        attack = gameObject.GetComponent<Character>().Attack.CalculateFinalValue();
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
        playerMovement.ChangeAnimationState(PLAYER_ATTACK);
        cooldownTimer = 0;

        Collider2D[] hitMobs = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, mobLayer);
        attack = gameObject.GetComponent<Character>().Attack.CalculateFinalValue();

        foreach (Collider2D mob in hitMobs)
        {
            mob.GetComponent<MobHealth>().TakeDamage(attack);
        }
        foreach (Collider2D mob in hitMobs)
        {
            if (mob.GetComponent<MobHealth>().isDead)
            {
                foreach(Quest quest in gameObject.GetComponent<Character>().questList.quests)
                {
                    if (quest.questCriteria.criteriaType == CriteriaType.Kill)
                    {
                        if (quest.questCriteria.Target == mob.GetComponent<MobController>().mobName)
                        {
                            quest.questCriteria.Execute();
                            quest.Update();
                        }
                    }
                }
            }
        }

        Invoke("AttackComplete", attackDelay);
    }
    public void AttackComplete()
    {
        isAttacking = false;
    }

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(firePoint.position, attackRange);
    }

    private void SpecialAttack()
    {
        playerMovement.ChangeAnimationState(PLAYER_SPECIAL_ATTACK);
        cooldownTimer = 0;

        GameObject fireBall = Instantiate(fireball, firePoint.transform.position, Quaternion.identity) as GameObject;
        fireBall.GetComponent<Projectile>().damage = (float)(gameObject.GetComponent<Character>().Attack.CalculateFinalValue()*0.75);
        fireBall.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 15.0f, 0);

        Invoke("AttackComplete", attackDelay);
    }
}
