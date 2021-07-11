using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFloorProjectile : SlimeProjectile
{
    [SerializeField] protected ParticleSystem particle;
    // Start is called before the first frame update
    public override IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        lifetime = 0.3f;
        speed = 50f;
        body.velocity = new Vector2(0, speed);
        yield return new WaitForSeconds(.02f);
        body.velocity = Vector2.zero;
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            Instantiate(particle, collision.gameObject.transform.position, transform.rotation);
            StartCoroutine(CollideCharacter(collision.gameObject));
        }
    }

    public override IEnumerator CollideCharacter(GameObject character)
    {
        body.velocity = Vector2.zero;
        character.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
        character.GetComponent<CharacterHealth>().TakeDamage(damage);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
