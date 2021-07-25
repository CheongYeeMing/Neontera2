using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SupremeLeaderHealth : BossHealth
{
    [SerializeField] Monologue GameCompleteMonologue;
    [SerializeField] GameObject HighEliteSoldiers;
    [SerializeField] GameObject HighEliteSoldierDetails;
    [SerializeField] GameObject EliteSoldiers;
    [SerializeField] GameObject EliteSoldierDetails;

    // Start is called before the first frame update
    public override void Start()
    {
        slider = mobDetails.GetComponentInChildren<Slider>();
        levelNameBG = levelName.GetComponentInChildren<Image>();
        levelNameText = levelName.GetComponentInChildren<TextMeshProUGUI>();
        levelNameText.SetText("Lv" + mobLevel + " " + mobName);
        currentHealth = maxHealth;
        low = Color.red;
        high = Color.green;
        low.a = 255;
        high.a = 255;
        SetBossDetails(currentHealth, maxHealth);
        isHurting = false;
        isDead = false;
        isInvulnerable = false;
        regenTimer = 0;
        outOfCombatTimer = 0;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
        foreach (BoxCollider2D boxCollider in gameObject.GetComponents<BoxCollider2D>())
        {
            boxCollider.enabled = true;
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        SetBossDetails(currentHealth, maxHealth);
        levelName.transform.position = new Vector2(gameObject.transform.position.x + nameOffsetX, gameObject.transform.position.y + nameOffsetY);
        slider.transform.position = new Vector2(gameObject.transform.position.x + hpOffsetX, gameObject.transform.position.y + hpOffsetY);
        if (isHurting || isDead || gameObject.GetComponent<SupremeLeaderPathfindingAI>().GetIsChasingTarget())
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
        GetComponent<SupremeLeaderMovement>().StopPatrol();
        DamagePopUp.Create(gameObject, damage);
        GetComponent<SupremeLeaderAnimation>().ChangeAnimationState(BOSS_HURT);
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
        gameObject.GetComponent<SupremeLeaderMovement>().GetRigidbody().velocity = Vector2.zero;
        RewardsPopUp.Create(gameObject);
        Debug.Log("Mob is dead!!!");
        gameObject.GetComponent<SupremeLeaderAnimation>().ChangeAnimationState(BOSS_DIE);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        GameCompleteMonologue.gameObject.transform.position = FindObjectOfType<Character>().gameObject.transform.position;
        GameCompleteMonologue.gameObject.SetActive(true);
        HighEliteSoldiers.SetActive(false);
        HighEliteSoldierDetails.SetActive(false);
        EliteSoldiers.SetActive(false);
        EliteSoldierDetails.SetActive(false);
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
        Invoke("DieComplete", dieDelay);
    }

    public void Heal()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += maxHealth / 10;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            HealingPopUp.Create(gameObject, maxHealth / 10);
        }
    }
}
