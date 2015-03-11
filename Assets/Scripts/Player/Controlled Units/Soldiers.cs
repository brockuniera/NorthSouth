using UnityEngine;

//'Soldiers' type of unit
public class Soldiers : AbstractUnitController{


	//
	//Very basic FSM, for attacking
	//

	//literally just 1 state
	private bool _attacking = false;
	//frame counter for animation timing in FixedUpdate
	private int _frame = 0;

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

	public Vector2 []GoalPositions;


	//
	//Unity Callbacks
	//

	void Start(){
		//When a soldiers is created, it spawns units too
		_controlledSubUnits.CreateChildren(ChildUnit, GoalPositions);
	}

	//Move and attack
	void FixedUpdate(){

		//attacking has 3 distinct parts
		//and it also stops movement
		if(_attacking){
			_frame++;

			if(_frame == 1){
				print("Getting ready...");
			}else if(_frame == PreAttackFrame){
				print("...Fire!...");
				Attack();
			}else if(_frame == PreAttackFrame + AttackShootFrame){ 
				print("...Done");
				_attacking = false;
			}

			//We don't want to process movement if we're attacking
			return;
		}

		//if attack, change state, stop moving
		if(input.a){
			_attacking = true;
			_frame = 0;
			//Don't give sub units movement input
			input.x = 0;
			input.y = 0;
		}

		//iterate over units to give input and move them
		foreach(AbstractControlledUnit acu in _controlledSubUnits){
				acu.InputMessage(input);
				acu.Act();
		}

		
	}

	
	//tell SubSoldier to shoot
	private void Attack(){
		foreach(Component co in _controlledSubUnits){
			AbstractControlledUnit acu = (AbstractControlledUnit)co;
			acu.Attack(); //if we get a NullReferenceException, we messed up somewhere: it should be unrecoverable
		}
	}

}


