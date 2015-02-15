using UnityEngine;
using System.Collections;

//A single SubSoldier, the actual soldiers that move around on screen with the sprites
//this probably
public class SubSoldier : MonoBehaviour {

	//Animations
	private Animator Anim;

	public Animator GetAnimator(){ return Anim; }


	//Goal Position
	// this unit attempts to move to its goal position at all times
	// overall movement is handled by being a child of Soldiers
	public Vector2 GoalPosition;

	//cached Transform
	// for shooting bullets
	private Transform _tran;

	// Use this for initialization
	void Awake () {
		Anim = GetComponent<Animator>();
		_tran = transform;
	}
	
	//Instead of FixedUpdate(), we'll use Act() so that the 
	//controlling soldiers class can call it and ensure order 
	//of execution isn't too weird
	public void Act(){
	}

	//Spawn bullets here
	public void Attack(){
	}

}
