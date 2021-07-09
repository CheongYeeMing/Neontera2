using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupremeLeaderDeathRay : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected Rigidbody2D body;

    protected float lifetime;

    // Start is called before the first frame update
    public virtual IEnumerator Start()
    {
        lifetime = 0.5f;
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            StartCoroutine(CollideCharacter(collision.gameObject));
        }
        else if (collision.gameObject.tag == "Invincible" && collision.gameObject.layer == 8)
        {
            CollideGround(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Invincible" && collision.gameObject.layer == 9)
        {
            CollideWall(collision.gameObject);
        }
    }

    public virtual IEnumerator CollideCharacter(GameObject character)
    {
        //Particles
        character.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
        character.GetComponent<CharacterHealth>().TakeDamage(damage);
        yield return new WaitForSeconds(0.1f);
    }

    public void CollideGround(GameObject ground)
    {
        //Particles
    }

    public void CollideWall(GameObject ground)
    {
        //Particles
    }
}
