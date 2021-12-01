using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterHealth : MonoBehaviour, Health
{
    [SerializeField] public GameOver gameOver;
    [SerializeField] public TextMeshProUGUI healthText;
    [SerializeField] public Image frontHealthBar;
    [SerializeField] public Image backHealthBar;
    [SerializeField] public float hurtDelay;

    private Character character;
    private CharacterAttack characterAttack;

    private GameObject attackedBy;

    public float health;
    private float lerpTimer;
    private float baseMaxHealth;
    public float maxHealth;
    private float chipSpeed = 2f;

    private bool isHurting;
    private bool isDead;

    // Character Animation States
    private const string CHARACTER_HURT = "Hurt";
    private const string CHARACTER_DIE = "Die";

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

    private void Awake()
    {
        character = GetComponent<Character>();
        characterAttack = GetComponent<CharacterAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        maxHealth = GetComponent<Character>().GetHealth().CalculateFinalValue();
        health = Mathf.Clamp(health, 0, maxHealth);
        if (GetComponent<Transform>().position.y < -100)
        {
            health -= maxHealth * 0.2f; // When fall out of map, slow death.
            if (health <= 0 && !isDead)
            {
                isDead = true;
                health = 0;
                Die();
            }
        }
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;
        if (fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }
        if (fillFront < healthFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = healthFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }
        healthText.text = Mathf.Round(health) + "/" + Mathf.Round(maxHealth); 
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
        gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_HURT);
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
        Rigidbody2D body = gameObject.GetComponent<CharacterMovement>().GetRigidBody();
        MobAttack mobAttack;
        if (mob.TryGetComponent<MobAttack>(out mobAttack))
        {
            if (mob.transform.position.x > gameObject.transform.position.x)
            {

                body.velocity = new Vector2(body.velocity.x - mob.GetComponent<MobAttack>().KnockbackX, body.velocity.y + mob.GetComponent<MobAttack>().KnockbackY);
            }
            else
            {
                body.velocity = new Vector2(body.velocity.x + mob.GetComponent<MobAttack>().KnockbackX, body.velocity.y + mob.GetComponent<MobAttack>().KnockbackY);
            }
        }
        BossAttack bossAttack;
        if (mob.TryGetComponent<BossAttack>(out bossAttack))
        {
            if (mob.transform.position.x > gameObject.transform.position.x)
            {

                body.velocity = new Vector2(body.velocity.x - mob.GetComponent<BossAttack>().KnockbackX, body.velocity.y + mob.GetComponent<BossAttack>().KnockbackY);
            }
            else
            {
                body.velocity = new Vector2(body.velocity.x + mob.GetComponent<BossAttack>().KnockbackX, body.velocity.y + mob.GetComponent<BossAttack>().KnockbackY);
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
        gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_DIE);
        FindObjectOfType<AudioManager>().StopEffect("Run");
        FindObjectOfType<AudioManager>().PlayEffect("CharacterDie");
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        gameOver.gameObject.SetActive(true);
    }

    public void Revive()
    {
        isDead = false;
        health = maxHealth;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().gravityScale = 1;
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
