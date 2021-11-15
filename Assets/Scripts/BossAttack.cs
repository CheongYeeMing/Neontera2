using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    // Boss Animation States
    protected const string BOSS_ATTACK = "Attack";

    [SerializeField] public float attackDelay;
    [SerializeField] public float KnockbackX;
    [SerializeField] public float KnockbackY;

    // Boss Damage
    [SerializeField] public float attack;

    protected bool isAttacking;

    // Boss Auto Attack
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            if (!IsAttacking())
            {
                Attack();
            }
            DealDamage(collision);
        }
    }

    public virtual void AttackComplete()
    {
        isAttacking = false;
    }

    private void Attack()
    {
        Debug.Log("attack animation called");
        isAttacking = true;
        gameObject.GetComponent<BossAnimation>().ChangeAnimationState(BOSS_ATTACK);
    }

    private void DealDamage(Collision2D character)
    {
        character.gameObject.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
        character.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
        Invoke("AttackComplete", attackDelay);
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
