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

    private Character character;
    private CharacterAnimation characterAnimation;
    private CharacterLevel characterLevel;
    private CharacterMovement characterMovement;
    private CharacterWallet characterWallet;
    private Rigidbody2D rigidBody;

    public float baseAttack;
    private float attack;
    private float attackCooldownTimer = Mathf.Infinity;
    private float attackRange = 1.5f;

    private bool isAttacking;

    // Combo
    public int attackCombo;
    [SerializeField] GameObject Slash_1;
    [SerializeField] GameObject Slash_2;
    [SerializeField] GameObject Slash_3;
    public float attackComboTimer;

    public void Start()
    {
        baseAttack = Data.baseAttack;
        UpdateAttackPower();
    }

    private void Awake()
    {
        character = GetComponent<Character>();
        characterAnimation = GetComponent<CharacterAnimation>();
        characterLevel = GetComponent<CharacterLevel>();
        characterMovement = GetComponent<CharacterMovement>();
        characterWallet = GetComponent<CharacterWallet>();
        rigidBody = GetComponent<Rigidbody2D>();
        attackCombo = COMBO_SLASH_1;
    }

    private void Update()
    {
        if (!characterMovement.IsAbleToMove())
        {
            return;
        }
        if (Input.GetKey(KeyCode.A) && characterMovement.IsAbleToAttack() && !isAttacking && attackCooldownTimer > attackDelay)
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.S) && characterMovement.IsAbleToAttack() && attackCooldownTimer > attackDelay)
        {
            SpecialAttack();
        }
        attackCooldownTimer += Time.deltaTime;
        attackComboTimer += Time.deltaTime;
        if (attackComboTimer > 1f)
        {
            ResetAttackCombo();
        }
    }

    private void Attack()
    {
        isAttacking = true;

        FindObjectOfType<AudioManager>().PlayEffect("CharacterNormalAttack");
        ResetAttackCooldown();
        ResetAttackComboTimer();
        if (attackCombo > COMBO_SLASH_3)
        {
            ResetAttackCombo();
        }
        if (attackCombo == COMBO_SLASH_1)
        {
            characterAnimation.ChangeAnimationState(CHARACTER_ATTACK);
            GameObject Slash1 = Instantiate(Slash_1, new Vector2(attackPoint.position.x, attackPoint.position.y), Quaternion.identity);
            Slash1.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 0.5f, -0.5f);
        }
        else if (attackCombo == COMBO_SLASH_2)
        {
            characterAnimation.ChangeAnimationState(CHARACTER_ATTACK_2);
            GameObject Slash2 = Instantiate(Slash_2, new Vector2(attackPoint.position.x, attackPoint.position.y), Quaternion.identity);
            Slash2.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 0.4f, 0.6f);
        }
        else if (attackCombo == COMBO_SLASH_3)
        {
            characterAnimation.ChangeAnimationState(CHARACTER_ATTACK_3);
            GameObject Slash3 = Instantiate(Slash_3, attackPoint.position, Quaternion.identity) as GameObject;
            Slash3.GetComponent<Projectile>().damage = (float)(character.GetAttack().CalculateFinalValue() * 0.75);
            Slash3.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 3f, 0);
        }
        if (attackCombo < COMBO_SLASH_3)
        {
            rigidBody.velocity = new Vector2(Mathf.Sign(gameObject.transform.localScale.x) * 1f, 0);
        }
        else if (attackCombo == COMBO_SLASH_3)
        {
            rigidBody.velocity = new Vector2(Mathf.Sign(gameObject.transform.localScale.x) * 3f, 0);
        }
        attackCombo++;

        Collider2D[] hitMobs = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, mobLayer);
        UpdateAttackPower();

        DealDamageToMobs(hitMobs);
        UpdateQuestAndReward(hitMobs);
        Invoke("AttackComplete", attackDelay);
    }

    private void DealDamageToMobs(Collider2D[] hitMobs)
    {
        foreach (Collider2D mob in hitMobs)
        {
            MobHealth mobHealth;
            if (mob.TryGetComponent(out mobHealth))
            {
                if (!mobHealth.IsHurting())
                {
                    mobHealth.SetAttackedBy(gameObject);
                    mobHealth.TakeDamage(attack);
                }
            }
            BossHealth bossHealth;
            if (mob.TryGetComponent(out bossHealth))
            {
                if (!bossHealth.IsHurting())
                {
                    bossHealth.SetAttackedBy(gameObject);
                    bossHealth.TakeDamage(attack);
                }
            }
        }
    }

    private void UpdateQuestAndReward(Collider2D[] hitMobs)
    {
        foreach (Collider2D mob in hitMobs)
        {
            MobHealth mobHealth;
            if (mob.TryGetComponent(out mobHealth))
            {
                MobReward mobReward = mob.GetComponent<MobReward>();
                if (mobHealth.IsDead() && !mobReward.GetIsRewardGiven())
                {
                    foreach (Quest quest in character.questList.quests)
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
                    mobReward.GetReward(characterLevel, characterWallet);
                }
            }
            BossHealth bossHealth;
            if (mob.TryGetComponent(out bossHealth))
            {
                BossReward bossReward = mob.GetComponent<BossReward>();
                if (bossHealth.IsDead() && !bossReward.GetIsRewardGiven())
                {
                    foreach (Quest quest in character.questList.quests)
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
                    bossReward.GetReward(characterLevel, characterWallet);
                }
            }
        }
    }

    private void UpdateAttackPower()
    {
        attack = character.GetAttack().CalculateFinalValue();
    }

    private void AttackComplete()
    {
        isAttacking = false;
        FindObjectOfType<AudioManager>().StopEffect("CharacterNormalAttack");
    }

    public void IncreaseAttack(int level)
    {
        baseAttack += (baseAttack * 0.015f) * ((100 - level) * 0.1f);
        character.Attack.SetBaseValue((int)baseAttack);
        UpdateAttackPower();
        character.UpdateCharacterStats();
    }

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(firePoint.position, attackRange);
    }

    private void SpecialAttack()
    {
        characterAnimation.ChangeAnimationState(CHARACTER_SPECIAL_ATTACK);
        ResetAttackCooldown();
        FindObjectOfType<AudioManager>().StopEffect("CharacterLaser");
        FindObjectOfType<AudioManager>().PlayEffect("CharacterLaser");
        GameObject fireBall = Instantiate(fireball, firePoint.transform.position, Quaternion.identity);
        fireBall.GetComponent<Projectile>().damage = (float)(character.GetAttack().CalculateFinalValue() * 0.75);
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

    private void ResetAttackCooldown()
    {
        attackCooldownTimer = 0;
    }

    private void ResetAttackCombo()
    {
        attackCombo = COMBO_SLASH_1;
    }

    private void ResetAttackComboTimer()
    {
        attackComboTimer = 0;
    }
}