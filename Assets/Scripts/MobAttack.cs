using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAttack : MonoBehaviour
{
    [SerializeField] public float attackDelay;
    [SerializeField] public float KnockbackX;
    [SerializeField] public float KnockbackY;

    // Mob Damage
    public float attack;

    public bool isAttacking;

    // Mob Animation States
    public const string MOB_ATTACK = "Attack";

    // Mob Auto Attack
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            if (isAttacking == false)
            {
                Debug.Log("attack animation called");
                isAttacking = true;
                gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_ATTACK);
            }
            collision.gameObject.GetComponent<CharacterHealth>().attackedBy = gameObject;
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
            Invoke("AttackComplete", attackDelay);
        }
    }

    public void AttackComplete()
    {
        isAttacking = false;
    }
}
