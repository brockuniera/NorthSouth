using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class CanonReticule : ExtraBehaviour {


	// How long hurtbox stays active when the canonball stops
	public float HurtboxAliveTime = .1f;

	// The explosion GO we should spawn
	public GameObject ExplosionToSpawn;

	// Are we still moving to the right?
	// Or has a target been chosen to die?
	private bool isMoving = true;

	//
	// Relevant data to set position with
	//

	// Our starting x position
	private float startXPos;
	private Canons canon; // Knows how far we are, as percentage

	//
	// Public methods
	//

	// Setup all needed data to do what we gotta do
	public void Setup(Canons parent){
		// Save canon so we know the percent charge at all times
		canon = parent;

		// Save initial position
		startXPos = transform.position.x;
	}

	public void StopMoving(){
		isMoving = false;

		// Don't follow our parent anymore
		transform.parent = null;

		// Play animation or whatever, and then die
		GetComponent<Animator>().SetTrigger("ToFast");
	}

	public void Die(){
		// Spawn explosion hitbox
		Instantiate(ExplosionToSpawn, transform.position, Quaternion.identity);

		// Spawn a hurtbox at where we die, lasting for HurtboxAliveTime
		SpawnHurtbox(transform.position, HurtboxAliveTime);
		Destroy(gameObject);
	}

	//
	// Unity Update methods
	//

	void FixedUpdate(){
		if(isMoving){
			Vector3 newpos = transform.position;
			newpos.x = startXPos + canon.relativeDistance;
			transform.position = newpos;
		}
	}
}
