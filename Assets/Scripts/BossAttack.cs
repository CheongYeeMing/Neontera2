using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] public float attackDelay;
    [SerializeField] public float KnockbackX;
    [SerializeField] public float KnockbackY;

    // Boss Damage
    [SerializeField] public float attack;

    protected bool isAttacking;

    // Boss Animation States
    protected const string BOSS_ATTACK = "Attack";

    // Boss Auto Attack
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            if (isAttacking == false)
            {
                Debug.Log("attack animation called");
                isAttacking = true;
                gameObject.GetComponent<BossAnimation>().ChangeAnimationState(BOSS_ATTACK);
            }
            collision.gameObject.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
            Invoke("AttackComplete", attackDelay);
        }
    }

    public virtual void AttackComplete()
    {
        isAttacking = false;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
