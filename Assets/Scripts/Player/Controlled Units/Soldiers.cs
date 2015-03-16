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

	//XXX What are these relative to?
	// Solutions
	//  Always relative to 0j
	public Vector2 []GoalPositions;
	//P1
	//2 1 0
	//5 4 3


	private void UpdateGoalPositions(){
		int i = 0;
		foreach(SubSoldier ss in controlledSubUnits){
			ss.GoalPosition = GoalPositions[i++];
		}
	}


	//tell SubSoldier to shoot
	private void Attack(){
		foreach(ControlledUnit cu in controlledSubUnits){
			cu.Attack(); 
		}
	}

	//
	//Unity Callbacks
	//

	void Start(){
		//When this class is created, it spawns units too
		controlledSubUnits.CreateChildren(ChildUnit, GoalPositions);
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
		foreach(ControlledUnit cu in controlledSubUnits){
				cu.InputMessage(input);
				cu.Act();
		}
	}

}


