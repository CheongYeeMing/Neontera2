using UnityEngine;

public class FireWormAttack : MobAttack
{
    protected FireWormSummonerFireball fireballSummonFactory;
    protected float summonCooldown;
    protected float summonCooldownTimer;

    public override void Start()
    {
        fireballSummonFactory = gameObject.GetComponent<FireWormSummonerFireball>();
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
        summonCooldown = Random.Range(2f, 5f);
        fireballSummonFactory.Summon();
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

    public void SummonComplete()
    {
        isAttacking = false;
        summonCooldownTimer = 0f;
    }
}
