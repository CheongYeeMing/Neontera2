using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAttack : MonoBehaviour
{
    [SerializeField] public float attackDelay;
    [SerializeField] public float KnockbackX;
    [SerializeField] public float KnockbackY;
    [SerializeField] public bool isHostile;

    public float attack;

    // Mob Animation States
    public const string MOB_ATTACK = "Attack";

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            collision.gameObject.GetComponent<CharacterHealth>().attackedBy = gameObject;
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
        }
    }
}
