using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MobHealth : MonoBehaviour, Health
{
    [SerializeField] public string mobName;
    [SerializeField] public float hurtDelay;
    [SerializeField] public float dieDelay;
    [SerializeField] public Transform DamagePopup;
    [SerializeField] public Transform HealingPopup;
    [SerializeField] public Transform RewardPopUp;
    [SerializeField] public float maxHealth;

    [SerializeField] public GameObject mobDetails;
    [SerializeField] public float hpOffsetY;
    [SerializeField] public float nameOffsetX;
    [SerializeField] public float nameOffsetY;

    [SerializeField] public GameObject levelName;
    [SerializeField] public float mobLevel;
    
    private Image levelNameBG;
    private TextMeshProUGUI levelNameText;

    private Slider slider;
    private Color low;
    private Color high;

    private float currentHealth;
    private float regenTimer; // Default 10%maxHP/second
    private float outOfCombatTimer; // Default set to 5 seconds

    private bool isHurting;
    private bool isDead;

    private GameObject attackedBy;

    // Mob Animation States
    private const string MOB_HURT = "Hurt";
    private const string MOB_DIE = "Die";

    // Start is called before the first frame update
    public void Start()
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
        SetMobDetails(currentHealth, maxHealth);
        isHurting = false;
        isDead = false;
        regenTimer = 0;
        outOfCombatTimer = 0;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
        foreach (BoxCollider2D boxCollider in gameObject.GetComponents<BoxCollider2D>())
        {
            boxCollider.enabled = true;
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Update()
    {
        SetMobDetails(currentHealth, maxHealth);
        levelName.transform.position = new Vector2(gameObject.transform.position.x + nameOffsetX, gameObject.transform.position.y + nameOffsetY);
        slider.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + hpOffsetY);
        if (isHurting || isDead ||gameObject.GetComponent<MobPathfindingAI>().GetIsChasingTarget())
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

    public void SetMobDetails(float currentHealth, float maxHealth)
    {
        mobDetails.SetActive(currentHealth != maxHealth && currentHealth > 0);
        slider.value = currentHealth;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }

    public void TakeDamage(float damage)
    {
        isHurting = true;
        gameObject.GetComponent<MobMovement>().StopPatrol();
        DamagePopUp.Create(gameObject, damage);
        gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_HURT);
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
        Rigidbody2D body = gameObject.GetComponent<MobMovement>().GetRigidbody();
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

    public void Die()
    {
        gameObject.GetComponent<MobSpawner>().SetDeathTimer(0);
        isDead = true;
        gameObject.GetComponent<MobMovement>().GetRigidbody().velocity = Vector2.zero;
        RewardsPopUp.Create(gameObject);
        Debug.Log("Mob is dead!!!");
        gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_DIE);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        foreach (BoxCollider2D boxCollider in gameObject.GetComponents<BoxCollider2D>())
        {
            boxCollider.enabled = false;
        }
        Invoke("DieComplete", dieDelay);
    }

    public void HurtComplete()
    {
        isHurting = false;
        if (gameObject.GetComponent<MobPathfindingAI>().passiveAggressive)
        {
            gameObject.GetComponent<MobPathfindingAI>().SetIsChasingTarget(true);
        }
    }

    public void DieComplete()
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
}
