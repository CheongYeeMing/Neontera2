using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    // Character Animation States
    private const string CHARACTER_ATTACK = "Attack";
    private const string CHARACTER_ATTACK_2 = "Attack2";
    private const string CHARACTER_ATTACK_3 = "Attack3";
    private const string CHARACTER_SPECIAL_ATTACK = "AttackFireball";

    [SerializeField] private Transform firePoint;
    [SerializeField] public Transform attackPoint;
    [SerializeField] private GameObject fireball;
    [SerializeField] public float attackDelay;
    [SerializeField] public float KnockbackX;
    [SerializeField] public float KnockbackY;
    
    private LayerMask mobLayer;

    private Character character;
    private CharacterAnimation characterAnimation;
    private CharacterLevel characterLevel;
    private CharacterMovement characterMovement;

    // Normal Attack 
    private float baseAttack;
    private float attack;
    private float cooldownTimer = Mathf.Infinity;
    private float attackRange = 1.5f;

    // Attack status
    private bool isAttacking;

    // Combo Variables
    [SerializeField] GameObject Slash_1;
    [SerializeField] GameObject Slash_2;
    [SerializeField] GameObject Slash_3;

    private int combo;
    private float comboTimer;

    public void Start()
    {
        baseAttack = Data.baseAttack;
        UpdateAttackValue();
    }

    private void Awake()
    {
        GetCharacterComponents();
        ResetCombo();
    }

    private void Update()
    {
        if (!characterMovement.IsAbleToMove())
        {
            return;
        }
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
        if (comboTimer > 1f)
        {
            ResetCombo();
        }
    }

    private void GetCharacterComponents()
    {
        character = GetComponent<Character>();
        characterAnimation = GetComponent<CharacterAnimation>();
        characterLevel = GetComponent<CharacterLevel>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    private void Attack()
    {
        isAttacking = true;
        FindObjectOfType<AudioManager>().PlayEffect("CharacterNormalAttack");
        ResetCooldownTimer();
        ResetComboTimer();
        ComboAttack();
        Collider2D[] hitMobs = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, mobLayer);
        UpdateAttackValue();
        DealDamageToMobs(hitMobs);
        UpdateQuestsAndRewards(hitMobs);
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
        character.Attack.SetBaseValue((int)baseAttack);
        UpdateAttackValue();
        character.statPanel.UpdateStatValues();
    }

    private void UpdateAttackValue()
    {
        attack = character.GetAttack().CalculateFinalValue();
    }

    private void ComboAttack()
    {
        if (combo >= 4)
        {
            ResetCombo();
        }
        if (combo == 1)
        {
            characterAnimation.ChangeAnimationState(CHARACTER_ATTACK);
            GameObject Slash1 = Instantiate(Slash_1, new Vector2(attackPoint.position.x, attackPoint.position.y), Quaternion.identity);
            Slash1.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 0.5f, -0.5f);
        }
        else if (combo == 2)
        {
            characterAnimation.ChangeAnimationState(CHARACTER_ATTACK_2);
            GameObject Slash2 = Instantiate(Slash_2, new Vector2(attackPoint.position.x, attackPoint.position.y), Quaternion.identity);
            Slash2.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 0.4f, 0.6f);
        }
        else if (combo == 3)
        {
            characterAnimation.ChangeAnimationState(CHARACTER_ATTACK_3);
            GameObject Slash3 = Instantiate(Slash_3, attackPoint.position, Quaternion.identity);
            Slash3.GetComponent<Projectile>().damage = (float)(character.GetAttack().CalculateFinalValue() * 0.75);
            Slash3.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 3f, 0);
        }
        if (combo < 3)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 1f, 0);
        }
        else if (combo == 3)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 3f, 0);
        }
        combo++;
    }

    private void UpdateQuestsAndRewards(Collider2D[] hitMobs)
    {
        foreach (Collider2D mob in hitMobs)
        {
            MobHealth mobHealth;
            if (mob.TryGetComponent(out mobHealth))
            {
                if (mobHealth.IsDead() && !mob.GetComponent<MobReward>().GetIsRewardGiven())
                {
                    UpdateQuestsMob(mobHealth);
                    // Rewards for Mob kill
                    mob.GetComponent<MobReward>().GetReward(characterLevel, GetComponent<CharacterWallet>());
                }
            }
            BossHealth bossHealth;
            if (mob.TryGetComponent(out bossHealth))
            {
                if (bossHealth.IsDead() && !mob.GetComponent<BossReward>().GetIsRewardGiven())
                {
                    UpdateQuestsBoss(bossHealth);
                    // Rewards for Mob kill
                    mob.GetComponent<BossReward>().GetReward(characterLevel, GetComponent<CharacterWallet>());
                }
            }
        }
    }

    private void UpdateQuestsMob(MobHealth mobHealth) 
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
    }

    private void UpdateQuestsBoss(BossHealth bossHealth)
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
    }

    private void DealDamageToMobs(Collider2D[] hitMobs)
    {
        foreach (Collider2D mob in hitMobs)
        {
            MobHealth mobHealth;
            if (mob.TryGetComponent(out mobHealth))
            {
                if (!mob.GetComponent<MobHealth>().IsHurting())
                {
                    mob.GetComponent<MobHealth>().SetAttackedBy(gameObject);
                    mob.GetComponent<MobHealth>().TakeDamage(attack);
                }
            }
            BossHealth bossHealth;
            if (mob.TryGetComponent(out bossHealth))
            {
                if (!mob.GetComponent<BossHealth>().IsHurting())
                {
                    mob.GetComponent<BossHealth>().SetAttackedBy(gameObject);
                    mob.GetComponent<BossHealth>().TakeDamage(attack);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(firePoint.position, attackRange);
    }

    private void SpecialAttack()
    {
        characterAnimation.ChangeAnimationState(CHARACTER_SPECIAL_ATTACK);
        ResetCooldownTimer();
        FindObjectOfType<AudioManager>().StopEffect("CharacterLaser");
        FindObjectOfType<AudioManager>().PlayEffect("CharacterLaser");
        GameObject fireBall = Instantiate(fireball, firePoint.transform.position, Quaternion.identity);
        fireBall.GetComponent<Projectile>().damage = (float)(character.GetAttack().CalculateFinalValue()*0.75);
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

    private void ResetCombo()
    {
        combo = 1;
    }

    private void ResetCooldownTimer()
    {
        cooldownTimer = 0;
    }

    private void ResetComboTimer()
    {
        comboTimer = 0;
    }
}
