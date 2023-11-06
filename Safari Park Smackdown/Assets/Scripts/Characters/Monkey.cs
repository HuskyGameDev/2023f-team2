using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : PlayerController
{
    [SerializeField] private GameObject jabCollider;

    protected override void Awake()
    {
        base.Awake();
        maxHealth = 80;
    }

    public override void Attack()
    {
        if (!IsStunned() && !IsPerformingAction())
        {
            //stop doing old things
            RefreshAttacks();

            //do new things
            if (action1)
            {
                Jab();
            }
            if (action2 && !isJumping)
            {
                Block();
            }
            else
            {
                canJump = true;
                canMove = true;
            }
        }
    }

    void Jab()
    {
        jabCollider.SetActive(true);
        animator.SetBool("Jab", true);

        StartPerformingAction(.5f);
    }

    void Block()
    {
        animator.SetBool("Block", true);

        canJump = false;
        canMove = false;
        StartPerformingAction(.25f);
    }

    void RefreshAttacks()
    {
        jabCollider.SetActive(false);

        animator.SetBool("Jab", false);
        animator.SetBool("Block", false);
    }
}
