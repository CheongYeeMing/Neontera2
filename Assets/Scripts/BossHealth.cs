using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealth : MonoBehaviour, Health
{
    protected const float GRAVITY_SCALE_ZERO = 0;
    protected const float GRAVITY_SCALE_NORMAL = 3;
    protected const float OPACITY_MAX = 255;
    protected const string BOSS_HURT = "Hurt";
    protected const string BOSS_DIE = "Die";
    protected const string LEVEL_TEXT = "Lv";
    protected const string SPACE = " ";

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
    protected BossSpawner bossSpawner;
    protected BoxCollider2D[] boxColliders;
    public GameObject attackedBy;
    protected Image levelNameBG;
    protected Rigidbody2D rigidBody;
    protected Slider slider;
    protected SpriteRenderer spriteRenderer;
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
        GetBossComponents();
        GetBossUiComponents();
        levelNameText.SetText(LEVEL_TEXT + mobLevel + SPACE + mobName);
        currentHealth = maxHealth;
        SetBossHealthBarColour();
        SetBossDetails(currentHealth, maxHealth);
        isHurting = false;
        isDead = false;
        isInvulnerable = false;
        regenTimer = 0;
        outOfCombatTimer = 0;
        rigidBody.gravityScale = GRAVITY_SCALE_NORMAL;
        foreach (BoxCollider2D boxCollider in boxColliders)
        {
            boxCollider.enabled = true;
        }
        spriteRenderer.enabled = true;
    }

    private void GetBossComponents()
    {
        bossAnimation = GetComponent<BossAnimation>();
        bossMovement = GetComponent<BossMovement>();
        bossPathfindingAI = GetComponent<BossPathfindingAI>();
        boxColliders = GetComponents<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void GetBossUiComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        slider = mobDetails.GetComponentInChildren<Slider>();
        levelNameBG = levelName.GetComponentInChildren<Image>();
        levelNameText = levelName.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void SetBossHealthBarColour()
    {
        low = Color.red;
        high = Color.green;
        low.a = OPACITY_MAX;
        high.a = OPACITY_MAX;
    }

    protected void SetBossDetails(float currentHealth, float maxHealth)
    {
        mobDetails.SetActive(currentHealth != maxHealth && currentHealth > 0);
        slider.value = currentHealth;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
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

    public virtual void TakeDamage(float damage)
    {
        if (isInvulnerable) return;
        isHurting = true;
        bossMovement.StopPatrol();
        DamagePopUp.Create(gameObject, damage);
        bossAnimation.ChangeAnimationState(BOSS_HURT);
        KnockBack(attackedBy);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        Invoke("HurtComplete", hurtDelay);
    }

    public void KnockBack(GameObject something)
    {
        CharacterAttack character;
        if (something.transform.position.x > gameObject.transform.position.x)
        {

            if (TryGetComponent(out character))
            {
                rigidBody.velocity += new Vector2(-character.KnockbackX, character.KnockbackY);
            }
            else
            {
                rigidBody.velocity += new Vector2(-3, 3.5f);
            }
        }
        else
        {
            if (TryGetComponent(out character))
            {
                rigidBody.velocity += new Vector2(character.KnockbackX, character.KnockbackY);
            }
            else
            {
                rigidBody.velocity += new Vector2(3, 3.5f);
            }
        }
    }

    public virtual void Die()
    {
        isDead = true;
        bossSpawner.SetDeathTimer(0);
        bossMovement.GetRigidbody().velocity = Vector2.zero;
        RewardsPopUp.Create(gameObject);
        bossAnimation.ChangeAnimationState(BOSS_DIE);
        rigidBody.gravityScale = 0;
        foreach (BoxCollider2D boxCollider in boxColliders)
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
        spriteRenderer.enabled = false;
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
