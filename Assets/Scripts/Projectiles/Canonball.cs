using UnityEngine;
using System.Collections;

public class Canonball : ExtraBehaviour {

	// How long hurtbox stays active when the canonball stops
	public float HurtboxAliveTime = .1f;

	// Our ending position
	private float f_goalpos;

	// PERCENTCHARGE: From canons.cs, percent charge
	// of our canons. We'll grab the distance etc from
	// him too.
	public void Setup(float distance){
		f_goalpos = transform.position.x + distance;

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
		Vector3 newpos = transform.position;
		float frameDist = Canons.CanonballVelocity * Time.fixedDeltaTime;
		if(isPlayerTwo)
			frameDist = -frameDist;
		newpos.x += frameDist;
		transform.position = newpos;

		if(FloatHelper.IsApproxDelta(f_goalpos, transform.position.x, .2f)){
			Die();
		}
	}
}
