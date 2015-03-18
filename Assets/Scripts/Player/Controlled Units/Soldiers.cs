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
	//Initial child goal positions
	//

	public Vector2 []GoalPositionsHorizontal;
	//P1
	//2 1 0
	//5 4 3
	public Vector2 []GoalPositionsVertical;
	//P1
	//5 0
	//3 1
	//4 2
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

	//Changes units formation, right now there are only two
	private void ChangeFormation(){
		currentFormation = currentFormation == GoalPositionsHorizontal
			? GoalPositionsVertical : GoalPositionsHorizontal;
	}

	//
	//Unity Callbacks
	//

	//XXX TEST CODE
	void Update(){
		if(Input.GetKeyDown("9")){
			//Component temp;
			//temp = controlledSubUnits[0];
			//controlledSubUnits[0] = controlledSubUnits[3];
			//controlledSubUnits[3] = temp;
		}
	}//XXX TEST CODE

	void Start(){
		//When this class is created, it spawns units too
		controlledSubUnits.CreateChildren(ChildUnit, GoalPositionsHorizontal);
		//set initial formation
		currentFormation = GoalPositionsHorizontal;
		attackTimer = new Timer();
		backTimer = new Timer();
	}

	//Move and attack
	void FixedUpdate(){

		//If back is pressed, start timer for it
		if(input.x == backdir && lastinput.x != backdir)
			backTimer.SetTimer(MaxBackButtonReformTime);

		//Attack state handling
		switch(attacking){
			//Just pressed attack; havent shot; can't input; moving to goals
			case AttackState.Warmup:
				//let units act, but with null input
				//to let subsoldiers move to goal pos
				foreach(SubSoldier ss in controlledSubUnits){
					ss.Act();
				}

				if(attackTimer.IsDone){
					Attack();

					attackTimer.SetTimer(CooldownTime);
					attacking = AttackState.Cooldown;
				}
				return;
				//Just shot; can't input
			case AttackState.Cooldown:
				if(attackTimer.IsDone){
					attackTimer.SetTimer(MoveNoShootTime);
					attacking = AttackState.MoveNoShoot;
				}
				return;
				//Post shooting; can move, can't attack
			case AttackState.MoveNoShoot:
				input.a = false;

				if(attackTimer.IsDone){
					attacking = AttackState.None;
				}
				break; //no return
		}


		//
		//Process Input
		//

		//Position of leader (ie, At(0)), so other units can
		//be placed relative to him
		//TODO Cache leader's rigidbody2D
		Vector2 relativeTo = controlledSubUnits.At(0).rigidbody2D.position;

		//if attack: move units slightly, change state, stop moving
		if(input.a){
			//Stagger units slightly
			//TODO random in any direction, make .1f or whatev a public var
			if(input.x != backdir){
				relativeTo.x -= .01f * playerNumber == 1 ? 1 : -1;
			}

			StartCatchingUp(true);

			//State setup
			//

			attacking = AttackState.Warmup;
			attackTimer.SetTimer(WarmupTime);

			//Don't give sub units movement input
			input.x = 0;
			input.y = 0;

			//XXX For whatever reason, this is mutually exclusive?
			// Its probably because of the input.x = 0 stuff
		}else if(lastinput.x == backdir && input.x != backdir){
			//Pressing back changes formation
			//Only reform if pressing back was considered a tap
			if(!backTimer.IsDone){
				ChangeFormation();
				StartCatchingUp();
			}
			//Reset timer whenver back is released
			backTimer.SetTimer(MaxBackButtonReformTime);
		}


		//
		//Final step; passing input
		//

		//Iterate over units to give input, goal pos, and move them
		int i = 0;
		foreach(SubSoldier ss in controlledSubUnits){
			ss.InputMessage(input);
			//TODO only update when relevant:
			// Only when there is input!!
			ss.GoalPosition = currentFormation[i++] + relativeTo;
			ss.Act();
		}
	}

}

