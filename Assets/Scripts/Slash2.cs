using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash2 : MonoBehaviour
{
    [SerializeField] protected ParticleSystem particle;
    [SerializeField] public float lifetime;

    Animator animator;


    public virtual IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
