using UnityEngine;

public class RangedMobAttack : MobAttack
{
    [SerializeField] protected float cooldownMin;
    [SerializeField] protected float cooldownMax;

    protected MobSummoner summonFactory;

    protected float summonCooldown;
    protected float summonCooldownTimer;

    public override void Start()
    {
        mobAnimation = GetComponent<MobAnimation>();
        summonFactory = gameObject.GetComponent<MobSummoner>();
        summonCooldown = 5f;
        summonCooldownTimer = 0f;
    }

    // Update is called once per frame
    public void Update()
    {
        if (GetComponent<MobHealth>().IsDead()) return;
        if (!GetComponent<MobPathfindingAI>().GetIsChasingTarget()) return;
        if (summonCooldownTimer > summonCooldown && isAttacking == false)
        {
            Attack();
        }
        summonCooldownTimer += Time.deltaTime;
    }

    public void Attack()
    {
        isAttacking = true;
        GetComponent<MobMovement>().StopPatrol();
        summonCooldown = Random.Range(cooldownMin, cooldownMax);
        summonFactory.Summon();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            isAttacking = true;
            GetComponent<MobMovement>().StopPatrol();
            Debug.Log("attack animation called");
            GetComponent<MobAnimation>().ChangeAnimationState(MOB_ATTACK);
            collision.gameObject.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
            Invoke("AttackComplete", attackDelay);
        }
    }

    // To be called by summonFactory upon success invocation
    public void SummonComplete()
    {
        isAttacking = false;
        summonCooldownTimer = 0f;
    }
}
