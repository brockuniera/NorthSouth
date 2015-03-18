using UnityEngine;
using System.Collections;

//'Soldiers' type of unit
//XXX THIS CLASS CANNOT HAVE AWAKE()
public class Soldiers : UnitController{


	//
	//Very basic FSM, for attacking
	//

	//literally just 1 state
	private bool attacking = false;
	//frame counter for animation timing in FixedUpdate
	private int frame = 0;

	//For attack animation lead up and cool down
	// These happen in consecutive order
	public int PreAttackFrame = 10;
	public int AttackShootFrame = 10;

	//
	//Tap back to reform units
	//

	///<summary>
	///Holding back for within this many frames counts as a tap and reforms the units
	///<\summary>
	public int MaxBackButtonReformFrames;
	//The timer for above
	private int backTimer;


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
	}
	//XXX TEST CODE

	void Start(){
		//When this class is created, it spawns units too
		controlledSubUnits.CreateChildren(ChildUnit, GoalPositionsHorizontal);
		currentFormation = GoalPositionsHorizontal;
	}

	//Move and attack
	void FixedUpdate(){

		//Increment timer for pressing back
		if(input.x == backdir)
			backTimer++;

		//attacking has 3 distinct parts
		//and it also stops movement
		if(attacking){
			frame++;

			if(frame == 1){
				//print("Getting ready...");
				StartCatchingUp(true);
			}else if(frame == PreAttackFrame){
				//print("...Fire!...");
				Attack();
			}else if(frame == PreAttackFrame + AttackShootFrame){ 
				//print("...Done");
				attacking = false;
			}

			//let units act, but with null input
			foreach(SubSoldier ss in controlledSubUnits){
				ss.InputMessage(new InputStruct());
				ss.Act();
			}
			return;
		}


		//
		//Initial attacking

		//Position of leader (ie, At(0)), so other units can
		//be placed relative to him
		//TODO Cache leader's rigidbody2D
		Vector2 relativeTo = controlledSubUnits.At(0).rigidbody2D.position;

		//if attack, change state, stop moving
		if(input.a){
			attacking = true;
			frame = 0;
			//Move units slightly forward/backward: P1/P2
			//TODO random in any direction, make .1f or whatev a public var
			if(input.x != backdir){
				relativeTo.x -= .01f * playerNumber == 1 ? 1 : -1;
			}
			//Don't give sub units movement input
			input.x = 0;
			input.y = 0;
		}else if(lastinput.x == backdir && input.x != backdir){
			//Pressing back changes formation
			//Only reform if pressing back was considered a tap
			if(backTimer < MaxBackButtonReformFrames){
				//Reform units
				currentFormation = currentFormation == GoalPositionsHorizontal ? GoalPositionsVertical : GoalPositionsHorizontal;
				StartCatchingUp(false);
			}
			//Reset timer whenver back is released
			backTimer = 0;
		}


		//
		//Final step; passing input
		//

		//Iterate over units to give input and move them
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


