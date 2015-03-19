using UnityEngine;
using System.Collections;

//'Horses' type of unit
//XXX THIS CLASS CANNOT HAVE awake()
public class Horses : UnitController{
	//
	//Very basic FSM for attacking
	//

	//The states we can be in for attacking:
	// None -- We are not attacking
	// Attacking -- We are attacking
	// MoveNoAttack -- We just attacked, so we can't attack right now
	private enum AttackState {None, Attacking, MoveNoAttack};
	private AttackState attacking = AttackState.None;
	//Timer for above states
	private Timer attackTimer;
	//Times for each of the above states
	public float AttackingTime;
	public float MoveNoAttackTime;

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
	private Vector2 []HorizontalFormation = {
		new Vector2( 0,  0),
		new Vector2(-2,  0),
		new Vector2(-4,  0)
	};
	//P1
	//0
	//1
	//2
	private Vector2 []VerticalFormation = {
		new Vector2( 0,  0),
		new Vector2( 0,  -2),
		new Vector2( 0,  -4)
	};
	//Reference to which of the above formations to use
	private Vector2 []currentFormation;


	//
	//Macro methods
	//

	//tell SubHorses to shoot
	private void Attack(){
		foreach(ControlledUnit cu in controlledSubUnits){
			cu.Attack(); 
		}
	}

	//tell SubHorses to catchup
	private void StartCatchingUp(bool precise = false){
		foreach(SubHorse sh in controlledSubUnits){
			sh.StartCatchingUp(precise);
		}
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
		controlledSubUnits.CreateChildren(ChildUnit, VerticalFormation);
		//set initial formation
		currentFormation = HorizontalFormation;
		attackTimer = new Timer();
		backTimer = new Timer();
	}

	//Move and attack
	void FixedUpdate(){

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

		//Attacking
		//


		if(input.a){
			StartCatchingUp(true);
		}

		//
		//Final step; passing input
		//

		Vector2 relativeTo = controlledSubUnits.At(0).rigidbody2D.position;

		//Iterate over units to give input, goal pos, and move them
		int i = 0;
		foreach(SubHorse sh in controlledSubUnits){
			sh.InputMessage(input);
			sh.GoalPosition = currentFormation[i++] + relativeTo;
			sh.Act();
		}
	}

}

