using UnityEngine;
using System.Collections;

public abstract class ControlledUnit : MonoBehaviour {

	//
	//Inputs
	//

	protected InputStruct input;
	public void InputMessage(InputStruct i){ input = i; }

	//
	//Player specifics
	//
	protected int playerNumber; 
	protected sbyte forwarddir { get; private set; }
	protected sbyte backdir { get; private set; }

	public bool isPlayerOne { get{ return playerNumber == 1; }}
	public bool isPlayerTwo { get{ return playerNumber == 2; }}

	//
	//Set HP
	//

	public int InitialHealth = 1;
	protected int health;


	//
	//Component caching
	//

	public Animator anim { get; private set; }
	protected Rigidbody2D rb2d;

	//
	//Setup
	//

	void Awake(){
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		health = InitialHealth;
		playerNumber = LayerMask.LayerToName(gameObject.layer).Contains("1") ? 1 : 2;
		forwarddir = playerNumber == 1 ? (sbyte)1 : (sbyte)-1;
		backdir = (sbyte)-forwarddir;
	}

	//
	//Abstract methods
	//
	
	//Instead of FixedUpdate(), we'll use Act()
	//that the controller will call
	public abstract void Act();
	public abstract void Attack();


	//
	//Unit specific things
	//

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

		//if that list becomes empty, our commander should die
		if(cl.Count == 0)
			dad.GetComponent<UnitController>().Die();

		//destroy instance
		Destroy(gameObject);
	}
}

