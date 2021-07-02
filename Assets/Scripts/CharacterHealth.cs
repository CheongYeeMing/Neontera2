using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterHealth : MonoBehaviour, Health
{
    [SerializeField] public TextMeshProUGUI healthText;
    [SerializeField] public Image frontHealthBar;
    [SerializeField] public Image backHealthBar;
    [SerializeField] public float hurtDelay;

    private GameObject attackedBy;

    private float health;
    private float lerpTimer;
    private float maxHealth;
    private float chipSpeed = 2f;

    private bool isHurting;
    private bool isDead;

    // Character Animation States
    private const string CHARACTER_HURT = "Hurt";
    private const string CHARACTER_DIE = "Die";

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = Data.maxHealth;
        if (Data.currentHealth == 0) 
        { 
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
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            RestoreHealth(Random.Range(5, 10));
        }
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
        isHurting = true;
        gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_HURT);
        KnockBack(attackedBy);
        health -= damage;
        lerpTimer = 0f;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        Data.currentHealth = health;
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
        Data.currentHealth = health;
    }

    public void IncreaseHealth(int level)
    {
        maxHealth += (health * 0.01f) * ((100 - level) * 0.1f);
        health = maxHealth;
        Data.currentHealth = health;
        Data.maxHealth = maxHealth;
    }

    public void Die()
    {
        Data.currentHealth = 0;
        isDead = true;
        gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_DIE);
        Debug.Log("Character is dead!!!");
        // Dead Screen, Auto Respawn in Town area??? 
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
}
