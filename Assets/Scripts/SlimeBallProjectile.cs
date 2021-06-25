using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBallProjectile : SlimeProjectile
{
    // Start is called before the first frame update
    public override IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        lifetime = Random.Range(2, 20);
        speed = Random.Range(10, 20);
        GameObject slimeKing = FindObjectOfType<SlimeKingMovement>().gameObject;
        if (slimeKing.transform.localScale.x > 0) body.velocity = new Vector2(speed, 0);
        if (slimeKing.transform.localScale.x < 0) body.velocity = new Vector2(-speed, 0);
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            StartCoroutine(CollideCharacter(collision.gameObject));
        }
        else if (collision.gameObject.tag == "Invincible" && collision.gameObject.layer == 9)
        {
            CollideWall(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Invincible" && collision.gameObject.layer == 8)
        {
            CollideGround(collision.gameObject);
        }
    }
    
    public void CollideWall(GameObject wall)
    {
        body.velocity = -body.velocity;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    public void CollideGround(GameObject ground)
    {
        body.velocity = new Vector2(body.velocity.x,-body.velocity.y);
    }
}
