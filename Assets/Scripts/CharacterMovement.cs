using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] public CharacterAttack characterAttack;

    private BoxCollider2D boxCollider;
    public Rigidbody2D body;

    private float wallJumpCooldown; // Prevent instant teleportation up wall
    private float horizontalInput;

    // Mob Animation States
    public const string CHARACTER_IDLE = "Idle";
    public const string CHARACTER_RUN = "Run";
    public const string CHARACTER_JUMP = "Jump";
    

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (CanMove() == false) return;

        horizontalInput = Input.GetAxis("Horizontal");

        // Flip player when moving
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 1);
        }

        // Set animator parameters
        //animator.SetBool("run", horizontalInput != 0);
        //animator.SetBool("grounded", isGrounded());
        if (isGrounded() && !characterAttack.isAttacking)
        {
            if (horizontalInput != 0)
            {
                gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_RUN);
            }
            else
            {
                gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_IDLE);
            }
        }

        // Wall Jump
        if (wallJumpCooldown > 0.2f)
        {
            // Player movement
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = new Vector2(transform.localScale.x, 0.3f);
            } else
            {
                body.gravityScale = 3;
            }
            // Player jump key
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        } 
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            //animator.SetTrigger("jump");
            gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_JUMP);
        }
        else if (onWall() && !isGrounded())
        {
            gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_JUMP);
            // Wall grab animation !!
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, jumpPower);
                transform.localScale= new Vector3(-Mathf.Sign(transform.localScale.x) * 0.3f, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }
            wallJumpCooldown = 0;
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.03f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.03f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return !onWall();
    }

    public bool CanMove()
    {
        bool can = true;
        if (FindObjectOfType<Interactable>().isExamining)
        {
            can = false;
        }
        if (FindObjectOfType<InventorySystem>().isOpen)
        {
            can = false;
        }
        if (gameObject.GetComponent<CharacterHealth>().isHurting || gameObject.GetComponent<CharacterHealth>().isDead)
        {
            can = false;
        }
        DialogueManager[] npc = FindObjectsOfType<DialogueManager>();
        for (int i = 0; i < npc.Length; i++)
        {
            if (npc[i].isTalking)
            {
                can = false;
                break;
            }
        }
        return can;
    }
}
