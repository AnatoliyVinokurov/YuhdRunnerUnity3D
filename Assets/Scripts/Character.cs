using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public enum SIDE
{
	Left,
	Mid,
	Right
}

public enum HitX
{
	Left,
	Mid,
	Right,
	None
}

public enum HitY
{
	Up,
	Mid,
	Down,
	None
}

public enum HitZ
{
	Forward,
	Mid,
	Backward,
	None
}


public class Character : MonoBehaviour
{

	public SIDE m_Side = SIDE.Mid;
	float NewXPos = 0f;
	[HideInInspector]
	public bool SwipeLeft, SwipeRight, SwipeUp, SwipeDown;
	public float XValue;
	private CharacterController m_char;
	private CapsuleCollider m_caps;
	public Animator m_Animator;
	private float x;
	public float SpeedDodge;
	public float JumpPower = 7f;
	private float y;
	public bool InJump;
	public bool InRoll;
	public float FwdSpeed = 7f;
	private float ColHeight;
	private float ColCenterY;
	private float CapsHeight;
	private float CapsCenterY;
	bool isRolling = false;
	public GameManager GM;
	public HitX hitX = HitX.None;
	public HitY hitY = HitY.None;
	public HitZ hitZ = HitZ.None;
	public bool CanPlay = true;
	public float CalculatingTheDistanceTraveled;
	public bool ImmunityToTraps = false;


	void Start ()
	{
		XValue = 3;
		m_char = GetComponent<CharacterController> ();
		m_caps = GetComponent<CapsuleCollider> ();
		ColHeight = m_char.height;
		CapsHeight = m_caps.height;
		ColCenterY = m_char.center.y;
		CapsCenterY = m_caps.center.y;
		transform.position = Vector3.zero;
	}

	void Update ()
	{
		if (CanPlay) {
			SwipeLeft = Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow);
			SwipeRight = Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow);
			SwipeUp = Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow);
			SwipeDown = Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow);
			if (SwipeLeft && !InRoll) {
				if (m_Side == SIDE.Mid) {
					NewXPos = -XValue;
					m_Side = SIDE.Left;
					m_Animator.Play ("DodgeLeft");
				} else if (m_Side == SIDE.Right) {
					NewXPos = 0;
					m_Side = SIDE.Mid;
					m_Animator.Play ("DodgeLeft");
				}
			} else if (SwipeRight && !InRoll) {
				if (m_Side == SIDE.Mid) {
					NewXPos = XValue;
					m_Side = SIDE.Right;
					m_Animator.Play ("DodgeRight");
				} else if (m_Side == SIDE.Left) {
					NewXPos = 0;
					m_Side = SIDE.Mid;
					m_Animator.Play ("DodgeRight");
				}
			}
			Vector3 moveVector = new Vector3 (x - transform.position.x, y * Time.deltaTime, FwdSpeed * Time.deltaTime * GM.MoveSpeed);
			x = Mathf.Lerp (x, NewXPos, Time.deltaTime * SpeedDodge);
			m_char.Move (moveVector);
			Jump ();
			Roll ();
			CalculatingTheDistanceTraveled = transform.position.z;
		}
	}


	public void Jump ()
	{
		if (m_char.isGrounded && !isRolling) {
			if (m_Animator.GetCurrentAnimatorStateInfo (0).IsName ("Jump")) {
				m_Animator.Play ("Landing");
				InJump = false;
			}
			if (SwipeUp) {
				y = JumpPower;
				m_Animator.CrossFadeInFixedTime ("Jump", 0.1f);
				InJump = true;
			}
		} else {
			y -= JumpPower * 2 * Time.deltaTime;
			if (m_char.velocity.y < 1)
				m_Animator.SetTrigger ("falling");
		}
	}

	internal float RollCounter;

	public void Roll ()
	{
		RollCounter -= Time.deltaTime;
		if (RollCounter <= 0f) {
			RollCounter = 0f;
			if (!isRolling) {
				m_char.center = new Vector3 (0, ColCenterY, 0);
				m_char.height = ColHeight;
				m_caps.center = new Vector3 (0, CapsCenterY, 0);
				m_caps.height = CapsHeight;
				InRoll = false;
			}
		}
		if (SwipeDown && !isRolling) {
			RollCounter = 0.2f;
			y -= 10f;
			m_char.center = new Vector3 (0, ColCenterY / 2f, 0);
			m_char.height = ColHeight / 2f;
			m_caps.center = new Vector3 (0, CapsCenterY / 2f, 0);
			m_caps.height = CapsHeight / 2f;
			m_Animator.CrossFadeInFixedTime ("Roll", 0.1f);
			InRoll = true;
			InJump = false;
			StartCoroutine (DoRoll ());
		}
	}


	IEnumerator DoRoll ()
	{
		Debug.Log ("rolling");
		isRolling = true;
		yield return new WaitForSeconds (0.65f);
		isRolling = false;
		Debug.Log ("not rolling");
	}


	public void OnCharacterColliderHit (Collider col)
	{
		hitX = GetHitX (col);
		hitY = GetHitY (col);
		hitZ = GetHitZ (col);
		if (hitX == HitX.Right) {
			if (col.gameObject.CompareTag ("Trap")) {
				Debug.Log ("On Left");
				if (m_Side == SIDE.Mid) {
					NewXPos = XValue;
					m_Side = SIDE.Right;
					m_Animator.Play ("DodgeRight");
					m_Animator.Play ("StumbleLeft");
				} else if (m_Side == SIDE.Left) {
					NewXPos = 0;
					m_Side = SIDE.Mid;
					m_Animator.Play ("DodgeRight");
					m_Animator.Play ("StumbleLeft");
				}
			}
		}
		if (hitX == HitX.Left) {
			if (col.gameObject.CompareTag ("Trap")) {
				Debug.Log ("On right");
				if (m_Side == SIDE.Mid) {
					NewXPos = -XValue;
					m_Side = SIDE.Left;
					m_Animator.Play ("DodgeLeft");
					m_Animator.Play ("StumbleRight");
				} else if (m_Side == SIDE.Right) {
					NewXPos = 0;
					m_Side = SIDE.Mid;
					m_Animator.Play ("DodgeLeft");
					m_Animator.Play ("StumbleRight");
				}
			}
		}
		if (hitZ == HitZ.Forward && hitY != HitY.Up && hitY != HitY.Down) { //if the character hit the front (Forward) and it's not up (UP boss) and not down (Down leg) then death
			if (col.gameObject.CompareTag ("Trap")) {
				Debug.Log ("Trap");
				if (ImmunityToTraps) {
					Destroy (col.gameObject);
				} else {
					m_Animator.Play ("FallOver");
					StartCoroutine (Death ());
				}

			} 
		}
		if (hitZ == HitZ.Forward && hitY == HitY.Up) {
			if (col.gameObject.CompareTag ("Trap")) {
				Debug.Log ("Hit the pole against a obstacle");
				if (ImmunityToTraps) {
					Destroy (col.gameObject);
				} else {
					m_Animator.Play ("SweepFall");
					StartCoroutine (Death ());
				}
			}
		}

		if (hitZ == HitZ.Forward && hitY == HitY.Down) {  // if the character collided with the front (Forward) and the foot (Down) then
			if (col.gameObject.CompareTag ("Trap")) {
				Debug.Log ("Tripping over a stone");
				if (ImmunityToTraps) {
					Destroy (col.gameObject);
				} else {
					StartCoroutine (SweepFall ());
					StartCoroutine (Death ());
				}
			} else if (col.gameObject.CompareTag ("Springboard")) {
				Debug.Log ("Jumped on the trampoline");
				//push the character back a little on zed
				Vector3 moveVector = new Vector3 (x - transform.position.x, y * Time.deltaTime + 1, FwdSpeed * Time.deltaTime); //The character is now in a jump, and so that he does not float in the air, we quickly lay him on the ground
				x = Mathf.Lerp (x, NewXPos, Time.deltaTime * SpeedDodge);
				m_char.Move (moveVector);
			}
		}
	}


	IEnumerator SweepFall ()
	{
		yield return new WaitForSeconds (0.1f);
		m_Animator.Play ("SweepFall");
	}


	public HitX GetHitX (Collider col)
	{
		Bounds char_bounds = m_char.bounds;
		Bounds col_bounds = col.bounds;
		float min_x = Mathf.Max (col_bounds.min.x, char_bounds.min.x);
		float max_x = Mathf.Min (col_bounds.max.x, char_bounds.max.x);
		float average = (min_x + max_x) / 2f - col_bounds.min.x;
		HitX hit;
		if (average > col_bounds.size.x - 0.33f) {
			hit = HitX.Right;
		} else if (average < 0.33f) {
			hit = HitX.Left;
		} else {
			hit = HitX.Mid;
		}
		return hit;
	}


	public HitY GetHitY (Collider col)
	{
		Bounds char_bounds = m_char.bounds;
		Bounds col_bounds = col.bounds;
		float min_y = Mathf.Max (col_bounds.min.y, char_bounds.min.y);
		float max_y = Mathf.Min (col_bounds.max.y, char_bounds.max.y);
		float average = ((min_y + max_y) / 2f - char_bounds.min.y) / char_bounds.size.y;
		HitY hit;
		if (average < 0.33f) {
			hit = HitY.Down;
		} else if (average < 0.66f) {
			hit = HitY.Mid;
		} else {
			hit = HitY.Up;
		}
		return hit;
	}

	public HitZ GetHitZ (Collider col)
	{
		Bounds char_bounds = m_char.bounds;
		Bounds col_bounds = col.bounds;
		float min_z = Mathf.Max (col_bounds.min.z, char_bounds.min.z);
		float max_z = Mathf.Min (col_bounds.max.z, char_bounds.max.z);
		float average = ((min_z + max_z) / 2f - char_bounds.min.z) / char_bounds.size.z;
		HitZ hit;
		if (average < 0.33f) {
			hit = HitZ.Backward;
		} else if (average < 0.66f) {
			hit = HitZ.Mid;
		} else {
			hit = HitZ.Forward;
		}
		return hit;
	}


	IEnumerator Death ()
	{
		Debug.Log ("death");
		CanPlay = false;

		//m_Animator.SetTrigger ("death");
		yield return new WaitForSeconds (0.9f);
		//m_Animator.ResetTrigger ("death");
		GM.ShowResult ();

	}

	public void ImmunityVb ()
	{
		//push the character back a little on z
		Vector3 moveVector = new Vector3 (x - transform.position.x, y * Time.deltaTime + 1, FwdSpeed * Time.deltaTime - 1);
		x = Mathf.Lerp (x, NewXPos, Time.deltaTime * SpeedDodge);
		m_char.Move (moveVector);
		StartCoroutine (ImmunityFr ());
	}

	IEnumerator ImmunityFr ()
	{
		ImmunityToTraps = true;
		yield return new WaitForSeconds (0.9f);
		ImmunityToTraps = false;
	}

	public void RespawnColider ()
	{
		m_char.center = new Vector3 (0, ColCenterY, 0);
		m_char.height = ColHeight;
		m_caps.center = new Vector3 (0, CapsCenterY, 0);
		m_caps.height = CapsHeight;
	}

	public void ReturnToMidline ()
	{
		if (m_Side == SIDE.Left) {
			NewXPos = 0;
			m_Side = SIDE.Mid;
		} else if (m_Side == SIDE.Right) {
			NewXPos = 0;
			m_Side = SIDE.Mid;
		}

	}

	private void OnTriggerEnter (Collider other)
	{
		if (!other.CompareTag ("Coin"))
			return;
		GM.AddCoins (1);
		Destroy (other.gameObject);
	}
}
