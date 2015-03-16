using UnityEngine;
using System.Collections;

//A single SubSoldier, the actual soldiers that move around on screen with the sprites
public class SubSoldier : ControlledUnit {

	//
	//Move Speeds
	//

	//'Rook' move speed
	public float MoveSpeed = 10f;
	//'Bishop' move speed, derived from MoveSpeed
	private float diagSpeedComponent;

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

		//Here we would put the attempt to move to goal position
		//change velocity
		rb2d.velocity = new Vector2(input.x, input.y) * speed;

		anim.SetBool("IsMoving", rb2d.velocity != Vector2.zero);

	}

	//Spawn bullets here
	public override void Attack(){
		Instantiate(Projectile, rb2d.position, Quaternion.identity);
	}

}

