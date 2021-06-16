using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobHealth : MonoBehaviour, Health
{
    [SerializeField] public string mobName;
    [SerializeField] public float hurtDelay;
    [SerializeField] public float dieDelay;
    [SerializeField] public Transform DamagePopup;
    [SerializeField] public Transform HealingPopup;
    [SerializeField] public float maxHealth;

    public float currentHealth;
    public float regenTimer; // Default 10%maxHP/second
    public float outOfCombatTimer; // Default set to 5 seconds

    public bool isHurting;
    public bool isDead;


    public GameObject attackedBy;

    // Mob Animation States
    public const string MOB_HURT = "Hurt";
    public const string MOB_DIE = "Die";

    // Start is called before the first frame update
    public void Start()
    {
        currentHealth = maxHealth;
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
        if (isHurting || isDead ||gameObject.GetComponent<MobPathfindingAI>().isChasingTarget)
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
        Rigidbody2D body = gameObject.GetComponent<MobMovement>().rb;
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
        gameObject.GetComponent<MobSpawner>().deathTimer = 0;
        isDead = true;
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
            gameObject.GetComponent<MobPathfindingAI>().isChasingTarget = true;
        }
    }

    public void DieComplete()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
