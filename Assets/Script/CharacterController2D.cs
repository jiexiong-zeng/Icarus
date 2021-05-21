using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;  
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Transform m_WallCheck1;
	[SerializeField] private Transform m_WallCheck2;

	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

	public float initialGravity = 2f;
	const float k_GroundedRadius = .01f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	public bool falling;
	public Animator animator;
	public bool atLadder;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	public bool grab;

    public void Update()
    {
		//groundtime -= Time.deltaTime;
	}
    private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
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
				//groundtime = groundedbuffer; ///
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

		if (!m_Grounded && m_Rigidbody2D.velocity.y < -0.01f)
        {
			falling = true;
			animator.SetBool("Jumping", false);
			animator.SetBool("Falling", true);
        }

		if (m_Grounded && !wasGrounded)
        {
			falling = false;
			animator.SetBool("Jumping", false);
			animator.SetBool("Falling", false);
		}


		Collider2D[] wall1 = Physics2D.OverlapCircleAll(m_WallCheck1.position, .01f , m_WhatIsGround);
		Collider2D[] wall2 = Physics2D.OverlapCircleAll(m_WallCheck2.position, .01f, m_WhatIsGround);


		if (wall1.Length == 0 && wall2.Length > 0)
        {
			grab = true;
			falling = false;
			animator.SetBool("Grab", true);
        }
        else
        {
			grab = false;
			animator.SetBool("Grab", false);
		}

	}


	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		if (grab)
        {
			Stop();
			m_Rigidbody2D.gravityScale = 0f;
        }
        else
        {
			m_Rigidbody2D.gravityScale = initialGravity;
        }

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

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
		if (m_Grounded && jump)
		//if (m_Grounded && groundtime > 0 && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			//m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			//change from addforce to set veloctiy
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
		}

	}

	public void ClimbLadder()
    {
		m_Rigidbody2D.velocity = new Vector2(0, 2f);
	}

    public void Stop()
    {
		m_Rigidbody2D.velocity = new Vector2(0, 0);
	}


	public void Roll()
    {

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
