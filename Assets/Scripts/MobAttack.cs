using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAttack : MonoBehaviour
{
    [SerializeField] public float attackDelay;
    [SerializeField] public float KnockbackX;
    [SerializeField] public float KnockbackY;

    // Mob Damage
    [SerializeField] public float attack;

    private bool isAttacking;

    // Mob Animation States
    private const string MOB_ATTACK = "Attack";

    // Mob Auto Attack
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            if (isAttacking == false)
            {
                isAttacking = true;
                gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_ATTACK);
            }
            collision.gameObject.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
            Invoke("AttackComplete", attackDelay);
        }
    }

    public void AttackComplete()
    {
        isAttacking = false;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
