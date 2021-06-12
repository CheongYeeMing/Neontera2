using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    private Animator animator;

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
            StartCoroutine(Collide());
        }
    }

    public IEnumerator Collide()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        animator.SetTrigger("explode");
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}