using UnityEngine;
using System.Collections;

public class CanonReticule : MonoBehaviour {

	// How long the reticule stays alive after reaching its destination
	public float PostMoveAliveTime = 1.0f;

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
		StartCoroutine(coro_stopMoving());
	}

	private IEnumerator	coro_stopMoving(){
		// Play animation
		// TODO

		yield return new WaitForSeconds(PostMoveAliveTime);

		// Die
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
