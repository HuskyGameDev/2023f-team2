using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonkeyMovement : MonoBehaviour
{
    public float speed = 100f;
    public float jumpForce = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    bool isJumping;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {

        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //animator.SetFloat("Horizontal", movement.x);
        //animator.SetFloat("Vertical", movement.y);
        //animator.SetFloat("Speed", movement.sqrMagnitude);
        
        //Jumping
        if (Input.GetKeyDown(KeyCode.W) && !isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        //Blocking
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("Block", true);
        }
        else if (Input.GetKeyUp(KeyCode.Z)) {
            animator.SetBool("Block", false);
        }

        //Jabbing
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("Jab", true);
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            animator.SetBool("Jab", false);
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * speed * Time.deltaTime, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform") {
            isJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = true;
        }
    }
}
