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

	//
	//Attacking
	//

	//Attack states
	private enum AttackState {None, Attacking, MoveNoAttack};
	private AttackState attacking = AttackState.None;

	//Hurtbox prefab
	public Hurtbox hurtbox;
	private Hurtbox currentHurtbox;

	//Length hurtbox is present
	public float AttackingTime;
	//How long we have to wait before inputting attack again, after AttackingTime
	public float MoveNoAttackTime;
	//Timer for above values
	private Timer attackTimer;

	//Goal Position
	//
	// This unit attempts to move to its goal position
	// Modified by Horses class
	public Vector2 GoalPosition { get; set; }

	//Wrapping around
	//

	//Are we currently wrapping around the level?
	public bool isWrapping = false;

	//
	//Player 2 specifc
	//

	private GameObject guyOnHorse;
	private Animator guyOnHorseAnim;

	//
	//Unity Callbacks
	//

	void Start(){
		DiagonalAngle.Normalize();
		diagonalMovementVector = DiagonalAngle * DiagonalInputSpeed;
		attackTimer = new Timer();

		//Player 2 specific: Guy On Horse
		if(transform.childCount > 0){
			guyOnHorse = transform.GetChild(0).gameObject;
			guyOnHorseAnim = guyOnHorse.GetComponent<Animator>();
		}
	}

	void Update(){
		if(guyOnHorseAnim == null){
			anim.SetBool("Attacking", attacking == AttackState.Attacking);
		}else{
			//Player 2 specific
			guyOnHorseAnim.SetBool("Attacking", attacking == AttackState.Attacking);
		}

		anim.SetBool("IsMoving", rb2d.velocity != Vector2.zero);
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

	private void CatchUp(){
		//Catching up
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
		CatchUp();

		//Attacking
		//

		switch(attacking){
			case AttackState.None:
				if(input.a && input.x != backdir){
					attacking = AttackState.Attacking;
					attackTimer.SetTimer(AttackingTime);
					Attack();
				}
				break;
			case AttackState.Attacking:
				if(attackTimer.isDone){
					currentHurtbox.Die();
					currentHurtbox = null;
					attacking = AttackState.MoveNoAttack;
					attackTimer.SetTimer(MoveNoAttackTime);
				}
				break;
			case AttackState.MoveNoAttack:
				if(attackTimer.isDone){
					attacking = AttackState.None;
				}
				break;
		}
	}

	//spawn the hurt box
	public override void Attack(){
		Hurtbox hbox = (Hurtbox)Instantiate(hurtbox, rb2d.position, Quaternion.identity);
		hbox.transform.parent = transform;
		currentHurtbox = hbox;
	}

	void OnCollisionExit2D(){
		//We should start catching up when we get off of a rock or something
		StartCatchingUp();
	}

}

