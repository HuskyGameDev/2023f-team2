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
    public static PlayerController Player1;
    public static PlayerController Player2;

    //Health
    [System.NonSerialized] public float health;
    [System.NonSerialized] public float maxHealth;  //this MUST be initialized in an override Awake() function of inherited members, see Monkey.cs

    //Movement
    public float speed;
    public float jumpForce;
    [System.NonSerialized] public bool isJumping = false;   //whether or not the player is in the air
    [System.NonSerialized] public bool jump;      //whether or not the jump button started being pressed this frame
    [System.NonSerialized] public float horiz;    //value of the horizontal movement controls composite
    [System.NonSerialized] public bool canMove = true;  //whether or not the player is allowed to move
    [System.NonSerialized] public bool canJump = true;  //whether or not the player is allowed to jump, separate from 'isJumping'
    [System.NonSerialized] public bool hasJump = false;

    [System.NonSerialized] public bool action1;
    [System.NonSerialized] public bool action2;
    [System.NonSerialized] public bool action3;
    private float performingActionTime = 0;
    private float stunTime = 0;

    [System.NonSerialized] public bool hasBlock = false;    //if the character has unlocked the block ability
    [System.NonSerialized] public bool isBlocking = false;

    //Upgrades
    public List<GameObject> possibleUpgradeList;
    [System.NonSerialized] public List<GameObject> currentUpgradeList = new List<GameObject>();


    protected virtual void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (isPlayer1)
            Player1 = this;
        else 
            Player2 = this;
    }

    private void OnEnable()
    {
        playerControls.Enable();
        health = maxHealth;

        playerControls.Player1.Pause.Enable();
        playerControls.Player1.Pause.started += ctx => { if (!GameController.isPaused) { GameController.Pause(); GameController.gc.pauseMenu.SetActive(true); } };
    }

    private void FixedUpdate()
    {
        
        SetControlVars();

        //cool the cooldowns
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
                float dmg = receivedAttack.GetDamage() * (isBlocking ? .5f : 1);
                health -= dmg;
                Stun(receivedAttack.GetHitStun());
                Debug.Log("Player " + (isPlayer1 ? "1" : "2") + " took " + dmg + " damage (" + health + " health)");
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

    public static void CreateUpgradeCards()
    {
        int[] used1 = new int[2];
        int[] used2 = new int[2];
        for (int i = 1; i <= 3; i++)
        {
            int player1CardIndex = Random.Range(0, Player1.possibleUpgradeList.Count);
            GameObject player1Card = Instantiate(Player1.possibleUpgradeList[player1CardIndex], Vector3.left * 2.5f * i, Quaternion.identity);
            UpgradeController player1CardController = player1Card.GetComponent<UpgradeController>();
            player1CardController.self = Player1.possibleUpgradeList[player1CardIndex]; //this lets the upgrade card know which prefab it was created from
            player1CardController.isForPlayer1 = true;


            int player2CardIndex = Random.Range(0, Player2.possibleUpgradeList.Count);
            GameObject player2Card = Instantiate(Player2.possibleUpgradeList[player2CardIndex], Vector3.right * 2.5f * i, Quaternion.identity);
            UpgradeController player2CardController = player2Card.GetComponent<UpgradeController>();
            player2CardController.self = Player2.possibleUpgradeList[player2CardIndex]; //this lets the upgrade card know which prefab it was created from
            player2CardController.isForPlayer1 = false;
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
        if (jump && !isJumping && canJump && hasJump)
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

    public float Stun(float stunTime)
    {
        if(!isBlocking)
            this.stunTime = Mathf.Max(this.stunTime, stunTime);
        return this.stunTime;
    }

    private void CheckIfDead()
    {
        if(health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public void ResetPlayer()
    {
        health = maxHealth;
        GetComponent<SpriteRenderer>().color = Color.white;
        rb.rotation = 0;
        animator.enabled = true;
        this.enabled = true;
        StopCoroutine(Die());

        transform.position =  Vector3.right * (isPlayer1 ? -4 : 4);
    }

    IEnumerator Die()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        rb.rotation = 90;
        rb.velocity = Vector3.zero;
        animator.enabled = false;
        this.enabled = false;
        yield return new WaitForSeconds(2);
        GameController.EndRound();
    }

    public abstract void Attack();


}
