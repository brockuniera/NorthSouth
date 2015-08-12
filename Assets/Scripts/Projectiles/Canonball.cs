using UnityEngine;
using System.Collections;

public class Canonball : ExtraBehaviour {

	// How long hurtbox stays active when the canonball stops
	public float HurtboxAliveTime = .1f;


	// PERCENTCHARGE: From canons.cs, percent charge
	// of our canons. We'll grab the distance etc from
	// him too.
	public void Setup(float percentCharge){

		float distance = percentCharge * Canons.maxDistance + Canons.minDistance;

		// Set our Velocity
		float xvel = isPlayerOne ? Canons.Velocity : -Canons.Velocity;
		GetComponent<Rigidbody2D>().velocity = new Vector2(xvel, 0.0f);

		// Set how long we live for
		float time = distance / Canons.Velocity;
		StartCoroutine(coro_dieAfter(time));


		// We need to play the full animation over the entire
		// course of the canonball.
		//

		// Animation finishes in one second
		GetComponent<Animator>().speed /= time;
	}

	// Coroutine for exploding after we reach our goal
	IEnumerator coro_dieAfter(float time){
		yield return new WaitForSeconds(time);

		// Spawn explosion hitbox
		// TODO Explosion animation

		// Spawn a hurtbox at where we die, lasting for .1f
		SpawnHurtbox(transform.position, HurtboxAliveTime);

		Object.Destroy(gameObject);
	}
}
