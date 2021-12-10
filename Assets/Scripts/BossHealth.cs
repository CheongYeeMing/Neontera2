using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealth : MonoBehaviour, Health
{
    protected const string BOSS_HURT = "Hurt";
    protected const string BOSS_DIE = "Die";

    [SerializeField] public GameObject levelName;
    [SerializeField] public GameObject mobDetails;
    [SerializeField] public Transform DamagePopup;
    [SerializeField] public Transform HealingPopup;
    [SerializeField] public Transform RewardPopUp;
    [SerializeField] public float dieDelay;
    [SerializeField] public float hpOffsetX;
    [SerializeField] public float hpOffsetY;
    [SerializeField] public float hurtDelay;
    [SerializeField] public float mobLevel;
    [SerializeField] public float nameOffsetY;
    [SerializeField] public float nameOffsetX;
    [SerializeField] public float maxHealth;
    [SerializeField] public string mobName;


    protected Color high;
    protected Color low;
    protected BossAnimation bossAnimation;
    protected BossMovement bossMovement;
    protected BossPathfindingAI bossPathfindingAI;
    protected GameObject attackedBy;
    protected Image levelNameBG;
    protected Slider slider;
    protected TextMeshProUGUI levelNameText;

    protected bool isDead;
    protected bool isHurting;
    protected bool isInvulnerable;
    protected float currentHealth;
    protected float outOfCombatTimer; // Default set to 5 seconds
    protected float regenTimer; // Default 10%maxHP/second


    // Start is called before the first frame update
    public virtual void Start()
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
        GetComponent<Rigidbody2D>().gravityScale = 3;
        foreach (BoxCollider2D boxCollider in GetComponents<BoxCollider2D>())
        {
            boxCollider.enabled = true;
        }
        GetComponent<SpriteRenderer>().enabled = true;
        bossAnimation = GetComponent<BossAnimation>();
        bossMovement = GetComponent<BossMovement>();
        bossPathfindingAI = GetComponent<BossPathfindingAI>();
    }

    public virtual void Update()
    {
        SetBossDetails(currentHealth, maxHealth);
        levelName.transform.position = new Vector2(transform.position.x + nameOffsetX, transform.position.y + nameOffsetY);
        slider.transform.position = new Vector2(transform.position.x + hpOffsetX, transform.position.y + hpOffsetY);
        if (isHurting || isDead || bossPathfindingAI.GetIsChasingTarget())
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

    protected void SetBossDetails(float currentHealth, float maxHealth)
    {
        mobDetails.SetActive(currentHealth != maxHealth && currentHealth > 0);
        slider.value = currentHealth;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }

    public virtual void TakeDamage(float damage)
    {
        if (isInvulnerable) return;
        isHurting = true;
        bossMovement.StopPatrol();
        DamagePopUp.Create(gameObject, damage);
        bossAnimation.ChangeAnimationState(BOSS_HURT);
        KnockBack(attackedBy);
        currentHealth -= damage;
        Debug.Log(damage);
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        Invoke("HurtComplete", hurtDelay);
    }

    public void KnockBack(GameObject something)
    {
        Debug.Log("Knockbacked???");
        Rigidbody2D body = GetComponent<BossMovement>().GetRigidbody();
        CharacterAttack character;
        if (something.transform.position.x > gameObject.transform.position.x)
        {

            if (TryGetComponent<CharacterAttack>(out character))
            {
                body.velocity += new Vector2(-character.GetComponent<CharacterAttack>().KnockbackX, character.GetComponent<CharacterAttack>().KnockbackY);
            }
            else
            {
                body.velocity += new Vector2(-3, 3.5f);
            }
        }
        else
        {
            if (TryGetComponent<CharacterAttack>(out character))
            {
                body.velocity += new Vector2(character.GetComponent<CharacterAttack>().KnockbackX, character.GetComponent<CharacterAttack>().KnockbackY);
            }
            else
            {
                body.velocity += new Vector2(3, 3.5f);
            }
        }
    }

    public virtual void Die()
    {
        GetComponent<BossSpawner>().SetDeathTimer(0);
        isDead = true;
        bossMovement.GetRigidbody().velocity = Vector2.zero;
        RewardsPopUp.Create(gameObject);
        Debug.Log("Mob is dead!!!");
        GetComponent<BossAnimation>().ChangeAnimationState(BOSS_DIE);
        GetComponent<Rigidbody2D>().gravityScale = 0;
        foreach (BoxCollider2D boxCollider in gameObject.GetComponents<BoxCollider2D>())
        {
            boxCollider.enabled = false;
        }
        Invoke("DieComplete", dieDelay);
    }

    protected void HurtComplete()
    {
        isHurting = false;
        if (bossPathfindingAI.passiveAggressive)
        {
            bossPathfindingAI.SetIsChasingTarget(true);
        }
    }

    protected void DieComplete()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public bool IsHurting()
    {
        return isHurting;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public GameObject GetAttackedBy()
    {
        return attackedBy;
    }

    public void SetAttackedBy(GameObject attackedBy)
    {
        this.attackedBy = attackedBy;
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    public void SetIsInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }
}
