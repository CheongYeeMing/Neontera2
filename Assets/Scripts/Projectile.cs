using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    private Animator animator;

    public float damage;

    public IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Invincible")
        {
            StartCoroutine(Collide(collision.gameObject));
        }
    }

    public IEnumerator Collide(GameObject collidedObject)
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        animator.SetTrigger("explode");
        MobHealth mobHealth;
        if (collidedObject.TryGetComponent<MobHealth>(out mobHealth))
        {
            mobHealth.TakeDamage(damage);
            if (mobHealth.gameObject.GetComponent<MobHealth>().isDead && mobHealth.gameObject.GetComponent<MobReward>().rewardGiven == false)
            {
                Character character = FindObjectOfType<Character>();
                foreach (Quest quest in character.questList.quests)
                {
                    if (quest.questCriteria.criteriaType == CriteriaType.Kill)
                    {
                        if (quest.questCriteria.Target == mobHealth.gameObject.GetComponent<MobController>().mobName)
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
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}