using UnityEngine;
using System.Collections;

//For List<T>
using System.Collections.Generic;


//'Soldiers' type of unit
//create function to pass functions/actions to all children?
public class Soldiers : AbstractUnit{

	//
	//Movement
	//

	//'Rook' move speed
	public float MoveSpeed = 10f;
	//'Bishop' move speed, derived from MoveSpeed
	private float _diagSpeedComponent;

	//cached rigidbody
	private Rigidbody2D _rb2d;


	//
	//FSM, for attacking
	//

	//Possible states
	private bool _attacking = false;

	//For attack animation leadup and cool down
	// These happen in consecutive order
	// ie the whole animation stops at PostAttackFrame
	//
	//'Frames' means FixedUpdate()
	public int PreAttackFrame = 10;
	public int AttackShootFrame = 10;

	//frame counter for animation timing in FixedUpdate
	private int _frame = 0;


	//
	//Bullets
	//
	

	//Bullet prefab
	public Bullet Projectile;
	

	//
	//Subunits
	//

	//SubSoldier might be a canidate for abstraction...
	private List<SubSoldier> units;
	public void RemoveFromList(SubSoldier ss){
		if(ss == null)
			return;
		units.Remove(ss);

		if(units.Count == 0)
			Destroy(gameObject);
	}


	//
	//Code
	//


	void Awake(){
		_diagSpeedComponent = MoveSpeed / Mathf.Sqrt(2);
		_rb2d = rigidbody2D;

		print(playerNumber);

		//populate list of controlled units
		units = new List<SubSoldier>();
		foreach(Transform child in transform){
			var comp = child.GetComponent<SubSoldier>();
			child.gameObject.layer = LayerMask.NameToLayer("Player " + 
					(playerNumber == 1 ? '1' : '2'));
			if(comp){
				units.Add(comp);
			}
		}
		
		//TODO:set layers recursivley throughout children
	}

	//move around, attack
	void FixedUpdate(){
		//we're iterating through and telling each to act()
		//instead of reying on unity's update()
		foreach(SubSoldier ss in units){
			ss.Act();
			ss.GetAnimator().SetBool("IsMoving", _rb2d.velocity != Vector2.zero);
		}

		//attacking has 3 distinct parts
		//and it also stops movement
		if(_attacking){

			_frame++;

			if(_frame == 1){
				print("Getting ready...");
			}else if(_frame == PreAttackFrame){
				print("...Fire!...");
				Shoot();
			}else if(_frame == PreAttackFrame + AttackShootFrame){ 
				print("...Done");
				_attacking = false;
			}

			return;
		}

		//if attack, change state, stop moving
		if(input.a){
			_attacking = true;
			_rb2d.velocity = Vector2.zero;
			_frame = 0;
			return;
		}


		//
		//Movement
		//

		//Directions!
		//   +y  
		//    |
		//-x -0- +x
		//    |
		//   -y  
		
		//if both inputs, use _diagSpeedComponent
		float speed = input.x != 0 && input.y != 0 ? _diagSpeedComponent : MoveSpeed;

		//change velocity
		_rb2d.velocity = new Vector2(input.x, input.y) * speed;
		
	}

	
	//tell sub units to shoot
	private void Shoot(){
		//create bullet
		foreach(SubSoldier ss in units){
			Bullet go = Instantiate(Projectile, ss.transform.position,
					Quaternion.identity) as Bullet;
			go.Initialize(playerNumber);
		}
	}

}


