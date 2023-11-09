using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonkeyCombat : MonoBehaviour
{
    public Animator animator;
    int damage = 10;
    public Collider2D[] attackHitboxes;
    float attackRate = 3f;
    float nextAttackTime = 0f;

    public bool isPlayer1 = true;
    public PlayerControls playerControls;
    bool jump;
    bool down;
    float horiz;
    bool action1;
    bool action2;
    bool action3;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void FixedUpdate() {
        SetControlVars();
        Block();
        //Prevents user from spamming attack key
        if (Time.time >= nextAttackTime) {
            //The basic punch attack for the character
            if (action2)
            {
                Attack(attackHitboxes[0]);
                nextAttackTime = Time.time + 1 / attackRate;
            }
        }
    }

    //Attack function
    private void Attack(Collider2D attackArea)
    {
        animator.SetTrigger("Jab");
        var hit = Physics2D.OverlapCapsule(attackArea.bounds.center, attackArea.bounds.extents, CapsuleDirection2D.Horizontal, LayerMask.GetMask("Hitbox"));
        if (!(hit.transform == transform || hit.transform.parent == transform)) {
            hit.SendMessageUpwards("TakeDamage", damage);
        }
    }

    //Block attack, not yet coded
    private void Block() {
        if (action1) {
            animator.SetBool("Block", true);
        } 
        else {
            animator.SetBool("Block", false);
        }
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
