using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterHealth : MonoBehaviour, Health
{
    private const float GRAVITY_SCALE_ZERO = 0;
    private const float GRAVITY_SCALE_NORMAL = 3;
    private const float HEALTH_BAR_CHIP_SPEED = 2f;
    private const string CHARACTER_HURT = "Hurt";
    private const string CHARACTER_DIE = "Die";
    private const string HEALTH_TEXT_SEPARATOR = "/";

    [SerializeField] private GameOver gameOver;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private float hurtDelay;

    private Character character;
    private CharacterAnimation characterAnimation;
    private CharacterAttack characterAttack;
    private GameObject attackedBy;
    private Rigidbody2D body;

    private bool isHurting;
    private bool isDead;
    private float health;
    private float lerpTimer;
    private float baseMaxHealth;
    private float maxHealth;

    private void Awake()
    {
        GetCharacterHealthComponents();
    }

    // Start is called before the first frame update
    void Start()
    {
        baseMaxHealth = Data.baseHealth;
        if (Data.currentHealth == 0) 
        { 
            maxHealth = character.GetHealth().CalculateFinalValue();
            health = maxHealth;
            Data.currentHealth = health;
        }
        else
        {
            health = Data.currentHealth;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        maxHealth = character.GetHealth().CalculateFinalValue();
        health = Mathf.Clamp(health, 0, maxHealth);
        if (transform.position.y < -100)
        {
            health -= maxHealth * 0.2f; // When fall out of map, slow death.
            if (health <= 0 && !isDead)
            {
                isDead = true;
                health = 0;
                Die();
            }
        }
        UpdateHealthBarUI();
    }

    private void GetCharacterHealthComponents()
    {
        body = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
        characterAnimation = GetComponent<CharacterAnimation>();
        characterAttack = GetComponent<CharacterAttack>();
    }

    public void UpdateHealthBarUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;
        float percentComplete = lerpTimer / HEALTH_BAR_CHIP_SPEED;
        float percentCompleteSquared = percentComplete * percentComplete;
        if (fillBack > healthFraction)
        {
            lerpTimer += Time.deltaTime;
            backHealthBar.color = Color.red;
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentCompleteSquared);
        }
        else if (fillFront < healthFraction)
        {
            lerpTimer += Time.deltaTime;
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = healthFraction;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, healthFraction, percentCompleteSquared);
        }
        healthText.text = Mathf.Round(health) + HEALTH_TEXT_SEPARATOR + Mathf.Round(maxHealth); 
    }

    public void TakeDamage(float damage)
    {
        if (characterAttack.IsAttacking())
        {
            return;
        }
        FindObjectOfType<AudioManager>().StopEffect("CharacterHurt");
        FindObjectOfType<AudioManager>().PlayEffect("CharacterHurt");
        isHurting = true;
        characterAnimation.ChangeAnimationState(CHARACTER_HURT);
        CinemachineShake.Instance.Hit();
        KnockBack(attackedBy);
        health -= damage;
        lerpTimer = 0f;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        Invoke("HurtComplete", hurtDelay);
    }

    public void HurtComplete()
    {
        isHurting = false;
    }

    public void KnockBack(GameObject mob)
    {
        MobAttack mobAttack;
        if (mob.TryGetComponent(out mobAttack))
        {
            if (mob.transform.position.x > transform.position.x)
            {

                body.velocity = new Vector2(body.velocity.x - mobAttack.KnockbackX, body.velocity.y + mobAttack.KnockbackY);
            }
            else
            {
                body.velocity = new Vector2(body.velocity.x + mobAttack.KnockbackX, body.velocity.y + mobAttack.KnockbackY);
            }
        }
        BossAttack bossAttack;
        if (mob.TryGetComponent(out bossAttack))
        {
            if (mob.transform.position.x > transform.position.x)
            {

                body.velocity = new Vector2(body.velocity.x - bossAttack.KnockbackX, body.velocity.y + bossAttack.KnockbackY);
            }
            else
            {
                body.velocity = new Vector2(body.velocity.x + bossAttack.KnockbackX, body.velocity.y + bossAttack.KnockbackY);
            }
        }
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    public void IncreaseHealth(int level)
    {
        baseMaxHealth += (baseMaxHealth * 0.01f) * ((100 - level) * 0.1f);
        health += (baseMaxHealth * 0.01f) * ((100 - level) * 0.1f);
        maxHealth += (baseMaxHealth * 0.01f) * ((100 - level) * 0.1f);
        character.Health.SetBaseValue((int)baseMaxHealth);
        character.UpdateCharacterStats();
    }

    public void Die()
    {
        isDead = true;
        characterAnimation.ChangeAnimationState(CHARACTER_DIE);
        FindObjectOfType<AudioManager>().StopEffect("Run");
        FindObjectOfType<AudioManager>().PlayEffect("CharacterDie");
        GetComponent<BoxCollider2D>().enabled = false;
        body.velocity = Vector2.zero;
        body.gravityScale = GRAVITY_SCALE_ZERO;
        gameOver.gameObject.SetActive(true);
    }

    public void Revive()
    {
        isDead = false;
        health = maxHealth;
        GetComponent<BoxCollider2D>().enabled = true;
        body.gravityScale = GRAVITY_SCALE_NORMAL;
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

    public void FullRestore()
    {
        health = maxHealth;
        FindObjectOfType<AudioManager>().PlayEffect("CharacterHeal");
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    public float GetBaseHealth()
    {
        return baseMaxHealth;
    }
}
