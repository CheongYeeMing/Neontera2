using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] public float attackDelay;
    [SerializeField] public float KnockbackX;
    [SerializeField] public float KnockbackY;

    public float attack;

    public bool isAttacking;

    // Boss Animation States
    public const string BOSS_ATTACK = "Attack";

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            if (isAttacking == false)
            {
                Debug.Log("attack animation called");
                isAttacking = true;
                gameObject.GetComponent<BossAnimation>().ChangeAnimationState(BOSS_ATTACK);
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
