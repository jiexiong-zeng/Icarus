using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class CharacterController2D : MonoBehaviour
{
	private Rigidbody2D m_Rigidbody2D;
	private Animator animator;
	private PlayerMovement playermove;
	private Vector3 m_Velocity = Vector3.zero;
	public Tilemap tile;


	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private LayerMask m_WhatIsLadder;
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Transform m_WallCheck1;
	[SerializeField] private Transform m_WallCheck2;
	[Space]
	 

	public float initialGravity = 2f;
	public bool m_Grounded;            // Whether or not the player is grounded.
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	public bool falling;
	public float movementSmoothing = 0.05f;

	public bool atLadder;
	private float ladderPosX;
	private bool wasOnLadder;
	public bool atLedge;

    private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		playermove = GetComponent<PlayerMovement>();
		
	}

	private void FixedUpdate()
	{


		m_Grounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, .01f , m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
			}
		}

		atLedge = false;
		Collider2D[] wall1 = Physics2D.OverlapCircleAll(m_WallCheck1.position, .01f , m_WhatIsGround);
		Collider2D[] wall2 = Physics2D.OverlapCircleAll(m_WallCheck2.position, .01f, m_WhatIsGround);
		if (wall1.Length == 0 && wall2.Length > 0)
        {
			atLedge= true;
        }

		atLadder = false;
		
		Collider2D[] ladder = Physics2D.OverlapCircleAll(m_GroundCheck.position, 0.01f, m_WhatIsLadder);
		if (ladder.Length > 0)
        {
			atLadder = true;
			wasOnLadder = true;
			Vector3Int cellPosition = tile.WorldToCell(transform.position);
			ladderPosX = tile.GetCellCenterWorld(cellPosition).x;
			//ladderPosX = tile.GetLayoutCellCenter(cellPosition).x;
			//ladderPosX = ladder[0].gameObject.transform.position.x;
        }
		if (wasOnLadder && atLadder == false)
        {
			wasOnLadder = false;
			playermove.animationLocked = false;
			m_Rigidbody2D.gravityScale = initialGravity;
        }


	}


	public void Move(float speed)
	{ 
		Vector3 targetVelocity = new Vector2(speed*10f, m_Rigidbody2D.velocity.y);
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, movementSmoothing);

			// If the input is moving the player right and the player is facing left...
		if (speed > 0 && !m_FacingRight)
		{
			Flip();
		}
		else if (speed < 0 && m_FacingRight)
		{
			Flip();
		}
		
	}

	public void Jump()
    {
		m_Grounded = false;
		m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
	}

	public void ClimbLadder(bool up)
    {
		m_Rigidbody2D.gravityScale = 0;
		playermove.animationLocked = true;
		if (up)
		{
			transform.position = new Vector2(ladderPosX, transform.position.y + 0.1f);
		}
        else
        {
			transform.position = new Vector2(ladderPosX, transform.position.y - 0.1f);
		}
	}


	public void ClimbLedge()
    {
		if (m_FacingRight)
        {
			transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y + 0.7f);
		}
        else
        {
			transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y + 0.7f);
		}
    }

	public void Freeze()
    {
		m_Rigidbody2D.velocity = new Vector2(0, 0);
		m_Rigidbody2D.gravityScale = 0;
	}
	public void UnFreeze()
	{
		m_Rigidbody2D.gravityScale = initialGravity;
	}


	public void Stop()
    {
		m_Rigidbody2D.velocity = new Vector2(0, 0);
	}

	public void Dash(float dashSpeed)
    {
		m_Rigidbody2D.velocity = new Vector2(dashSpeed, 0);
	}

    private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
