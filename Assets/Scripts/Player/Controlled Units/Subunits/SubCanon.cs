using UnityEngine;
using System.Collections;

// A single SubCanon
public class SubCanon : ControlledUnit {

	//
	// Movement
	//

	// Input speeds
	//
	// Normal movement speed
	public float MoveSpeed;
	// Speed while charging
	public float SlowSpeed;

	// Are we currently moving slow?
	public bool isSlow { get; set; }

	//
	// Attacking
	//

	// Reticule prefab
	public CanonReticule Reticule;
	// Reference to an instance of the reticule
	private CanonReticule m_reticule;
	
	// Projectilve prefab
	public Canonball CanonballToSpawn;

	//Where to spawn projectile, relative to transform.position
	public Vector2 MuzzleRelative;

	// The transform of our targeting reticule
	private Transform t_reticule;

	//
	// Attacking methods
	//

	public void StartCharging(){
		// Spawn and save an instance of our reticule
		m_reticule = Instantiate(
			Reticule,
			transform.position + (Vector3)MuzzleRelative,
			Quaternion.identity
		) as CanonReticule;

		// Reticule moves relative to us
		m_reticule.transform.parent = transform;

		// Setup our reticule
		m_reticule.Setup(transform.parent.GetComponent<Canons>());
	}

	public override void Attack(){
		// Spawn projectile
		Canonball cball = Instantiate(
			CanonballToSpawn,
			transform.position + (Vector3)MuzzleRelative,
			Quaternion.identity
		) as Canonball;

		// Get parent
		;

		// Tell our cball whats up
		cball.Setup(GetComponentInParent<Canons>().relativeDistance);

		// Tell our reticule to stop moving
		m_reticule.StopMoving();
		m_reticule = null;
	}

	public void Cooldown(){
		//Nothing
	}

	//
	// Movement
	//

	public override void Act(){
		// Movement
		//
		float speed = isSlow ? SlowSpeed : MoveSpeed;
		rb2d.velocity = new Vector2(0, input.y * speed);
	}
}

