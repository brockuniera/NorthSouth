using UnityEngine;
using System.Collections;

//'Canons' type of unit
//XXX THIS CLASS CANNOT HAVE Awake()
public class Canons : UnitController{

	//
	//Attacking FSM
	//

	//States
	private enum AttackState {None, Charging, Cooldown, PostShootSlowMove};
	private AttackState attacking;

	//Timers
	//
	//Time until full charge
	public float ChargingTime;
	//Time until done cooling down
	public float CooldownTime;
	//Time until full charge
	public float PostShootSlowMoveTime;
	private Timer attackTimer;

	//
	//UI hook
	//

	//How much percent has our timer gone through?
	//TODO
	//
	// .getTime = -1 * Charging time
	// |                .getTime = 0
	// |                | 
	// \/               \/
	// 0%              100%
	// [-----------------]
	// PRESS...  ^     SHOOT
	//           |
	//           .getTime = -1 * ChargingTime + .5 ChargingTime
	//
	//
	//
	public float percentCharge {
		get {
			if(attacking == AttackState.Charging)
				//return Mathf.Max(1.0f, (attackTimer.getTime + ChargingTime ) / ChargingTime); // Alternative version, using Mathf.Max()
				return (attackTimer.getTime + ChargingTime ) / ChargingTime;
			else
				return 0.0f;
		}
	}


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

	private void SetSlowSpeed(){
		foreach(SubCanon sc in controlledSubUnits){
			sc.isSlow = true;
		}
	}

	private void SetNormalSpeed(){
		foreach(SubCanon sc in controlledSubUnits){
			sc.isSlow = false;
		}
	}

	//
	//Unity Callbacks
	//

	void Start(){
		//When this class is created, it spawns units too
		controlledSubUnits.CreateChildren(ChildUnit, SpawnPoints);
		attacking = AttackState.None;
		attackTimer = new Timer();
	}

	//Move and attack
	void FixedUpdate(){
		//TODO Canon distance shooting, reticule

		//Attacking
		//

		//on down: start chage
		if((attacking == AttackState.None || attacking == AttackState.PostShootSlowMove) &&
				input.a && !lastinput.a){
			attacking = AttackState.Charging;
			attackTimer.SetTimer(ChargingTime);
			SetSlowSpeed();
			//Debug.Log("Key down!");
		}

		switch(attacking){
			case AttackState.None:
				break;
			case AttackState.Charging:
				//on key up or timeout: shoot
				if((!input.a && lastinput.a) || attackTimer.isDone){
					Attack();
					attacking = AttackState.Cooldown;
					attackTimer.SetTimer(CooldownTime);
				}
				break;
			case AttackState.Cooldown:
				if(attackTimer.isDone){
					attacking = AttackState.PostShootSlowMove;
					attackTimer.SetTimer(PostShootSlowMoveTime);
				}
				break;
			case AttackState.PostShootSlowMove:
				if(attackTimer.isDone){
					attacking = AttackState.None;
					SetNormalSpeed();
				}
				break;

		}

		//Send Input
		//

		//Iterate over units to give input, goal pos, and move them
		foreach(SubCanon sc in controlledSubUnits){
			sc.InputMessage(input);
			sc.Act();
		}

	}

}

