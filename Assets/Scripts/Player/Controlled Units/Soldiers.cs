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
	// ie the whole animation stops at PostAttackFrame
	//
	//I define 'frame' to be 1 call to FixedUpdate()
	public int PreAttackFrame = 10;
	public int AttackShootFrame = 10;


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
		//attacking has 3 distinct parts
		//and it also stops movement
		if(attacking){
			frame++;

			if(frame == 1){
				//print("Getting ready...");
				foreach(SubSoldier ss in controlledSubUnits){
					ss.StartCatchingUp();
				}
			}else if(frame == PreAttackFrame){
				//print("...Fire!...");
				Attack();
			}else if(frame == PreAttackFrame + AttackShootFrame){ 
				//print("...Done");
				attacking = false;
			}

			//let units act with null input
			foreach(SubSoldier ss in controlledSubUnits){
				ss.InputMessage(new InputStruct());
				ss.Act();
			}
			return;
		}

		//if attack, change state, stop moving
		if(input.a){
			attacking = true;
			frame = 0;
			//Don't give sub units movement input
			input.x = 0;
			input.y = 0;
		}


		//Pressing back changes formation
		if(input.x == backdir && lastinput.x != backdir){
			currentFormation = currentFormation == GoalPositionsHorizontal ? GoalPositionsVertical : GoalPositionsHorizontal;
			foreach(SubSoldier ss in controlledSubUnits){
				ss.StartCatchingUp();
			}

		}


		//Iterate over units to give input and move them
		Vector2 relativeTo = controlledSubUnits.At(0).rigidbody2D.position;
		int i = 0;
		foreach(SubSoldier ss in controlledSubUnits){
			ss.InputMessage(input);
			//TODO only update when relevant?
			ss.GoalPosition = currentFormation[i++] + relativeTo;
			ss.Act();
		}
	}

}


