using UnityEngine;
using System.Collections;

public class Canonball : ExtraBehaviour {

	// How long hurtbox stays active when the canonball stops
	public float HurtboxAliveTime = .1f;

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

	public AnimationCurve FlightPath;
	public float MaxHeight = 12.0f;

	// PERCENTCHARGE: From canons.cs, percent charge
	// of our canons. We'll grab the distance etc from
	// him too.
	public void Setup(float distance){
		f_goalposx = transform.position.x + distance;
		f_startposx = transform.position.x;
		f_startposy = transform.position.y;
		f_distance = distance;

		// We need to play the full animation over the entire
		// course of the canonball.
		//

		// Animation finishes in one second
		float invtime = Canons.CanonballVelocity / distance;
		GetComponent<Animator>().speed *= invtime;
	}

	// Coroutine for exploding after we reach our goal
	private void Die(){
		// Spawn explosion hitbox
		// TODO Explosion animation

		// Spawn a hurtbox at where we die, lasting for HurtboxAliveTime
		SpawnHurtbox(transform.position, HurtboxAliveTime);

		Object.Destroy(gameObject);
	}

	//
	// Unity Update methods
	//

	void FixedUpdate(){
		// Move Vel * fixedDeltaTime every fixedupdate
		// Using rigidbody.velocity
		Vector3 newpos = transform.position;

		// x pos
		//
		float frameDist = Canons.CanonballVelocity * Time.deltaTime;
		if(isPlayerTwo)
			frameDist = -frameDist;
		newpos.x += frameDist;

		// y pos
		//
		// TODO Will this work for Player 2?
		float percentDist = (newpos.x - f_startposx) / f_distance;
		newpos.y = FlightPath.Evaluate(percentDist) * MaxHeight + f_startposy;

		transform.position = newpos;

		// Explode when we're close enough
		if(transform.position.x > f_goalposx - GoalEpsilon){
			Die();
		}
	}
}
