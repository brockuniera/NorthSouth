using UnityEngine;
using System.Collections;

//A single SubSoldier, the actual soldiers that move around on screen with the sprites
//this probably should be abstracted
public class SubSoldier : AbstractControlledUnit {

	//Goal Position
	// this unit attempts to move to its goal position at all times
	// overall movement is handled by being a child of Soldiers
	public Vector2 GoalPosition;

	//Instead of FixedUpdate(), we'll use Act() so that the 
	//controlling soldiers class can call it and ensure order 
	//of execution isn't too weird
	public override void Act(){
		//Here we would put the attempt to move to goal position
	}

	//Spawn bullets here
	public override void Attack(){}
}
