using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private const string AUDIO_JUMP = "Jump";
    private const string AUDIO_RUN = "Run";
    private const float BOXCAST_ANGLE = 0;
    private const float BOXCAST_DISTANCE = 0.03f;
    private const string CHARACTER_IDLE = "Idle";
    private const string CHARACTER_RUN = "Run";
    private const string CHARACTER_JUMP = "Jump";
    private const float CHARACTER_X = 0.3f;
    private const float CHARACTER_Y = 0.3f;
    private const float CHARACTER_Z = 1;
    private const float GRAVITY_SCALE_ZERO = 0;
    private const float GRAVITY_SCALE_NORMAL = 3;
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const float MINIMUM_HORIZONTAL_LEFT_INPUT = -0.01f;
    private const float MINIMUM_HORIZONTAL_RIGHT_INPUT = 0.01f;
    private const float WALL_JUMP_COOLDOWN = 0.2f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] ParticleSystem jumpDust;
    [SerializeField] ParticleSystem walkDust;

    private BoxCollider2D boxCollider;
    private Character character;
    private CharacterAnimation characterAnimation;
    private CharacterAttack characterAttack;
    private CharacterHealth characterHealth;
    private Rigidbody2D body;

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
        transform.position = new Vector2(Data.Xcoordinate,Data.Ycoordinate);
    }

    private void FixedUpdate()
    {
        UpdateWalkDustParticle();
        UpdateSpeed();
        if (characterAttack.IsAttacking() && IsGrounded()) return;
        if (!IsAbleToMove())
        {
            characterAnimation.ChangeAnimationState(CHARACTER_IDLE);
            return;
        }
        UpdateHorizontalInput();
        UpdateMovementAudio();
        UpdateFacingDirection();

        // Set animator parameters
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
                body.gravityScale = GRAVITY_SCALE_ZERO;
                body.velocity = new Vector2(transform.localScale.x, CHARACTER_Y);
            } else
            {
                body.gravityScale = GRAVITY_SCALE_NORMAL;
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
            FindObjectOfType<AudioManager>().PlayEffect(AUDIO_RUN);
        }
        else FindObjectOfType<AudioManager>().StopEffect(AUDIO_RUN);
        if (IsGrounded())
        {
            FindObjectOfType<AudioManager>().StopEffect(AUDIO_JUMP);
        }
    }

    private void UpdateSpeed()
    {
        speed = character.GetSpeed().CalculateFinalValue();
    }

    private void UpdateFacingDirection()
    {
        // Flip player to match facing direction when moving
        if (horizontalInput > MINIMUM_HORIZONTAL_RIGHT_INPUT) // Character facing right
        {
            transform.localScale = new Vector3(CHARACTER_X, CHARACTER_Y, CHARACTER_Z);
        }
        else if (horizontalInput < MINIMUM_HORIZONTAL_LEFT_INPUT) // Character facing left
        {
            transform.localScale = new Vector3(-CHARACTER_X, CHARACTER_Y, CHARACTER_Z); // Flip the x scale
        }
    }

    private void UpdateHorizontalInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL_AXIS);
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
            FindObjectOfType<AudioManager>().PlayEffect(AUDIO_JUMP);
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
                FindObjectOfType<AudioManager>().PlayEffect(AUDIO_JUMP);
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
        bool isOnGround = raycastHit.collider != null;
        return isOnGround;
    }

    private bool IsOnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size,
            BOXCAST_ANGLE,
            new Vector2(transform.localScale.x, 0),
            BOXCAST_DISTANCE,
            wallLayer);
        bool isOnWall = raycastHit.collider != null;
        return isOnWall;
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
