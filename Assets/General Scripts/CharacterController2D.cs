using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
	private Rigidbody2D m_Rigidbody2D;
	//private PlayerMovement playermove;
	private CapsuleCollider2D Collider;
	//private Vector3 m_Velocity = Vector3.zero;
	//public Tilemap tile;
	private GameObject oneWayPlatform;

	//slope stuff
	private Vector2 colliderSize;
	private float slopeDownAngle;
	private Vector2 slopeNormalPerp;
	private float slopeDownAngleOld;
	private bool isOnSlope;
	



	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_GroundCheckLeft;
	[SerializeField] private Transform m_GroundCheckRight;
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Transform m_WallCheck1;
	[SerializeField] private Transform m_WallCheck2;

	public float maxSpeed = 10f;
	public float initialGravity = 2f;
	public bool m_Grounded;            // Whether or not the player is grounded.
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	public bool falling;
	public float movementSmoothing = 0.05f;

	public bool atLadder;
	public bool atLedge;
	private Vector3 targetVelocity;
	public bool isJumping = false;

	public bool pushedBack = false;
	public Vector3 pushBackDirection;
	public float pushBackSpeed;

	private void Awake()
    {
		DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		Collider = GetComponent<CapsuleCollider2D>();
		colliderSize = Collider.size;
		m_Rigidbody2D.gravityScale = initialGravity;
	}


	private void Update()
	{

		m_Grounded = false;
		RaycastHit2D hit_mid, hit_left, hit_right;
		hit_mid = Physics2D.Raycast(m_GroundCheck.transform.position, Vector2.down, 0.2f, m_WhatIsGround);
		hit_left = Physics2D.Raycast(m_GroundCheckLeft.transform.position, Vector2.down, 0.2f, m_WhatIsGround);
		hit_right = Physics2D.Raycast(m_GroundCheckRight.transform.position, Vector2.down, 0.2f, m_WhatIsGround);
		//Debug.Log("left: " + hit_left.collider + " mid: " + hit_mid.collider + " right: " + hit_right.collider);
		if (hit_mid.collider != null || hit_left.collider != null || hit_right.collider != null)
		{
			m_Grounded = true;
		}
		

		atLedge = false;
		Collider2D[] wall1 = Physics2D.OverlapCircleAll(m_WallCheck1.position, .01f, m_WhatIsGround);
		Collider2D[] wall2 = Physics2D.OverlapCircleAll(m_WallCheck2.position, .01f, m_WhatIsGround);
		if (wall1.Length == 0 && wall2.Length > 0)
		{
			atLedge = true;
		}

	}
    void FixedUpdate()
    {
		SlopeCheck();
		m_Rigidbody2D.velocity = Vector2.ClampMagnitude(m_Rigidbody2D.velocity, maxSpeed);

		if (pushedBack)
		{
			ApplyPushBack(pushBackDirection, pushBackSpeed);
		}
	}

    private void SlopeCheck()
    {
		Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);
		SlopeCheckVertical(checkPos);
    }


	private void SlopeCheckVertical(Vector2 checkPos)
	{
		RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, 0.5f, m_WhatIsGround);
		if (hit)
        {
			slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
			slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

			if(slopeDownAngle != slopeDownAngleOld)
            {
				isOnSlope = true;
			}
			if (slopeDownAngle == 0)
			{
				isOnSlope = false;
			}



			slopeDownAngleOld = slopeDownAngle;

			Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
			Debug.DrawRay(hit.point, hit.normal, Color.green);



		}

	}

	private void ApplyPushBack(Vector3 direction, float speed)
	{
		if (pushBackSpeed > 0)
		{
			targetVelocity = new Vector2(speed * direction.x, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = targetVelocity;
			pushBackSpeed -= 1;
		}

		else
		{
			pushedBack = false;
		}
	}

	private float jumptime;
	public void Move(float speed, bool jump)
	{
		float multiplier = 10f;

		if (!pushedBack)
		{
			if (m_Grounded && isOnSlope && !isJumping)
			{
				targetVelocity = new Vector2(-speed * multiplier * slopeNormalPerp.x, -speed * multiplier * slopeNormalPerp.y);
			}
			else
			{
				targetVelocity = new Vector2(speed * multiplier, m_Rigidbody2D.velocity.y);
			}

			if (jump)
			{
				jumptime = Time.time;
				m_Grounded = false;
				targetVelocity.y = m_JumpForce;
			}
			//m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
			m_Rigidbody2D.velocity = targetVelocity;

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
	}

	public GameObject blinkEffect;
	public void Blink(float distance = 1)
    {
		if(SkillWheel.selected == 1)
			Instantiate(blinkEffect, transform.position, Quaternion.identity);

		float blinkDistance;
		RaycastHit2D hit2 = Physics2D.Raycast(m_GroundCheck.position, Vector2.right * transform.localScale, 10f, m_WhatIsGround);
		//Debug.Log("forwardMotion: " + forwardMotion + ", maxdistance: " + hit.distance);
		if (hit2.distance > distance || hit2.collider == null)
			blinkDistance = distance;
        else
        {
			blinkDistance = hit2.distance;
		}
		transform.position += new Vector3(transform.localScale.x * blinkDistance, 0, 0); 
	}


	public IEnumerator FallThrough()
    {
		oneWayPlatform = GameObject.Find("OneWay");
		Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), oneWayPlatform.GetComponent<CompositeCollider2D>(),true);
		yield return new WaitForSeconds(0.2f);
		Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), oneWayPlatform.GetComponent<CompositeCollider2D>(),false);
	}

	public void Drift()
    {
		m_Rigidbody2D.velocity += Vector2.up * 0.25f;
    }

	public void Jump()
    {
		m_Grounded = false;
		m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
	}
	public void AirJump()
    {
		m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce*3f/4f);
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


    private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	//private float knockbackDuration = 1f;
	//private float knockbackStartTime;
	public Vector2 knockbackSpeed = new Vector2(10, 1);

	public void Knockback(float direction)
	{
		//knockbackStartTime = Time.time;
		m_Rigidbody2D.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
	}
	/*
	private void CheckKnockBack()
    {
		if (Time.time >= knockbackStartTime + knockbackDuration)
        {
			m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);

		}
    }
	*/

}
