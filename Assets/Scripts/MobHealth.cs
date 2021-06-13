using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHealth : MonoBehaviour, Health
{
    [SerializeField] public float hurtDelay;
    [SerializeField] public float dieDelay;

    public float maxHealth = 100;
    public float currentHealth;

    public bool isHurting;
    public bool isDead;

    // Mob Animation States
    public const string MOB_HURT = "Hurt";
    public const string MOB_DIE = "Die";

    // Start is called before the first frame update
    public void Start()
    {
        currentHealth = maxHealth;
        isHurting = false;
        isDead = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
        foreach (BoxCollider2D boxCollider in gameObject.GetComponents<BoxCollider2D>())
        {
            boxCollider.enabled = true;
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void TakeDamage(float damage)
    {
        isHurting = true;
        gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_HURT);
        currentHealth -= damage;
        Debug.Log(damage);
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        Invoke("HurtComplete", hurtDelay);
    }

    public void Die()
    {
        gameObject.GetComponent<MobSpawner>().deathTimer = 0;
        isDead = true;
        Debug.Log("Mob is dead!!!");
        gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_DIE);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        foreach (BoxCollider2D boxCollider in gameObject.GetComponents<BoxCollider2D>())
        {
            boxCollider.enabled = false;
        }
        Invoke("DieComplete", dieDelay);
    }

    public void HurtComplete()
    {
        isHurting = false;
    }

    public void DieComplete()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
