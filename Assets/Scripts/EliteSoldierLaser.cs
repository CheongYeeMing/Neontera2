using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteSoldierLaser : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected Rigidbody2D body;

    protected float lifetime;
    protected float speed;

    // Start is called before the first frame update
    public virtual IEnumerator Start()
    {
        body = GetComponent<Rigidbody2D>();
        lifetime = Random.Range(10, 15);
        speed = Random.Range(10, 20);
        Transform target = GameObject.FindGameObjectWithTag("Character").transform;
        Vector3 direction = target.position - transform.position;
        body.AddForce(direction * 200);
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
        body.velocity = Vector2.zero;
        character.GetComponent<CharacterHealth>().SetAttackedBy(gameObject);
        character.GetComponent<CharacterHealth>().TakeDamage(damage);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    public void CollideGround(GameObject ground)
    {
        //body.velocity = new Vector2(body.velocity.x, -body.velocity.y);
        Destroy(gameObject);
    }

    public void CollideWall(GameObject ground)
    {
        //body.velocity = new Vector2(-body.velocity.x, body.velocity.y);
        Destroy(gameObject);
    }
}
