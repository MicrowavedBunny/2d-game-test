using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;
	[SerializeField] private Transform playerPosition;
	[SerializeField] private GameObject complete;
	[SerializeField] private GameObject check;
	
	
	//[SerializeField] private RaycastHit m_WallCheck;// A position marking where to check for ceilings

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D; //instantiate ridgidbody2d
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	//level storage
	public int currentLevel = 0;
	public int[] unlockedLevels;

	//wall jumping raycast variables
	private bool leftWall = false;
	private bool rightWall = false;
	
	//can double jump and has double jumped checks
	private bool canDoubleJump;
	private bool doubleJumped = false;
	
	//player animator controller
	public Animator animator;

	//animator controller for checkpoints
	public Animator checkpoint;

	private bool checkPointHit = false;

	//check for levelcomplete
	private bool levelComplete = false;

	//particles for dust effect
	public ParticleSystem dust;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		//complete = completeLevel.trigger;
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

		//raycast for walls
		//////////////////////
		/////initialize layermask
		LayerMask terrain = LayerMask.GetMask("ForegroundTerrain");

		RaycastHit2D m_WallCheckRight = Physics2D.Raycast(playerPosition.transform.position, -Vector2.left, .45f, terrain);

		RaycastHit2D m_WallCheckLeft = Physics2D.Raycast(playerPosition.transform.position, -Vector2.right, .45f, terrain);

		if (m_WallCheckRight.collider != null)
		{
			rightWall = true;
		}
		else
		{
			rightWall = false;
		}

		if (m_WallCheckLeft.collider != null)
		{
			leftWall = true;
		}
		else
		{
			leftWall = false;
		}

		//keep current level up to date
		currentLevel = SceneManager.GetActiveScene().buildIndex;

		//check if level is complete
		if (levelComplete)
		{
			//freeze player
			Invoke("freezePlayer", .15f);

			//sets next level to current level then saves the game
			currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
			SavePlayer();
			//waits 3 seconds then switches to next level
			Invoke("nextLevel", 3f);
		}

	}

	//freezeplayer method
	private void freezePlayer()
	{
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
	}
	
	//switches to next level
	private void nextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	//on finish trigger set levelComplete to true
	private void OnTriggerEnter2D(Collider2D collider)
	{

		if (collider.gameObject == check)
		{
			checkpoint.SetBool("CheckPoint", true);
			checkPointHit = true;
			Invoke("resetCheckpoint", 1.17f);
		}

		if (collider.gameObject == complete) 
		{
			levelComplete = true;
		}
	}

	private void resetCheckpoint()
	{
		checkpoint.SetBool("CheckPoint", false);
	}

	public void Move(float move, bool jump)
	{

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded)
		{
			canDoubleJump = true;
		}

		if (jump == true)
		{
			if (m_Grounded == true)
			{
				m_Grounded = false;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				createDust();
			}
			else
			{
				if (canDoubleJump == true)
				{
					//actual jump
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
					m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
					
					canDoubleJump = false;

					//animation
					doubleJumped = true;
					Invoke("resetDoubleJump", .5f);
				}
			}
		}
	}

	//timer for double jump animation
	private void resetDoubleJump()
	{
		doubleJumped = false;
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		if (m_Grounded == true)
		{
			createDust();
		}
	}

	void createDust()
	{
		dust.Play();
	}

	public void onLand()
	{
		dust.Play();
	}

	public bool getDoubleJump()
	{
		return doubleJumped;
	}
	
	//saving
	public void SavePlayer()
	{
		SaveSystem.savePlayer(this);
	}

	public bool getCheckPointHit()
	{
		return checkPointHit;
	}

}