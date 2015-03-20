using UnityEngine;
using System.Collections;

//'Canons' type of unit
//XXX THIS CLASS CANNOT HAVE Awake()
public class Canons : UnitController{

	//
	//Attacking FSM
	//

	//States
	private enum AttackState {None, Charging, Cooldown};
	private AttackState attacking;

	//Timers
	//
	//Time until full charge
	public float ChargingTime;
	private Timer chargeTimer;


	//
	//UI hook
	//

	//How much percent has our timer gone through?
	//public float PercentCharge {
		//get {
			//return chargeTimer.getTime + ChargingTime ) /


	//
	//Spawn points
	//

	public Vector2[] SpawnPoints;

	//
	//Macro methods
	//

	//tell SubCanons to shoot
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
		controlledSubUnits.CreateChildren(ChildUnit, SpawnPoints);
		chargeTimer = new Timer();
	}

	//Move and attack
	void FixedUpdate(){

		//Attacking
		//

		if(input.a){
		}

	}

}

