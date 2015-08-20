using UnityEngine;
using System.Collections;

public class Canonball : ExtraBehaviour {

	// How close we can get to goal before blowing up
	public float GoalEpsilon = .2f;

	// Our ending position in x
	private float f_goalposx;
	// Our starting pos in x
	private float f_startposx;
	// Our starting pos in y
	private float f_startposy;
	// 1 dimensional vector of distance on x axis
	private float f_distance;

	// Flight path canonball takes
	public AnimationCurve FlightPath;
	// MaxHeight is a function of percentdistance
	public AnimationCurve MaxHeightFunction;
	//Max height
	public float MaxHeight = 12.0f;
	// Max height we actually use
	private float f_finalMaxHeight;

	// The reticule we want to destroy
	private CanonReticule reticuleToDestroy;

	// PERCENTCHARGE: From canons.cs, percent charge
	// of our canons. We'll grab the distance etc from
	// him too.
	public void Setup(float distance, CanonReticule reticule){
		f_goalposx = transform.position.x + distance;
		f_startposx = transform.position.x;
		f_startposy = transform.position.y;
		f_distance = distance;
		reticuleToDestroy = reticule;

		// We need to play the full animation over the entire
		// course of the canonball.
		//

		// Animation finishes in one second
		float invtime = Canons.CanonballVelocity / distance;
		GetComponent<Animator>().speed *= invtime;

		// Figure out our actual max height to use
		float percentDist =
			(Mathf.Abs(distance) - Canons.minDistance) /
			(Canons.maxDistance - Canons.minDistance);
		f_finalMaxHeight = MaxHeightFunction.Evaluate(percentDist) * MaxHeight;
	}

	private void Die(){
		Destroy(gameObject);
		reticuleToDestroy.Die();
	}

	//
	// Unity Update methods
	//

	void FixedUpdate(){
		Vector3 newpos = transform.position;

		// x pos
		//
		float frameDist = Canons.CanonballVelocity * Time.fixedDeltaTime;
		if(isPlayerTwo)
			frameDist = -frameDist;
		newpos.x += frameDist;

		// y pos
		//
		float percentDist = (newpos.x - f_startposx) / f_distance;
		newpos.y = FlightPath.Evaluate(percentDist) * f_finalMaxHeight + f_startposy;

		// Set pos
		//
		transform.position = newpos;

		// Explode when we're close enough
		//
		if((isPlayerOne && transform.position.x > f_goalposx - GoalEpsilon) || 
				(isPlayerTwo && transform.position.x < f_goalposx + GoalEpsilon))
		{
			Die();
		}
	}
}
