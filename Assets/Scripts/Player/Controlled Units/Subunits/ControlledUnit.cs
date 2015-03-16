using UnityEngine;
using System.Collections;

//A single SubSoldier, the actual soldiers that move around on screen with the sprites
//this probably should be abstracted
public abstract class ControlledUnit : MonoBehaviour {

	//
	//Inputs
	//

	protected InputStruct input;
	public void InputMessage(InputStruct i){ input = i; }

	//
	//Set HP
	//

	public int InitialHealth = 1;
	protected int health;

	//
	//Componenets
	//

	//Animations
	protected Animator anim;
	//rigidbody2d
	protected Rigidbody2D rb2d;


	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		health = InitialHealth;
	}
	
	//Instead of FixedUpdate(), we'll use Act()
	//that the controller will call
	public abstract void Act();
	public abstract void Attack();


	//returns true if this dies
	public bool InflictDamage(int dmg){
		health -= dmg;
		if(health <= 0){
			Die();
			return true;
		}
		return false;
	}

	//kill this object
	void Die(){
		//1) Remove self from parent's list
		transform.parent.GetComponent<ChildrenList>().RemoveFromList(this);
		//2) death
		Destroy(gameObject);
	}
}

