using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    //walljumping variables
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;
    //************************/

    public CharacterController2D controller;

    public Animator animator;

    public float runSpeed = 40f;

    private Rigidbody2D m_Rigidbody2D;

    float horizontalMove = 0f;
    bool jump = false;

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        animator.SetBool("IsJumping", jumpingUp());
        animator.SetBool("IsFalling", fallingDown());
        animator.SetBool("isDoubleJumping", controller.getDoubleJump());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }


        //walljumping code

        //////////////////

    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    bool jumpingUp()
	{
        bool goingUp = false;

        if (m_Rigidbody2D.velocity.y > 0)
            goingUp = true;

        return goingUp;
	}

    bool fallingDown()
    {
        bool goingDown = false;

        if (m_Rigidbody2D.velocity.y < 0)
            goingDown = true;

        return goingDown;
    }

}
