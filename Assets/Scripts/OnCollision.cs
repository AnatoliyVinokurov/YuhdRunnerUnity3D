using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
	public Character m_char;

	private void OnCollisionEnter (Collision collision)
	{
		if (collision.transform.tag == "Player")
			return;
		m_char.OnCharacterColliderHit (collision.collider);
	}
}
