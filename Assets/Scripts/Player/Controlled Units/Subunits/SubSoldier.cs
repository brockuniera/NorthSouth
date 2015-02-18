using UnityEngine;
using System.Collections;

//A single SubSoldier, the actual soldiers that move around on screen with the sprites
//this probably should be abstracted
public class SubSoldier : MonoBehaviour {
	private int health = 1;

	//Animations
	private Animator Anim;

	public Animator GetAnimator(){ return Anim; }

	//Goal Position
	// this unit attempts to move to its goal position at all times
	// overall movement is handled by being a child of Soldiers
	public Vector2 GoalPosition;

	// Use this for initialization
	void Awake () {
		Anim = GetComponent<Animator>();
	}
	
	//Instead of FixedUpdate(), we'll use Act() so that the 
	//controlling soldiers class can call it and ensure order 
	//of execution isn't too weird
	public void Act(){
	}

	//Spawn bullets here
	public void Attack(){
	}

	//returns true if this dies
	// replace with event system????
	public void InflictDamage(int dmg){
		health -= dmg;
		if(health <= 0){
			Die();
		}
	}

	//kill this object
	void Die(){
		//1) Remove self from parent's list
		// abstract this line
		transform.parent.GetComponent<Soldiers>().RemoveFromList(this);
		//2) Die
		Destroy(gameObject);
	}
}
