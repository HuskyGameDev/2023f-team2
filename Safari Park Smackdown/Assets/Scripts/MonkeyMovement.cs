using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MonkeyMovement : MonoBehaviour
{
    public float speed = 200f;
    public float jumpForce = 5f;
    public Rigidbody2D rb;
    bool isJumping;
    bool onPlatform;
    public Animator animator;
    public CapsuleCollider2D col;
    int health = 100;
    float jumpRate = 1.2f;
    float nextJumpTime = 0f;

    public bool isPlayer1 = true;
    public PlayerControls playerControls;
    bool jump;
    bool down;
    float horiz;
    bool action1;
    bool action2;
    bool action3;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<CapsuleCollider2D>();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void FixedUpdate()
    {
        SetControlVars();
        Move();
    }

    //Checking to see if the Monkey is on a platform or the ground
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Ground") {
            isJumping = false;
        }

        if (collision.gameObject.tag == "Platform") {
            onPlatform = true;
        }
    }

    //Checking to see if the Monkey is in the air jumping
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Ground") {
            isJumping = true;
        }

        if (collision.gameObject.tag == "Platform") {
            onPlatform = false;
        }
    }

    private void Move() {
        //Left and right movement
        if (!(horiz == 0)) {
            //animator.SetBool("Move", true);
            rb.velocity = new Vector2(horiz * speed * Time.deltaTime, rb.velocity.y);
        }
        else {
            //animator.SetBool("Move", false);
        }

        //Jumping 
        //if (jump && !isJumping)
        //{
        //    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        //}
        if (Time.time >= nextJumpTime)
        {
            //The basic punch attack for the character
            if (jump && !isJumping)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                nextJumpTime = Time.time + 1 / jumpRate;
            }
        }

        //Falling down platforms
        if (down && onPlatform) {
            StartCoroutine(dropDown());
        }
    }

    //Disables the collider to allow the Monkey to drop down and turns the collider back on before it reaches the ground
    private IEnumerator dropDown() {
        col.enabled = false;
        yield return new WaitForSeconds(0.3f);
        col.enabled = true;
    }

    //Damage function for the player to take damage when attacked
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    //Death function to visibily indicate character's death and disable movement and animations
    private void Die()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        rb.rotation = 90;
        animator.enabled = false;
        this.enabled = false;
    }

    private void SetControlVars() {
        if (isPlayer1) {
            jump = playerControls.Player1.Jump.IsPressed();
            down = playerControls.Player1.Down.IsPressed();
            horiz = playerControls.Player1.Horizontal.ReadValue<Vector2>().x;
            action1 = playerControls.Player1.Action1.IsPressed();
            action2 = playerControls.Player1.Action2.IsPressed();
            action3 = playerControls.Player1.Action3.IsPressed();
        }
        else {
            jump = playerControls.Player2.Jump.IsPressed();
            down = playerControls.Player2.Down.IsPressed();
            horiz = playerControls.Player2.Horizontal.ReadValue<Vector2>().x;
            action1 = playerControls.Player2.Action1.IsPressed();
            action2 = playerControls.Player2.Action2.IsPressed();
            action3 = playerControls.Player2.Action3.IsPressed();
        }
    }
}
