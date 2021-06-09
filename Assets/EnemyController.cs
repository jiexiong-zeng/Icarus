using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	private Rigidbody2D m_Rigidbody2D;
	private CapsuleCollider2D Collider;

	//slope stuff
	private float slopeDownAngle;
	private Vector2 slopeNormalPerp;
	private float slopeDownAngleOld;
	public bool isOnSlope;

	[SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private Transform m_GroundCheck;
	[SerializeField] private Transform m_GapCheck;


	public float maxSpeed = 10f;
	public float initialGravity = 2f;
	public bool m_Grounded;            // Whether or not the player is grounded.
	public bool gap;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	public bool falling;
	public float movementSmoothing = 0.05f;

	private Vector3 targetVelocity;

	public bool startFaceRight = true;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		Collider = GetComponent<CapsuleCollider2D>();
		m_Rigidbody2D.gravityScale = initialGravity;
	}

    void Start()
    {
        if (!startFaceRight)
        {
			Flip();
        }
    }

    private void Update()
	{

		m_Grounded = false;
		RaycastHit2D hit_mid;
		hit_mid = Physics2D.Raycast(m_GroundCheck.transform.position, Vector2.down, 0.1f, m_WhatIsGround);
		if (hit_mid.collider != null)
		{
			m_Grounded = true;
		}

		gap = true;
		RaycastHit2D hit_front;
		hit_front = Physics2D.Raycast(m_GapCheck.transform.position, Vector2.down, 1f, m_WhatIsGround);
		if (hit_front.collider != null)
		{
			gap = false;
		}


	}
	void FixedUpdate()
	{
		SlopeCheck();
		m_Rigidbody2D.velocity = Vector2.ClampMagnitude(m_Rigidbody2D.velocity, maxSpeed);
	}

	private void SlopeCheck()
	{
		//Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);
		SlopeCheckVertical(m_GroundCheck.position);
	}


	private void SlopeCheckVertical(Vector2 checkPos)
	{
		RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, 0.5f, m_WhatIsGround);
		if (hit)
		{
			slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
			slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

			if (slopeDownAngle != slopeDownAngleOld)
			{
				isOnSlope = true;
			}

			slopeDownAngleOld = slopeDownAngle;
			Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
			Debug.DrawRay(hit.point, hit.normal, Color.green);

			if(slopeDownAngle == 0)
            {
				isOnSlope = false;
            }



		}

	}


	public void Move(float speed)
	{
		float multiplier = 10f;

		if (isOnSlope && m_Grounded)
		{
			targetVelocity = new Vector2(-speed * multiplier * slopeNormalPerp.x, -speed * multiplier * slopeNormalPerp.y);
			m_Rigidbody2D.velocity = targetVelocity;

		}
		else
		{
			targetVelocity = new Vector2(speed * multiplier, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = targetVelocity;

		}
		
		if (speed > 0 && !m_FacingRight)
		{
			Flip();
		}
		else if (speed < 0 && m_FacingRight)
		{
			Flip();
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
		m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
	}

	public void Dash(float dashSpeed)
	{
		m_Rigidbody2D.velocity = new Vector2(dashSpeed, 0);
	}

	public void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
