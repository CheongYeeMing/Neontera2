using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaGolemLaserBeam : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected Rigidbody2D body;

    protected float lifetime;
    protected float speed;

    // Start is called before the first frame update
    public virtual IEnumerator Start()
    {
        body = GetComponent<Rigidbody2D>();
        lifetime = 1.3f;
        Debug.Log(transform.localScale.x);
        //if (GameObject.FindGameObjectWithTag("Character").transform.position.x < transform.position.x && transform.localScale.x > 0) 
        //    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        Debug.Log(GameObject.FindGameObjectWithTag("Character").transform.position.x);
        Debug.Log(transform.position.x);
        Debug.Log(transform.localScale.x);
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
        yield return new WaitForSeconds(0.2f);
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
