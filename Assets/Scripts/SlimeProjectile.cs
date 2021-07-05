using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected Animator animator;
    protected Rigidbody2D body;

    protected float lifetime;
    protected float speed;

    // Start is called before the first frame update
    public virtual IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        lifetime = Random.Range(2, 20);
        speed = Random.Range(10, 20);
        if (body.velocity.x < 0) transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            StartCoroutine(CollideCharacter(collision.gameObject));
        }
    }

    public virtual IEnumerator CollideCharacter(GameObject character)
    {
        body.velocity = Vector2.zero;
        animator.SetTrigger("Explode");
        character.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
        character.GetComponent<CharacterHealth>().TakeDamage(damage);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
