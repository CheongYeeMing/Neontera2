using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] ParticleSystem jumpDust;
    [SerializeField] ParticleSystem walkDust;

    private BoxCollider2D boxCollider;
    private Rigidbody2D body;

    private float wallJumpCooldown; // Prevent instant teleportation up wall
    private float horizontalInput;

    // Location
    public string location;

    // Mob Animation States
    private const string CHARACTER_IDLE = "Idle";
    private const string CHARACTER_RUN = "Run";
    private const string CHARACTER_JUMP = "Jump";


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Start()
    {
        location = Data.location;
        gameObject.transform.position = new Vector2(Data.Xcoordinate,Data.Ycoordinate);
    }

    private void FixedUpdate()
    {
        if (isGrounded()) walkDust.gameObject.SetActive(true);
        else walkDust.gameObject.SetActive(false);
        speed = GetComponent<Character>().GetSpeed().CalculateFinalValue();
        if (CanMove() == false)
        {
            if (!GetComponent<CharacterAttack>().GetIsAttacking()) GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_IDLE);
            //body.velocity = Vector2.zero;
            return;
        }
        horizontalInput = Input.GetAxis("Horizontal");

        // Audio
        if (isGrounded() && horizontalInput != 0)
        {
            FindObjectOfType<AudioManager>().PlayEffect("Run");
        }
        else FindObjectOfType<AudioManager>().StopEffect("Run");
        if (isGrounded()) FindObjectOfType<AudioManager>().StopEffect("Jump");

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
        if (isGrounded() && !GetComponent<CharacterAttack>().GetIsAttacking())
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

    public void CreateDust()
    {
        jumpDust.Play();
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            //animator.SetTrigger("jump");
            gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_JUMP);
            CreateDust();
            FindObjectOfType<AudioManager>().PlayEffect("Jump");
        }
        else if (onWall() && !isGrounded())
        {
            gameObject.GetComponent<CharacterAnimation>().ChangeAnimationState(CHARACTER_JUMP);
            // Wall grab animation !!
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, jumpPower);
                transform.localScale= new Vector3(-Mathf.Sign(transform.localScale.x) * 0.3f, transform.localScale.y, transform.localScale.z);
                CreateDust();
                FindObjectOfType<AudioManager>().PlayEffect("Jump");
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
        return !onWall() && !GetComponent<CharacterHealth>().IsDead();
    }

    public bool CanMove()
    {
        bool can = true;
        Monologue[] monologues = FindObjectsOfType<Monologue>();
        foreach(Monologue mono in monologues)
        {
            if (mono.IsExamining())
            {
                can = false;
                break;
            }
        }
        if (FindObjectOfType<InventorySystem>().isOpen)
        {
            can = false;
        }
        if (gameObject.GetComponent<CharacterHealth>().IsHurting() || gameObject.GetComponent<CharacterHealth>().IsDead())
        {
            can = false;
        }
        if (GetComponent<CharacterAttack>().GetIsAttacking()) can = false;
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

    public Rigidbody2D GetRigidBody()
    {
        return body;
    }
}
