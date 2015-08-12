using UnityEngine;
using System.Collections;

public class CanonReticule : MonoBehaviour {

	// Are we still moving to the right?
	// Or has a target been chosen to die?
	private bool isMoving = true;

	//
	// Relevant data to set position with
	//

	private float startXPos;
	private float minDist;
	private float maxDist;
	private Canons canon; // Knows how far we are, as percentage

	//
	// Public methods
	//

	// Setup all needed data to do what we gotta do
	public void Setup(Canons parent){
		canon = parent;

		startXPos = transform.position.x;
		minDist = Canons.minDistance;
		maxDist = Canons.maxDistance;
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

		yield return new WaitForSeconds(1.0f);

		// Die
		Destroy(gameObject);
	}

	//
	// Unity Update methods
	//

	void FixedUpdate(){
		if(isMoving){
			Vector3 newpos = transform.position;
			newpos.x = startXPos + minDist + (maxDist * canon.percentCharge);
			// TODO Stay aligned on y axis also
			// We could be the child of this guy : )
			transform.position = newpos;
		}else{

		}
	}
}
