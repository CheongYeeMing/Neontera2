using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private Animator animator;
    private CharacterMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    // Animation States
    const string PLAYER_ATTACK_FIREBALL = "AttackFireball";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        playerMovement.ChangeAnimationState(PLAYER_ATTACK_FIREBALL);
        cooldownTimer = 0;

        // Pool Fireballs
        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
