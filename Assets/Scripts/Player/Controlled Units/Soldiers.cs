using UnityEngine;
using System.Collections;

//'Soldiers' type of unit
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
	//enum SoldierState {Normal, PreAttack, AttackShoot, PostAttack};
	//private SoldierState state = SoldierState.Normal;

	//For attack animation leadup and cool down
	//These happen in consecutive order
	// ie the whole animation stops at PostAttackFrame
	//
	//'Frames' means FixedUpdate()
	public int PreAttackFrame = 10;
	public int AttackShootFrame = 10;

	//frame counter
	private int _frame = 0;

	//
	//Bullets
	//
	

	public Rigidbody2D Bullet;
	public float BulletSpeed = 10f;


	//
	//Code
	//


	void Start(){
		_diagSpeedComponent = MoveSpeed / Mathf.Sqrt(2);

		_rb2d = rigidbody2D;
	}

	//move around, attack
	void FixedUpdate(){
		
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
			}else{
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

		//Directions!
		//   +y  
		//    |
		//-x -0- +x
		//    |
		//   -y  
		
		//if both inputs, use _diagSpeedComponent
		float speed = input.x != 0 && input.y != 0 ? _diagSpeedComponent : MoveSpeed;

		_rb2d.velocity = new Vector2(input.x, input.y) * speed;
		
	}

	//tell sub units to shoot
	private void Shoot(){
		Instantiate(Bullet, _rb2d.position, Quaternion.identity);
	}

}


