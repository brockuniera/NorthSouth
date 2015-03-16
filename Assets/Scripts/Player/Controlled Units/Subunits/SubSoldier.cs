using UnityEngine;
using System.Collections;

//A single SubSoldier, the actual soldiers that move around on screen with the sprites
public class SubSoldier : ControlledUnit {

	//
	//Move Speeds
	//

	//Input speeds
	//
	//'Rook' move speed
	public float MoveSpeed = 10f;
	//'Bishop' move speed, derived from MoveSpeed
	private float diagSpeedComponent;

	//'Catchup' speeds, used to move to GoalPosition
	//
	public float CatchupSpeed = 10f;
	public float CatchupMinDist = 1f;


	//Goal Position
	// this unit attempts to move to its goal position at all times
	//We could weigh the need to move to GoalPosition
	//vs the movement vector given by just the input
	public Vector2 GoalPosition;

	//
	//Prefabs
	//

	//Bullet prefab to use for shooting
	public Bullet Projectile;

	void Start(){
		//Moving diagonally
		diagSpeedComponent = MoveSpeed / Mathf.Sqrt(2);
	}

	public override void Act(){
		//
		//Movement
		//

		//if both inputs, use diagSpeedComponent
		float speed = input.x != 0 && input.y != 0 ? diagSpeedComponent : MoveSpeed;

		//attempt to move to goal position
		//TODO only catchup if we're too far away
		//Old method:
		//Vector2 catchup = Vector2.MoveTowards(rb2d.position, GoalPosition, CatchupSpeed) - rb2d.position;
		Vector2 catchup = Vector2.zero;
		Vector2 dist = GoalPosition - rb2d.position;
		if(dist.sqrMagnitude > CatchupMinDist){
			catchup = dist;
			catchup.Normalize();
			catchup *= CatchupSpeed;
		}

		//DEBUG
		//if(playerNumber == 1 && this == transform.parent.GetComponent<ChildrenList>().At(1)){
			//Debug.Log("Current: " + rb2d.position + " Goal: " + GoalPosition + " catchup: " + catchup);
		//}

		//change velocity
		rb2d.velocity = new Vector2(input.x, input.y) * speed + catchup;

		anim.SetBool("IsMoving", rb2d.velocity != Vector2.zero);
	}

	//Spawn bullets
	public override void Attack(){
		Instantiate(Projectile, rb2d.position, Quaternion.identity);
	}

	//Oncollsion, we could swap leaders?
	void OnCollisionEnter2D(){
		print("Test");
	}

}

