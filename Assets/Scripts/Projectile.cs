using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    private Animator animator;

    public float damage;

    public virtual IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Invincible")
        {
            StartCoroutine(Collide(collision.gameObject));
        }
    }

    public virtual IEnumerator Collide(GameObject collidedObject)
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        animator.SetTrigger("explode");
        MobHealth mobHealth;
        if (collidedObject.TryGetComponent<MobHealth>(out mobHealth))
        {
            if (!mobHealth.IsHurting())
            {
                mobHealth.SetAttackedBy(gameObject);
                mobHealth.TakeDamage(damage);
            }
            if (mobHealth.IsDead() && mobHealth.gameObject.GetComponent<MobReward>().GetIsRewardGiven() == false)
            {
                Character character = FindObjectOfType<Character>();
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
                mobHealth.gameObject.GetComponent<MobReward>().GetReward(character.GetComponent<CharacterLevel>(), character.GetComponent<CharacterWallet>());
            }
        }
        BossHealth bossHealth;
        if (collidedObject.TryGetComponent<BossHealth>(out bossHealth))
        {
            if (!bossHealth.IsHurting())
            {
                bossHealth.SetAttackedBy(gameObject);
                bossHealth.TakeDamage(damage);
            }
            if (bossHealth.IsDead() && bossHealth.gameObject.GetComponent<BossReward>().GetIsRewardGiven() == false)
            {
                Character character = FindObjectOfType<Character>();
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
                bossHealth.gameObject.GetComponent<BossReward>().GetReward(character.GetComponent<CharacterLevel>(), character.GetComponent<CharacterWallet>());
            }
        }
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}