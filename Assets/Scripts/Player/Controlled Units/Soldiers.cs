using UnityEngine;
using System.Collections;

//'Soldiers' type of unit
//XXX THIS CLASS CANNOT HAVE awake()
public class Soldiers : UnitController{
	//
	//Very basic FSM for attacking
	//

	//The states we can be in for attacking:
	// None -- We are not attacking
	// Warmup -- We can't move and will shoot soon
	// -- Shooting happens here --
	// Cooldown -- We just shot and still can't move
	// MoveNoShoot -- We can move, but we can't shoot
	private enum AttackState {None, Warmup, Cooldown, MoveNoShoot};
	private AttackState attacking = AttackState.None;
	//Timer for above states
	private Timer attackTimer;
	//Times for each of the above states
	public float WarmupTime;
	public float CooldownTime;
	public float MoveNoShootTime;

	//
	//Tap back to reform units
	//

	//Holding back for within this much time counts as a 'tap' and reforms the units
	public float MaxBackButtonReformTime;
	//The timer for above
	private Timer backTimer;

	//
	//Child goal positions
	//

	//TODO player specific
	//TODO player specific
	//TODO player specific
	//TODO player specific

	//P1
	//2 1 0
	//5 4 3
	private Vector2 []HorizontalFormation = {
		new Vector2( 0,  0),
		new Vector2(-2,  0),
		new Vector2(-4,  0),
		new Vector2( 0, -2),
		new Vector2(-2, -2),
		new Vector2(-4, -2)
	};
	//Slightly staggered version of HorizontalFormation
	private Vector2 []StaggeredFormation = {
		new Vector2( 0,  0.000f),
		new Vector2(-2, -0.666f),
		new Vector2(-4, -1.333f),
		new Vector2( 0, -2.000f),
		new Vector2(-2, -2.666f),
		new Vector2(-4, -3.333f)
	};
	//P1
	//5 0
	//3 1
	//4 2
	private Vector2 []VerticalFormation = {
		new Vector2( 0,  0),
		new Vector2( 0,  -2),
		new Vector2( 0,  -4),
		new Vector2(-2, -3),
		new Vector2(-2, -5),
		new Vector2(-2,  -1)
	};
	//Reference to which of the above formations to use
	private Vector2 []currentFormation;


	//
	//Macro methods
	//

	//tell SubSoldier to shoot
	private void Attack(){
		foreach(ControlledUnit cu in controlledSubUnits){
			cu.Attack(); 
		}
	}

	//tell SubSoldiers to catchup
	private void StartCatchingUp(bool precise = false){
		foreach(SubSoldier ss in controlledSubUnits){
			ss.StartCatchingUp(precise);
		}
	}

	//Changes units to staggered version if not already
	private void StaggerFormation(){
		currentFormation = currentFormation == VerticalFormation
			? VerticalFormation : StaggeredFormation;
	}

	//Changes units to staggered version if not already
	private void UnStaggerFormation(){
		currentFormation = currentFormation == StaggeredFormation
			? HorizontalFormation : currentFormation;
	}

	//Changes units formation
	private void ChangeFormation(){
		currentFormation = currentFormation == VerticalFormation
			? HorizontalFormation : VerticalFormation;
	}

	//
	//Unity Callbacks
	//

	void Start(){
		//When this class is created, it spawns units too
		controlledSubUnits.CreateChildren(ChildUnit, HorizontalFormation);
		//set initial formation
		currentFormation = HorizontalFormation;
		attackTimer = new Timer();
		backTimer = new Timer();
	}

	//Move and attack
	void FixedUpdate(){

		//Attack state handling
		switch(attacking){
			//Just pressed attack; havent shot; can't input; moving to goals
			case AttackState.Warmup:
				//let units act, but with null input
				//to let subsoldiers move to goal pos
				foreach(SubSoldier ss in controlledSubUnits){
					ss.Act();
				}

				if(attackTimer.isDone){
					Attack();

					attackTimer.SetTimer(CooldownTime);
					attacking = AttackState.Cooldown;
				}
				return;
				//Just shot; can't input
			case AttackState.Cooldown:
				if(attackTimer.isDone){
					attackTimer.SetTimer(MoveNoShootTime);
					attacking = AttackState.MoveNoShoot;
				}
				return;
				//Post shooting; can move, can't attack
			case AttackState.MoveNoShoot:
				input.a = false;

				if(attackTimer.isDone){
					attacking = AttackState.None;
				}
				break; //no return
		}


		//
		//Process Input
		//
		
		//Tapping back
		//

		//If back is pressed, start timer for it
		if(input.x == backdir && lastinput.x != backdir)
			backTimer.SetTimer(MaxBackButtonReformTime);

		//If back is release, check if it was a tap
		if(input.x != backdir && lastinput.x == backdir){
			//Pressing back changes formation
			//Only reform if pressing back was considered a tap
			if(!backTimer.isDone){
				ChangeFormation();
				StartCatchingUp();
			}
			//Reset timer whenver back is released
			backTimer.SetTimer(MaxBackButtonReformTime);
		}

		//Position of leader (ie, At(0)), so other units can
		//be placed relative to him
		Vector2 relativeTo = controlledSubUnits.At(0).rigidbody2D.position;

		//Attacking
		//

		if(input.a){
			StaggerFormation();
			int j = 0;
			foreach(SubSoldier ss in controlledSubUnits)
				ss.GoalPosition = currentFormation[j++] + relativeTo;
			StartCatchingUp(true);

			//State change
			attacking = AttackState.Warmup;
			attackTimer.SetTimer(WarmupTime);

			//No movement input
			input.x = 0;
			input.y = 0;

			foreach(SubSoldier ss in controlledSubUnits){
				ss.InputMessage(input);
				ss.Act();
			}

			return;
		}

		//
		//Final step; passing input
		//

		UnStaggerFormation();

		//Iterate over units to give input, goal pos, and move them
		int i = 0;
		foreach(SubSoldier ss in controlledSubUnits){
			ss.InputMessage(input);
			ss.GoalPosition = currentFormation[i++] + relativeTo;
			ss.Act();
		}
	}

}

