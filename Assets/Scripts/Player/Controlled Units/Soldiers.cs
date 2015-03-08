using UnityEngine;
using System.Collections;


//'Soldiers' type of unit
//create function to pass functions/actions to all children?
public class Soldiers : AbstractUnitController{

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
	//Very basic FSM, for attacking
	//

	//literally just 1 state
	private bool _attacking = false;

	//For attack animation lead up and cool down
	// These happen in consecutive order
	// ie the whole animation stops at PostAttackFrame
	//
	//I define 'frame' to be 1 call to FixedUpdate()
	public int PreAttackFrame = 10;
	public int AttackShootFrame = 10;

	//frame counter for animation timing in FixedUpdate
	private int _frame = 0;


	//
	//Bullets
	//
	
	//Bullet prefab to use for shooting
	public Bullet Projectile;
	

	//
	//Unity Callbacks
	//


	void Start(){
		//Moving diagonally
		_diagSpeedComponent = MoveSpeed / Mathf.Sqrt(2);
		//caching :^)
		_rb2d = rigidbody2D;

		//Debugging to figure out who I belong to
		Debug.Log(playerNumber);
	}

	//Move and attack
	void FixedUpdate(){
		//we're iterating through and telling each to act()
		//instead of relying on unity's update()
		foreach(SubSoldier ss in _controlledSubUnits){
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
				Attack();
			}else if(_frame == PreAttackFrame + AttackShootFrame){ 
				print("...Done");
				_attacking = false;
			}

			//We don't want to process movement if we're attacking
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

	
	//tell SubSoldier to shoot
	private void Attack(){
		//create bullet
		foreach(SubSoldier ss in _controlledSubUnits){
			Bullet go = Instantiate(Projectile, ss.transform.position,
					Quaternion.identity) as Bullet;
			go.Initialize(playerNumber);
		}
	}

}


