using UnityEngine;

public class MobAttack : MonoBehaviour
{
    // Mob Animation States
    protected const string MOB_ATTACK = "Attack";

    [SerializeField] public float attackDelay;      // Mob attacking frequency
    [SerializeField] public float KnockbackX;
    [SerializeField] public float KnockbackY;
    [SerializeField] public float attack;           // Mob Damage

    protected MobAnimation mobAnimation;

    protected bool isAttacking;

    public virtual void Start()
    {
        mobAnimation = GetComponent<MobAnimation>();
    }

    // Mob Auto Attack
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            if (isAttacking == false)
            {
                isAttacking = true;
                mobAnimation.ChangeAnimationState(MOB_ATTACK);
            }
            collision.gameObject.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(attack);
            Invoke("AttackComplete", attackDelay);
        }
    }

    public void AttackComplete()
    {
        isAttacking = false;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
