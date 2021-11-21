using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Mob Animation States
    private const string CHARACTER_IDLE = "Idle";
    private const string CHARACTER_RUN = "Run";
    private const string CHARACTER_JUMP = "Jump";

    private const float BOXCAST_ANGLE = 0;
    private const float BOXCAST_DISTANCE = 0.03f;
    private const float WALL_JUMP_COOLDOWN = 0.2f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] ParticleSystem jumpDust;
    [SerializeField] ParticleSystem walkDust;

    private BoxCollider2D boxCollider;
    private Rigidbody2D body;

    private Character character;
    private CharacterAnimation characterAnimation;
    private CharacterAttack characterAttack;
    private CharacterHealth characterHealth;

    private float wallJumpTimer; // Prevent instant teleportation up wall
    private float horizontalInput;

    // Location
    public string location;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        character = GetComponent<Character>();
        characterAnimation = GetComponent<CharacterAnimation>();
        characterAttack = GetComponent<CharacterAttack>();
        characterHealth = GetComponent<CharacterHealth>();
    }

    private void Start()
    {
        location = Data.location;
        gameObject.transform.position = new Vector2(Data.Xcoordinate,Data.Ycoordinate);
    }

    private void FixedUpdate()
    {
        UpdateWalkDustParticle();
        UpdateSpeed();
        if (characterAttack.IsAttacking() && IsGrounded()) return;
        if (IsAbleToMove() == false)
        {
            characterAnimation.ChangeAnimationState(CHARACTER_IDLE);
            return;
        }
        UpdateHorizontalInput();
        UpdateMovementAudio();
        UpdateFacingDirection();

        // Set animator parameters
        //animator.SetBool("run", horizontalInput != 0);
        //animator.SetBool("grounded", isGrounded());
        if (IsGrounded() && !characterAttack.IsAttacking())
        {
            if (IsMoving())
            {
                characterAnimation.ChangeAnimationState(CHARACTER_RUN);
            }
            else
            {
                characterAnimation.ChangeAnimationState(CHARACTER_IDLE);
            }
        }

        // Wall Jump
        if (IsAbleToWallJump())
        {
            // Player movement
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (IsOnWall() && !IsGrounded())
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
    }

    private void UpdateMovementAudio()
    {
        if (IsGrounded() && IsMoving())
        {
            FindObjectOfType<AudioManager>().PlayEffect("Run");
        }
        else FindObjectOfType<AudioManager>().StopEffect("Run");
        if (IsGrounded())
        {
            FindObjectOfType<AudioManager>().StopEffect("Jump");
        }
    }

    private void UpdateSpeed()
    {
        speed = character.GetSpeed().CalculateFinalValue();
    }

    private void UpdateFacingDirection()
    {
        // Flip player to match facing direction when moving
        if (horizontalInput > 0.01f) // Character facing right
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 1);
        }
        else if (horizontalInput < -0.01f) // Character facing left
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 1); // Flip the x scale
        }
    }

    private void UpdateHorizontalInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private bool IsMoving()
    {
        return horizontalInput != 0;
    }

    private bool IsAbleToWallJump()
    {
        if (wallJumpTimer > WALL_JUMP_COOLDOWN)
        {
            return true;
        }
        else
        {
            wallJumpTimer += Time.deltaTime;
            return false;
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            characterAnimation.ChangeAnimationState(CHARACTER_JUMP);
            CreateDust();
            FindObjectOfType<AudioManager>().PlayEffect("Jump");
        }
        else if (IsOnWall() && !IsGrounded())
        {
            characterAnimation.ChangeAnimationState(CHARACTER_JUMP);
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
            wallJumpTimer = 0;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size,
            BOXCAST_ANGLE,
            Vector2.down,
            BOXCAST_DISTANCE,
            groundLayer);
        return raycastHit.collider != null;
    }

    private bool IsOnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size,
            BOXCAST_ANGLE,
            new Vector2(transform.localScale.x, 0),
            BOXCAST_DISTANCE,
            wallLayer);
        return raycastHit.collider != null;
    }

    public bool IsAbleToAttack()
    {
        //bool can = true;
        //Monologue[] monologues = FindObjectsOfType<Monologue>();
        //foreach (Monologue mono in monologues)
        //{
        //    if (mono.IsExamining())
        //    {
        //        can = false;
        //        break;
        //    }
        //}
        //if (FindObjectOfType<InventorySystem>().isOpen)
        //{
        //    can = false;
        //}
        //if (gameObject.GetComponent<CharacterHealth>().IsHurting() || gameObject.GetComponent<CharacterHealth>().IsDead())
        //{
        //    can = false;
        //}
        return !IsOnWall() && !characterHealth.IsDead();
    }

    public bool IsAbleToMove()
    {
        bool can = true;
        if (IsInMonologue())
        {
            can = false;
        }
        if (FindObjectOfType<InventorySystem>().isOpen)
        {
            can = false;
        }
        if (characterHealth.IsHurting() || characterHealth.IsDead())
        {
            can = false;
        }
        if (IsInDialogue())
        {
            can = false;
        }
        return can;
    }

    private bool IsInMonologue()
    {
        bool inMonologue = false;
        Monologue[] monologues = FindObjectsOfType<Monologue>();
        foreach (Monologue mono in monologues)
        {
            if (mono.IsExamining())
            {
                inMonologue = true;
                break;
            }
        }
        return inMonologue;
    }

    private bool IsInDialogue()
    {
        bool inDialogue = false;
        DialogueManager[] npc = FindObjectsOfType<DialogueManager>();
        for (int i = 0; i < npc.Length; i++)
        {
            if (npc[i].isTalking)
            {
                inDialogue = true;
                break;
            }
        }
        return inDialogue;
    }

    public Rigidbody2D GetRigidBody()
    {
        return body;
    }

    // Methods for Particle System
    private void CreateDust()
    {
        jumpDust.Play();
    }

    private void UpdateWalkDustParticle()
    {
        if (IsGrounded())
        {
            walkDust.gameObject.SetActive(true);
        }
        else
        {
            walkDust.gameObject.SetActive(false);
        }
    }
}
