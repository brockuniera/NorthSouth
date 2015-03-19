using UnityEngine;
using System.Collections;

//A single SubHorse
public class SubHorse : ControlledUnit {

	//
	//Movement
	//

	//Input speeds
	//
	//No input
	public float NoInputSpeed;
	//Forward input
	public float ForwardInputSpeed;
	//Diag Vector
	public Vector2 DiagonalAngle;
	//Up or Down input
	public float DiagonalInputSpeed;
	//This is the vector we actually use for diagonal movement
	private Vector2 diagonalMovementVector;

	//
	//'Catchup' variables, used to move to GoalPosition
	//
	
	//Are we currently trying to return to the squad?
	private bool isCatchingUp = false;
	//How far until we are caught up?
	// This is computed once for every attempt to catchup
	// CatchupMinDist, RandomMinDistFactor, and Random.value are used
	private float minDist;

	//Speed SubHorse moves at to catchup
	public float CatchupSpeed;
	//Minimum distance to try catching up
	public float CatchupMinDist;
	//Random[0,1] is multiplied by this and added to CatchupMinDist
	public float RandomMinDistFactor;

	//Squared Radius to try and move precisley within
	public float PreciseCatchupDist;

	//Speed SubHorse moves at to catchup slowly, ie when already close
	public float CatchupSpeedSlow;
	//Maximum distance to move slowly within when catching up
	public float SlowCatchupDist;

	//
	//Attacking
	//

	//Length hurtbox is present
	public float AttackingTime;
	//How long we have to wait before inputting attack again, after AttackingTime
	public float MoveNoAttackTime;
	//Timer for above values
	private Timer attackTimer;
	//Am I attacking?
	private bool attacking;


	//Goal Position
	//
	// this unit attempts to move to its goal position
	// modified by Horses class
	public Vector2 GoalPosition;

	//
	//Unity Callbacks
	//

	void Start(){
		DiagonalAngle.Normalize();
		diagonalMovementVector = DiagonalAngle * DiagonalInputSpeed;
		attackTimer = new Timer();
	}

	void Update(){
		anim.SetBool("Attacking", attacking);
	}

	//
	//Catchup methods
	//

	//I need to start catching up
	public void StartCatchingUp(bool precise = false){
		//If we're already catching up, don't do anything
		if(!isCatchingUp){
			isCatchingUp = true;
			//Calculate how close to get before stopping
			minDist = precise ? PreciseCatchupDist : CatchupMinDist + RandomMinDistFactor * Random.value;
		}
	}

	//
	//Movement Helpers
	//

	private void SetVelocityFromInput(){
		Vector2 movement;
		//move diagonally
		if(input.y != 0){
			movement = diagonalMovementVector;
			movement.y *= input.y;
		}else{
			if(input.x == forwarddir){
				movement = new Vector2(ForwardInputSpeed, 0);
			}else if(input.x == 0){
				movement = new Vector2(NoInputSpeed, 0);
			}else{
				movement = Vector2.zero;
			}
		}
		rb2d.velocity = movement;
	}

	//
	//Overrides
	//

	public override void Act(){

		//Movement
		//

		SetVelocityFromInput();

		if(isCatchingUp){
			Vector2 dist = GoalPosition - rb2d.position;
			float distSqrMag = dist.sqrMagnitude;
			if(distSqrMag > minDist){
				//I'm too far; I'll catchup to the unit!
				Vector2 catchup = dist.normalized * CatchupSpeed;
				rb2d.velocity += catchup;
			}else{
				//I caught up!
				isCatchingUp = false;
			}
		}

		//Attacking
		//

		//TODO alot

		if(input.a){
			if(!attacking){
				attacking = true;
				attackTimer.SetTimer(AttackingTime);
				Attack();
			}
		}

	}

	//spawn the hurt box
	public override void Attack(){
	}

	void OnCollisionExit2D(){
		StartCatchingUp();
	}

}

