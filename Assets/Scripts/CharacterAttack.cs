using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;

    [SerializeField] private Transform firePoint;

    [SerializeField] public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask mobLayer;
    public bool isAttacking;
    [SerializeField] public float attackDelay = 0.3f;

    [SerializeField] private GameObject[] fireballs;

    [SerializeField] private GameObject fireball;

    private Animator animator;
    private CharacterMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    // Animation States
    const string PLAYER_ATTACK = "Attack";
    const string PLAYER_SPECIAL_ATTACK = "AttackFireball";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) && playerMovement.canAttack() && cooldownTimer > attackCooldown)
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.S) && playerMovement.canAttack() && cooldownTimer > attackCooldown)
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

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, mobLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit" + enemy.name);
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
        fireBall.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 15.0f, 0);

        Invoke("AttackComplete", attackDelay);
    }
}
