using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAttack : MonoBehaviour
{
    public Collider2D boxCollider;
    public float attack;

    [SerializeField] public bool isHostile;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Debug.Log("damage");
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
        }
    }
}
