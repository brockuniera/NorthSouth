using UnityEngine;
using System.Collections;

//TODO Make player specific prefabs for everything
//A single SubSoldier, the actual soldiers that move around on screen with the sprites
public class SubSoldier : ControlledUnit {

	//'Rook' move speed
	public float MoveSpeed = 10f;
	//'Bishop' move speed, derived from MoveSpeed
	private float _diagSpeedComponent;

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
		_diagSpeedComponent = MoveSpeed / Mathf.Sqrt(2);
	}

	//Instead of FixedUpdate(), we'll use Act() so that the 
	//controlling soldiers class can call it and ensure order 
	//of execution isn't too weird
	public override void Act(){
		//
		//Movement
		//
		
		//if both inputs, use _diagSpeedComponent
		float speed = input.x != 0 && input.y != 0 ? _diagSpeedComponent : MoveSpeed;

		//Here we would put the attempt to move to goal position
		//change velocity
		_rb2d.velocity = new Vector2(input.x, input.y) * speed;

		_anim.SetBool("IsMoving", _rb2d.velocity != Vector2.zero);

	}

	//Spawn bullets here
	public override void Attack(){
		Instantiate(Projectile, _rb2d.position, Quaternion.identity);
	}

}

