using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlimeKingHealth : BossHealth
{
    private const float enrageDelay = 2.2f; // Animation is 1.1s
    private const string BOSS_ENRAGE = "Enrage";
    private const string BOSS_ENRAGED_DIE = "EnragedDie";
    private const string BOSS_ENRAGED_IDLE = "EnragedIdle";
    private const string BOSS_ENRAGED_HURT = "EnragedHurt";

    [SerializeField] private Monologue defeatedMonologue;

    private bool isEnraged;
    private bool isEnraging;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        isEnraged = false;
        isEnraging = false;
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
        isDead = true;
        gameObject.GetComponent<BossSpawner>().SetDeathTimer(0);
        gameObject.GetComponent<SlimeKingMovement>().GetRigidbody().velocity = Vector2.zero;
        RewardsPopUp.Create(gameObject);
        Debug.Log("Mob is dead!!!");
        gameObject.GetComponent<SlimeKingAnimation>().ChangeAnimationState(GetDieState());
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        defeatedMonologue.gameObject.transform.position = FindObjectOfType<Character>().gameObject.transform.position;
        defeatedMonologue.gameObject.SetActive(true);
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
        Invoke("DieComplete", dieDelay);
    }

    private void Enrage()
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

    private void DeEnrage()
    {
        isEnraged = false;
    }

    private void EnrageComplete()
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
