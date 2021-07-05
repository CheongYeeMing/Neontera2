using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRainProjectile : SlimeProjectile
{
    // Start is called before the first frame update
    public override IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        lifetime = 5f;
        yield return new WaitForSeconds(lifetime);
        animator.SetTrigger("Explode");
        Destroy(gameObject);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            Debug.Log("Collided");
            StartCoroutine(CollideCharacter(collision.gameObject));
        }
        else if (collision.gameObject.tag == "Invincible" && collision.gameObject.layer == 8)
        {
            CollideGround(collision.gameObject);
        }
    }

    public override IEnumerator CollideCharacter(GameObject character)
    {
        body.velocity = Vector2.zero;
        animator.SetTrigger("Explode");
        character.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
        character.GetComponent<CharacterHealth>().TakeDamage(damage);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    public IEnumerator CollideGround(GameObject ground)
    {
        body.velocity = new Vector2(0,0);
        animator.SetTrigger("Explode");
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
