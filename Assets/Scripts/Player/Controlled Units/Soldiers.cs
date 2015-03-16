using UnityEngine;
using System.Collections;

//'Soldiers' type of unit
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
			currentFormation = currentFormation == GoalPositionsHorizontal ? GoalPositionsVertical : GoalPositionsHorizontal;
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
			}else if(frame == PreAttackFrame){
				//print("...Fire!...");
				Attack();
			}else if(frame == PreAttackFrame + AttackShootFrame){ 
				//print("...Done");
				attacking = false;
			}

			//We don't want to process movement if we're attacking
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

		//iterate over units to give input and move them
		Vector2 relativeTo = controlledSubUnits.At(0).rigidbody2D.position;
		int i = 0;
		foreach(SubSoldier ss in controlledSubUnits){
			ss.InputMessage(input);
			//TODO only update when relevant
			ss.GoalPosition = currentFormation[i++] + relativeTo;
			ss.Act();
		}
	}

}


