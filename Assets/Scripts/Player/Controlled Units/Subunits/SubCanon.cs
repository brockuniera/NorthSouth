using UnityEngine;
using System.Collections;
using GameObjectExtensions;

//A single SubCanon
public class SubCanon : ControlledUnit {

	//
	//Movement
	//

	//Input speeds
	//
	//Normal movement speed
	public float MoveSpeed;
	//Speed while charging
	public float SlowSpeed;

	// Are we currently moving slow?
	public bool isSlow { get; set; }

	//
	//Attacking
	//

	// Drawn reticule
	public CanonReticule Reticule;
	private CanonReticule m_reticule;
	
	//Projectilve prefab
	public Canonball CanonballToSpawn;

	//Where to spawn projectile, relative to transform.position
	public Vector2 MuzzleRelative;

	// The transform of our targeting reticule
	private Transform t_reticule;

	//
	//Unity Callbacks
	//

	void Start(){
		// Nothing
	}

	void Update(){
		// Nothing
	}

	//
	//Attacking methods
	//

	public void StartCharging(){
		// Spawn and save an instance of our reticule
		m_reticule = Instantiate(
			Reticule,
			transform.position + (Vector3)MuzzleRelative,
			Quaternion.identity
		) as CanonReticule;

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
		Canons parent = transform.parent.gameObject.GetComponent<Canons>() as Canons;

		// Tell our cball whats up
		cball.Setup(parent.percentCharge);

		// Tell our reticule to stop moving
		m_reticule.StopMoving();
	}

	public void Cooldown(){
		//Nothing
	}

	//
	//Movement Helpers
	//

	private void SetVelocityFromInput(){
		float speed = isSlow ? SlowSpeed : MoveSpeed;
		rb2d.velocity = new Vector2(0, input.y * speed);
	}

	//
	// Movement
	//

	public override void Act(){

		//Movement
		//

		SetVelocityFromInput();
	}

}

