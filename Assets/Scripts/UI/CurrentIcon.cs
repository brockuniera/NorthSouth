using UnityEngine;
using System.Collections;

// Hovers over the average position of a group of units
public class CurrentIcon : MonoBehaviour {

	//
	// Settings
	//

	// How long CurrentIcon is displayed
	public float ShowTime = 1.5f;

	// Offset vector
	public Vector2 Offset;

	//
	// Private vars
	//

	// Stored coroutine, so we can stop it if we want
	private IEnumerator _stored_coro;

	// The unit we're currently following around
	private UnitController _uc;

	// Cached SpriteRenderer, for quick enable/disabling
	private SpriteRenderer sr;

	// Should we be rendering ourselves right now?
	private bool shouldRender = false;

	//
	// Unity Callbacks
	//

	void Start(){
		sr = GetComponent<SpriteRenderer>();
		sr.enabled = false;
	}

	void Update() {
		// Only render if we're supposed to
		if(shouldRender){
			// Get position we should go to
			Vector2 pos = getPositionFromUnits(_uc) + Offset;

			// Check if its valid
			if(float.IsNaN(pos.x) || float.IsNaN(pos.y)){
				// Try again next frame if it isn't valid
				sr.enabled = false;
				return;
			}

			// Update our position and make sure we're showing
			sr.enabled = true;
			transform.position = pos;
		}
	}

	//
	// Helper methods
	//

	// Given a UnitController, get the average position of
	// all of his ControlledUnits.
	private Vector2 getPositionFromUnits(UnitController uc){
		float xsum = 0.0f;
		float ymax = float.NegativeInfinity;
		int count = 0;
		foreach(ControlledUnit cu in uc.controlledSubUnits){
			xsum += cu.transform.position.x;
			ymax = Mathf.Max(cu.transform.position.y, ymax);
			count++;
		}
		return new Vector2(xsum / count, ymax);
	}

	// Make us stop rendering after TIME
	private IEnumerator coro_stopAfterTime(float time){
		yield return new WaitForSeconds(time);
		shouldRender = false;
		sr.enabled = false;
	}

	//
	// Public Methods
	//

	// Tell us to hover over a given unit
	public void HoverOver(UnitController uc){
		// Stop hovering over anyone if we currently are
		if(_stored_coro != null){
			StopCoroutine(_stored_coro);
		}

		// Start hovering over new guy if he exists
		if(uc != null){
			_uc = uc;
			shouldRender = true;
			_stored_coro = coro_stopAfterTime(ShowTime);
			StartCoroutine(_stored_coro);
		}
	}
}
