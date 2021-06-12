using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHealth : MonoBehaviour, Health
{
    public float maxHealth = 100;
    public float currentHealth;

    const string MOB_HURT = "Hurt";
    const string MOB_DIE = "Die";

    [SerializeField] public float hurtDelay;
    [SerializeField] public float dieDelay;

    public bool isHurting;
    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isHurting = false;
        isDead = false;
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
        isDead = true;
        Debug.Log("Mob is dead!!!");
        // Death animation
        gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_DIE);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Invoke("DieComplete", dieDelay);
    }

    public void HurtComplete()
    {
        isHurting = false;
    }

    public void DieComplete()
    {
        isDead = false;
        gameObject.SetActive(false);
    }
}
