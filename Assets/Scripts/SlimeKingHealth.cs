using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlimeKingHealth : BossHealth
{
    [SerializeField] public float enrageDelay; // Animation is 1.1s

    protected const string BOSS_ENRAGE = "Enrage";
    protected const string BOSS_ENRAGED_IDLE = "EnragedIdle";
    protected const string BOSS_ENRAGED_HURT = "EnragedHurt";
    protected const string BOSS_ENRAGED_DIE = "EnragedDie";

    protected bool isEnraged;
    protected bool isEnraging;

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
        isEnraged = false;
        isEnraging = false;
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
        if (currentHealth > maxHealth * 4 / 10 && IsEnraged())
        {
            DeEnrage();
        }
        SetBossDetails(currentHealth, maxHealth);
        levelName.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + nameOffsetY);
        slider.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + hpOffsetY);
        if (isHurting || isDead || gameObject.GetComponent<BossPathfindingAI>().GetIsChasingTarget())
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
        GetComponent<SlimeKingMovement>().StopPatrol();
        DamagePopUp.Create(gameObject, damage);
        GetComponent<SlimeKingAnimation>().ChangeAnimationState(GetHurtState());
        KnockBack(attackedBy);
        currentHealth -= damage;
        Debug.Log(damage);
        Invoke("HurtComplete", hurtDelay);
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        else if (currentHealth <= maxHealth * 4 / 10 && !IsEnraged())
        {
            Enrage();
        }
    }

    public override void Die()
    {
        gameObject.GetComponent<BossSpawner>().SetDeathTimer(0);
        isDead = true;
        gameObject.GetComponent<SlimeKingMovement>().GetRigidbody().velocity = Vector2.zero;
        RewardsPopUp.Create(gameObject);
        Debug.Log("Mob is dead!!!");
        gameObject.GetComponent<SlimeKingAnimation>().ChangeAnimationState(GetDieState());
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
        Invoke("DieComplete", dieDelay);
    }

    public void Enrage()
    {
        isInvulnerable = true;
        isEnraging = true;
        isEnraged = true;
        GetComponent<SlimeKingMovement>().StopPatrol();
        gameObject.GetComponent<SlimeKingAnimation>().ChangeAnimationState(BOSS_ENRAGE);
        gameObject.GetComponent<SlimeKingMovement>().moveSpeed *= 1.5f;
        gameObject.GetComponent<SlimeKingAttack>().attack *= 1.5f;
        Invoke("EnrageComplete", enrageDelay);
    }

    public void DeEnrage()
    {
        isEnraged = false;
    }

    public void EnrageComplete()
    {
        isEnraging = false;
        isInvulnerable = false;
    }

    public bool IsEnraged()
    {
        return isEnraged;
    }

    public bool IsEnraging()
    {
        return isEnraging;
    }

    public string GetHurtState()
    {
        if (IsEnraged())
        {
            return BOSS_ENRAGED_HURT;
        }
        else
        {
            return BOSS_HURT;
        }
    }

    public string GetDieState()
    {
        if (IsEnraged())
        {
            return BOSS_ENRAGED_DIE;
        }
        else
        {
            return BOSS_DIE;
        }
    }
}
