using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    // Character Animation States
    private const string CHARACTER_ATTACK = "Attack";
    private const string CHARACTER_ATTACK_2 = "Attack2";
    private const string CHARACTER_ATTACK_3 = "Attack3";
    private const string CHARACTER_SPECIAL_ATTACK = "AttackFireball";
    private const int COMBO_SLASH_1 = 1;
    private const int COMBO_SLASH_2 = 2;
    private const int COMBO_SLASH_3 = 3;

    [SerializeField] private Transform firePoint;
    [SerializeField] public Transform attackPoint;
    [SerializeField] public float attackDelay;
    [SerializeField] private GameObject fireball;
    [SerializeField] public float KnockbackX;
    [SerializeField] public float KnockbackY;

    public LayerMask mobLayer;
    private CharacterMovement characterMovement;

    public float baseAttack;
    private float attack;
    private float cooldownTimer = Mathf.Infinity;
    private float attackRange = 1.5f;

    private bool isAttacking;

    // Combo
    public int combo;
    [SerializeField] GameObject Slash_1;
    [SerializeField] GameObject Slash_2;
    [SerializeField] GameObject Slash_3;
    public float comboTimer;

    public void Start()
    {
        baseAttack = Data.baseAttack;
        attack = GetComponent<Character>().GetAttack().CalculateFinalValue();
    }

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        combo = COMBO_SLASH_1;
    }

    private void Update()
    {
        if (!characterMovement.IsAbleToMove()) return;
        if (Input.GetKey(KeyCode.A) && characterMovement.IsAbleToAttack() && !isAttacking && cooldownTimer > attackDelay)
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.S) && characterMovement.IsAbleToAttack() && cooldownTimer > attackDelay)
        {
            SpecialAttack();
        }
        cooldownTimer += Time.deltaTime;
        comboTimer += Time.deltaTime;
        if (comboTimer > 1f) combo = COMBO_SLASH_1;
    }

    private void Attack()
    {
        isAttacking = true;

        FindObjectOfType<AudioManager>().PlayEffect("CharacterNormalAttack");
        cooldownTimer = 0;
        comboTimer = 0;
        if (combo > COMBO_SLASH_3) combo = COMBO_SLASH_1;
        if (combo == COMBO_SLASH_1)
        {
            gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_ATTACK);
            GameObject Slash1 = Instantiate(Slash_1, new Vector2(attackPoint.position.x, attackPoint.position.y), Quaternion.identity);
            Slash1.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 0.5f, -0.5f);
        }
        else if (combo == COMBO_SLASH_2)
        {
            gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_ATTACK_2);
            GameObject Slash2 = Instantiate(Slash_2, new Vector2(attackPoint.position.x, attackPoint.position.y), Quaternion.identity);
            Slash2.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 0.4f, 0.6f);
        }
        else if (combo == COMBO_SLASH_3)
        {
            gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_ATTACK_3);
            GameObject Slash3 = Instantiate(Slash_3, attackPoint.position, Quaternion.identity) as GameObject;
            Slash3.GetComponent<Projectile>().damage = (float)(gameObject.GetComponent<Character>().GetAttack().CalculateFinalValue() * 0.75);
            Slash3.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 3f, 0);
        }
        if (combo < COMBO_SLASH_3) GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(gameObject.transform.localScale.x) * 1f, 0);
        else if (combo == COMBO_SLASH_3) GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(gameObject.transform.localScale.x) * 3f, 0);
        combo++;

        Collider2D[] hitMobs = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, mobLayer);
        attack = gameObject.GetComponent<Character>().GetAttack().CalculateFinalValue();

        foreach (Collider2D mob in hitMobs)
        {
            MobHealth mobHealth;
            if (mob.TryGetComponent<MobHealth>(out mobHealth))
            {
                if (!mob.GetComponent<MobHealth>().IsHurting())
                {
                    mob.GetComponent<MobHealth>().SetAttackedBy(gameObject);
                    mob.GetComponent<MobHealth>().TakeDamage(attack);
                }
            }
            BossHealth bossHealth;
            if (mob.TryGetComponent<BossHealth>(out bossHealth))
            {
                if (!mob.GetComponent<BossHealth>().IsHurting())
                {
                    mob.GetComponent<BossHealth>().SetAttackedBy(gameObject);
                    mob.GetComponent<BossHealth>().TakeDamage(attack);
                }
            }
        }
        foreach (Collider2D mob in hitMobs)
        {
            MobHealth mobHealth;
            if (mob.TryGetComponent<MobHealth>(out mobHealth))
            {
                if (mobHealth.IsDead() && mob.GetComponent<MobReward>().GetIsRewardGiven() == false)
                {
                    foreach (Quest quest in gameObject.GetComponent<Character>().questList.quests)
                    {
                        if (quest.questCriteria.criteriaType == CriteriaType.Kill)
                        {
                            if (quest.questCriteria.Target == mobHealth.mobName)
                            {
                                quest.questCriteria.Execute();
                                quest.Update();
                            }
                        }
                    }
                    // Rewards for Mob kill
                    mob.GetComponent<MobReward>().GetReward(gameObject.GetComponent<CharacterLevel>(), gameObject.GetComponent<CharacterWallet>());
                }
            }
            BossHealth bossHealth;
            if (mob.TryGetComponent<BossHealth>(out bossHealth))
            {
                if (bossHealth.IsDead() && mob.GetComponent<BossReward>().GetIsRewardGiven() == false)
                {
                    foreach (Quest quest in gameObject.GetComponent<Character>().questList.quests)
                    {
                        if (quest.questCriteria.criteriaType == CriteriaType.Kill)
                        {
                            if (quest.questCriteria.Target == bossHealth.mobName)
                            {
                                quest.questCriteria.Execute();
                                quest.Update();
                            }
                        }
                    }
                    // Rewards for Mob kill
                    mob.GetComponent<BossReward>().GetReward(gameObject.GetComponent<CharacterLevel>(), gameObject.GetComponent<CharacterWallet>());
                }
            }
        }
        Invoke("AttackComplete", attackDelay);
    }
    public void AttackComplete()
    {
        isAttacking = false;
        FindObjectOfType<AudioManager>().StopEffect("CharacterNormalAttack");
    }

    public void IncreaseAttack(int level)
    {
        baseAttack += (baseAttack * 0.015f) * ((100 - level) * 0.1f);
        GetComponent<Character>().Attack.SetBaseValue((int)baseAttack);
        attack = GetComponent<Character>().GetAttack().CalculateFinalValue();
        GetComponent<Character>().UpdateCharacterStats();
    }

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(firePoint.position, attackRange);
    }

    private void SpecialAttack()
    {
        gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_SPECIAL_ATTACK);
        cooldownTimer = 0;
        FindObjectOfType<AudioManager>().StopEffect("CharacterLaser");
        FindObjectOfType<AudioManager>().PlayEffect("CharacterLaser");
        GameObject fireBall = Instantiate(fireball, firePoint.transform.position, Quaternion.identity);
        fireBall.GetComponent<Projectile>().damage = (float)(gameObject.GetComponent<Character>().GetAttack().CalculateFinalValue() * 0.75);
        fireBall.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 15.0f, 0);

        Invoke("AttackComplete", attackDelay);
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public float GetBaseAttack()
    {
        return baseAttack;
    }
}