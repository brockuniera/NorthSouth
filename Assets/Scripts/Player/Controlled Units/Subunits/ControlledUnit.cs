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
	//Player num
	//

	protected int playerNumber; 
	//public void SetPlayerNumber(int pn){ playerNumber = pn; } //not needed as of now

	//
	//Component caching
	//

	protected Animator anim;
	protected Rigidbody2D rb2d;


	void Awake(){
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		health = InitialHealth;
		playerNumber = LayerMask.LayerToName(gameObject.layer).Contains("1") ? 1 : 2;
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
	protected void Die(){
		//remove self from list
		Transform dad = transform.parent;
		ChildrenList cl = dad.GetComponent<ChildrenList>();
		cl.RemoveFromList(this);

		//if that list becomes empty, destroy our owner
		if(cl.Count == 0)
			dad.GetComponent<UnitController>().Die();

		//destroy instance
		Destroy(gameObject);
	}
}

