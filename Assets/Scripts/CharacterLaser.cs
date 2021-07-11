using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLaser : Projectile
{
    [SerializeField] protected ParticleSystem particle;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Invincible")
        {
            if (collision.gameObject.layer == 11) Instantiate(particle, collision.gameObject.transform.position, transform.rotation);
            else Instantiate(particle, transform.position, transform.rotation);
            StartCoroutine(Collide(collision.gameObject));
        }
    }

    public override IEnumerator Collide(GameObject collidedObject)
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
        yield return new WaitForSeconds(0f);
        Destroy(gameObject);
    }
}
