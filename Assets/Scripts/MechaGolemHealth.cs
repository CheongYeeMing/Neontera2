using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MechaGolemHealth : BossHealth
{
    [SerializeField] GameObject injuredSoldier;
    [SerializeField] NPC DrAaron;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        SetBossDetails(currentHealth, maxHealth);
        levelName.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + nameOffsetY);
        slider.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + hpOffsetY);
        if (isHurting || isDead || gameObject.GetComponent<MechaGolemPathfindingAI>().GetIsChasingTarget())
        {
            outOfCombatTimer = 0;
        }
        else
        {
            if (outOfCombatTimer > 5)
            {
                if (currentHealth < maxHealth && regenTimer > 1)
                {
                    currentHealth += maxHealth / 10;
                    if (currentHealth > maxHealth)
                    {
                        currentHealth = maxHealth;
                    }
                    HealingPopUp.Create(gameObject, maxHealth / 10);
                    regenTimer = 0;
                }
            }
        }
        regenTimer += Time.deltaTime;
        outOfCombatTimer += Time.deltaTime;
    }

    public override void TakeDamage(float damage)
    {
        if (isInvulnerable) return;
        isHurting = true;
        GetComponent<MechaGolemMovement>().StopPatrol();
        DamagePopUp.Create(gameObject, damage);
        GetComponent<MechaGolemAnimation>().ChangeAnimationState(BOSS_HURT);
        KnockBack(attackedBy);
        currentHealth -= damage;
        Debug.Log(damage);
        Invoke("HurtComplete", hurtDelay);
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
    }

    public override void Die()
    {
        gameObject.GetComponent<BossSpawner>().SetDeathTimer(0);
        isDead = true;
        gameObject.GetComponent<MechaGolemMovement>().GetRigidbody().velocity = Vector2.zero;
        RewardsPopUp.Create(gameObject);
        Debug.Log("Mob is dead!!!");
        gameObject.GetComponent<MechaGolemAnimation>().ChangeAnimationState(BOSS_DIE);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        injuredSoldier.gameObject.SetActive(false);
        DrAaron.sequenceNumber = 4;
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
        Invoke("DieComplete", dieDelay);
    }

    public void Heal()
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += maxHealth / 20;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            HealingPopUp.Create(gameObject, maxHealth / 20);
        }
    }
}
