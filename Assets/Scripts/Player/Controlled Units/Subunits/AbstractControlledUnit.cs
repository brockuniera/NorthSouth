using UnityEngine;
using System.Collections;

//A single SubSoldier, the actual soldiers that move around on screen with the sprites
//this probably should be abstracted
public abstract class AbstractControlledUnit : MonoBehaviour {
	protected int _health = 1;

	//Animations
	private Animator Anim;
	public Animator GetAnimator(){ return Anim; }


	// Use this for initialization
	void Awake () {
		Anim = GetComponent<Animator>();
	}
	
	//Instead of FixedUpdate(), we'll use Act()
	//that the controller will call
	public abstract void Act();
	public abstract void Attack();


	//returns true if this dies
	// replace with event system????
	public bool InflictDamage(int dmg){
		_health -= dmg;
		if(_health <= 0){
			Die();
			return true;
		}
		return false;
	}

	//kill this object
	void Die(){
		//1) Remove self from parent's list
		// abstract this line
		//TODO fix this line:
		// all controllers will maintain lists of how they work
		transform.parent.GetComponent<AbstractUnitController>().RemoveFromList(this);
		//2) Die
		Destroy(gameObject);
	}
}
