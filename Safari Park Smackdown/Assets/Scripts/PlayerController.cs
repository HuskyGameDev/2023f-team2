using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Abstract Player Controller
 * Handles generic control of the player, meant to be implemented by each individual character.
 * See character script for character specific controls, such as attacks.
 */
public abstract class PlayerController : MonoBehaviour
{
    [System.NonSerialized] public PlayerControls playerControls;
    [System.NonSerialized] public Rigidbody2D rb;
    [System.NonSerialized] public Animator animator;

    public bool isPlayer1;  //true = player 1, false = player 2

    //Health
    [System.NonSerialized] public float health;

    //Movement
    public float speed;
    public float jumpForce;
    [System.NonSerialized] public bool isJumping = false;   //whether or not the player is in the air
    [System.NonSerialized] public bool jump;      //whether or not the jump button started being pressed this frame
    [System.NonSerialized] public float horiz;    //value of the horizontal movement controls composite
    [System.NonSerialized] public bool canMove = true;  //whether or not the player is allowed to move
    [System.NonSerialized] public bool canJump = true;  //whether or not the player is allowed to jump, separate from 'isJumping'

    [System.NonSerialized] public bool action1;
    [System.NonSerialized] public bool action2;
    [System.NonSerialized] public bool action3;
    private float performingActionTime = 0;
    private float stunTime = 0;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void FixedUpdate()
    {
        SetControlVars();

        if(IsPerformingAction())
            performingActionTime -= Time.fixedDeltaTime;
        if(IsStunned())
            stunTime -= Time.fixedDeltaTime;

        CheckIfDead();
        Move();
        Attack();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = false;
        }
        else if (collision.gameObject.tag == "Attack")
        {
            Attack receivedAttack = collision.GetComponent<Attack>();
            if (receivedAttack != null)
            {
                health -= receivedAttack.GetDamage();
                Stun(receivedAttack.GetHitStun());
                Debug.Log("Player " + (isPlayer1 ? "1" : "2") + " took " + receivedAttack.GetDamage() + " damage (" + health + " health)");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = true;
        }
    }

    private void SetControlVars()
    {
        if (isPlayer1)
        {
            jump = playerControls.Player1.Jump.IsPressed();
            horiz = playerControls.Player1.Horizontal.ReadValue<Vector2>().x;
            action1 = playerControls.Player1.Action1.IsPressed();
            action2 = playerControls.Player1.Action2.IsPressed();
            action3 = playerControls.Player1.Action3.IsPressed();
        }
        else
        {
            jump = playerControls.Player2.Jump.IsPressed();
            horiz = playerControls.Player2.Horizontal.ReadValue<Vector2>().x;
            action1 = playerControls.Player2.Action1.IsPressed();
            action2 = playerControls.Player2.Action2.IsPressed();
            action3 = playerControls.Player2.Action3.IsPressed();
        }
    }

    private void Move()
    {
        if (jump && !isJumping && canJump)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        if (horiz != 0 && canMove)
        {
            animator.SetBool("Move", true);
            rb.velocity = new Vector2(horiz * speed * Time.deltaTime, rb.velocity.y);
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }

    public bool IsPerformingAction()
    {
        return performingActionTime > 0;
    }

    public void StartPerformingAction(float performingActionTime)
    {
        this.performingActionTime = Mathf.Max(this.performingActionTime, performingActionTime);
    }

    public bool IsStunned()
    {
        return stunTime > 0;
    }

    public bool Stun(float stunTime)
    {
        this.stunTime = Mathf.Max(this.stunTime, stunTime);
        return IsStunned();
    }

    private void CheckIfDead()
    {
        if(health <= 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            rb.rotation = 90;
            animator.enabled = false;
            this.enabled = false;
        }
    }

    public abstract void Attack();


}
