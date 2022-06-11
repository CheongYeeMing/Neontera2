using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterHealth : MonoBehaviour, Health
{
    private const float DEATH_DELAY = 2;
    private const float GRAVITY_SCALE_ZERO = 0;
    private const float GRAVITY_SCALE_NORMAL = 3;
    private const float HEALTH_BAR_CHIP_SPEED = 2f;
    private const float HEALTH_ZERO = 0;
    private const float MAXIMUM_LEVEL = 100;
    private const float TRANSFORM_NEGATIVE_LIMIT = -100;
    private const string AUDIO_CHARACTER_HEAL = "CharacterHeal";
    private const string AUDIO_CHARACTER_HURT = "CharacterHurt";
    private const string AUDIO_CHARACTER_DIE = "CharacterDie";
    private const string AUDIO_RUN = "Run";
    private const string CHARACTER_HURT = "Hurt";
    private const string CHARACTER_DIE = "Die";
    private const string HEALTH_TEXT_SEPARATOR = "/";

    [SerializeField] private GameOver gameOver;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private float hurtDelay;

    private BoxCollider2D boxCollider;
    private Character character;
    private CharacterAnimation characterAnimation;
    private CharacterAttack characterAttack;
    private CharacterWallet characterWallet;
    private GameObject attackedBy;
    private Rigidbody2D rigidBody;

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
    private void Start()
    {
        //if (Input.GetKeyDown(KeyCode.V)) health = 1;
        baseMaxHealth = Data.baseHealth;
        if (Data.currentHealth == HEALTH_ZERO) 
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            health = 1;
        }
        UpdateHealthValue();
        if (CharacterOutOfBounds())
        {
            SlowlyKillCharacter();
        }
        UpdateHealthBarUI();
    }

    private void UpdateHealthValue()
    {
        maxHealth = character.GetHealth().CalculateFinalValue();
        health = Mathf.Clamp(health, HEALTH_ZERO, maxHealth);
    }

    private void SlowlyKillCharacter()
    {
        health -= maxHealth * 0.2f; // When fall out of map, slow death.
        if (HealthBelowZeroAndAlive())
        {
            Die(); // Kill Character
        }
    }

    private bool CharacterOutOfBounds()
    {
        return transform.position.y < TRANSFORM_NEGATIVE_LIMIT;
    }

    private void GetCharacterHealthComponents()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        character = GetComponent<Character>();
        characterAnimation = GetComponent<CharacterAnimation>();
        characterAttack = GetComponent<CharacterAttack>();
        characterWallet = GetComponent<CharacterWallet>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private bool HealthBelowZeroAndAlive()
    {
        bool characterIsDead = health <= HEALTH_ZERO && !isDead;
        return characterIsDead;
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
        FindObjectOfType<AudioManager>().StopEffect(AUDIO_CHARACTER_HURT);
        FindObjectOfType<AudioManager>().PlayEffect(AUDIO_CHARACTER_HURT);
        isHurting = true;
        characterAnimation.ChangeAnimationState(CHARACTER_HURT);
        CinemachineShake.Instance.Hit();
        KnockBack(attackedBy);
        health -= damage;
        ResetLerpTimer();
        if (HealthBelowZeroAndAlive())
        {
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

                rigidBody.velocity = new Vector2(rigidBody.velocity.x - mobAttack.KnockbackX, rigidBody.velocity.y + mobAttack.KnockbackY);
            }
            else
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x + mobAttack.KnockbackX, rigidBody.velocity.y + mobAttack.KnockbackY);
            }
        }
        BossAttack bossAttack;
        if (mob.TryGetComponent(out bossAttack))
        {
            if (mob.transform.position.x > transform.position.x)
            {

                rigidBody.velocity = new Vector2(rigidBody.velocity.x - bossAttack.KnockbackX, rigidBody.velocity.y + bossAttack.KnockbackY);
            }
            else
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x + bossAttack.KnockbackX, rigidBody.velocity.y + bossAttack.KnockbackY);
            }
        }
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        ResetLerpTimer();
    }

    public void IncreaseHealth(int level)
    {
        baseMaxHealth += (baseMaxHealth * 0.01f) * ((MAXIMUM_LEVEL - level) * 0.1f);
        health += (baseMaxHealth * 0.01f) * ((MAXIMUM_LEVEL - level) * 0.1f);
        maxHealth += (baseMaxHealth * 0.01f) * ((MAXIMUM_LEVEL - level) * 0.1f);
        character.Health.SetBaseValue((int)baseMaxHealth);
        character.UpdateCharacterStats();
    }

    // Invoked by TakeDamage method above
    // Removes player control over character 
    public void Die()
    {
        isDead = true;
        health = HEALTH_ZERO;
        characterAnimation.ChangeAnimationState(CHARACTER_DIE);
        FindObjectOfType<AudioManager>().StopEffect(AUDIO_RUN);
        FindObjectOfType<AudioManager>().PlayEffect(AUDIO_CHARACTER_DIE);
        boxCollider.enabled = false;
        rigidBody.velocity = Vector2.zero;
        rigidBody.gravityScale = GRAVITY_SCALE_ZERO;
        Invoke("DeathComplete", DEATH_DELAY);
    }

    private void DeathComplete()
    {
        gameOver.gameObject.SetActive(true);
    }

    private void ResetLerpTimer()
    {
        lerpTimer = 0f;
    }

    public void Revive()
    {
        isDead = false;
        health = maxHealth;
        boxCollider.enabled = true;
        rigidBody.gravityScale = GRAVITY_SCALE_NORMAL;
        characterWallet.Revive();
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
        float missingHealth = maxHealth - health;
        RestoreHealth(missingHealth);
        FindObjectOfType<AudioManager>().PlayEffect(AUDIO_CHARACTER_HEAL);
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
