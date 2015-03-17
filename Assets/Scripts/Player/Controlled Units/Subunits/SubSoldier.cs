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
	public float MoveSpeed;
	//'Bishop' move speed, derived from MoveSpeed
	private float diagSpeedComponent;

	//'Catchup' variables, used to move to GoalPosition
	//
	//Speed SubSoldier moves at to catchup
	public float CatchupSpeed;
	//Minimum distance to try catching up
	public float CatchupMinDist;
	//Random[0,1] is multiplied by this and added to CatchupMinDist
	public float RandomMinDistFactor;
	//TODO above will probably cause jittering
	// until unit moves past CatchupMinDist

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

	void Update(){
		anim.SetBool("IsMoving", rb2d.velocity != Vector2.zero);
	}

	public override void Act(){
		//
		//Movement
		//


		//Catchup has a minimum distance required to start trying to catchup
		Vector2 dist = GoalPosition - rb2d.position;
		if(dist.sqrMagnitude > (CatchupMinDist + RandomMinDistFactor * Random.value)){
			//I'm too far; I'll catchup to the unit!
			Vector2 catchup = dist;
			catchup.Normalize();
			catchup *= CatchupSpeed;
			rb2d.velocity = catchup;
		}else{
			//I'm within range; I'll move with the unit!
			//if both inputs, use diagSpeedComponent
			float speed = input.x != 0 && input.y != 0 ? diagSpeedComponent : MoveSpeed;
			rb2d.velocity = new Vector2(input.x, input.y) * speed;
		}

		//DEBUG
		//if(playerNumber == 1 && this == transform.parent.GetComponent<ChildrenList>().At(1))
			//Debug.Log("Current: " + rb2d.position + " Goal: " + GoalPosition + " catchup: " + catchup);

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

