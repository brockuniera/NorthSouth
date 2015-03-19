using UnityEngine;
using System.Collections;

//A single SubSoldier, the actual soldiers that move around on screen with the sprites
public class SubSoldier : ControlledUnit {

	//
	//Move Speeds
	//

	//Input speeds
	//
	//'Rook' move speed
	public float MoveSpeed;
	//'Bishop' move speed, derived from MoveSpeed
	private float diagSpeedComponent;

	//Catchup Timer
	//
	//The timer
	private Timer catchupTimer;
	//Time until action
	public float AutoCatchupTime;


	//'Catchup' variables, used to move to GoalPosition
	//
	//Are we currently trying to return to the squad?
	private bool isCatchingUp = false;
	//How far until we are caught up?
	// This is computed once for every attempt to catchup
	// CatchupMinDist, RandomMinDistFactor, and Random.value are used
	private float minDist;

	//Speed SubSoldier moves at to catchup
	public float CatchupSpeed;
	//Minimum distance to try catching up
	public float CatchupMinDist;
	//Random[0,1] is multiplied by this and added to CatchupMinDist
	public float RandomMinDistFactor;

	///<summary>
	///Squared Radius to try and move precisley within
	///<\summary>
	public float PreciseCatchupDist;

	//Speed SubSoldier moves at to catchup slowly, ie when already close
	public float CatchupSpeedSlow;

	//Maximum distance to move slowly within when catching up
	public float SlowCatchupDist;

	//Goal Position
	// this unit attempts to move to its goal position
	// modified by Soldiers class
	public Vector2 GoalPosition;

	//
	//Prefabs
	//

	//Bullet prefab to use for shooting
	public Bullet Projectile;

	//
	//Unity Callbacks
	//

	void Start(){
		//Moving diagonally
		diagSpeedComponent = MoveSpeed / Mathf.Sqrt(2);
		catchupTimer = new Timer();
	}

	void Update(){
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
			//TODO If we're doing a precise catchup over a short distance,
			// we should play something like a crouching animation to make it
			// look more solid or something
		}
	}

	//
	//Movement Helpers
	//

	private void SetVelocityFromInput(){
		//if both inputs, use diagSpeedComponent
		float speed = input.x != 0 && input.y != 0 ? diagSpeedComponent : MoveSpeed;
		rb2d.velocity = new Vector2(input.x, input.y) * speed;
	}

	//
	//Overrides
	//

	public override void Act(){

		//Auto Catchup timer
		if(!isCatchingUp){
			if(catchupTimer.isDone){
				catchupTimer.SetTimer(AutoCatchupTime);
				StartCatchingUp(true);
			}
		}

		//Movement
		//

		if(isCatchingUp){
			catchupTimer.SetTimer(AutoCatchupTime);
			Vector2 dist = GoalPosition - rb2d.position;
			float distSqrMag = dist.sqrMagnitude;
			if(distSqrMag > minDist){
				//I'm too far; I'll catchup to the unit!
				Vector2 catchup = dist;
				catchup.Normalize();

				//If close enough...
				if(distSqrMag < SlowCatchupDist){
					//...then move slower and allow input...
					SetVelocityFromInput();
					catchup *= CatchupSpeedSlow;
					rb2d.velocity += catchup;
				}else{
					//...otherwise we move with our faster CatchupSpeed
					catchup *= CatchupSpeed;
					rb2d.velocity = catchup;
				}
			}else{
				//I caught up!
				isCatchingUp = false;
			}
		}
		if(!isCatchingUp){
			//I'm within range; I'll move with the unit!
			SetVelocityFromInput();
		}

		//DEBUG
		//if(playerNumber == 1 && this == transform.parent.GetComponent<ChildrenList>().At(1))
		//Debug.Log("Current: " + rb2d.position + " Goal: " + GoalPosition + " catchup: " + catchup);

	}

	//Spawn bullets
	public override void Attack(){
		rb2d.velocity = Vector2.zero;
		Instantiate(Projectile, rb2d.position, Quaternion.identity);
	}

	//TODO Oncollsion, we could swap leaders?
	void OnCollisionEnter2D(){
		//print("Test");
	}

	void OnCollisionExit2D(){
		StartCatchingUp();
	}

}

