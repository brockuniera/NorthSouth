using UnityEngine;
using System.Collections;
using System.Linq;

//'Horses' type of unit
//XXX THIS CLASS CANNOT HAVE awake()
public class Horses : UnitController{
	//
	//Tap back to reform units
	//

	//Holding back for within this much time counts as a 'tap' and reforms the units
	public float MaxBackButtonReformTime;
	//The timer for above
	private Timer backTimer;

	//
	//Active
	//

	//Have I been activated?
	public bool Active { get; set; }

	//
	//Child goal positions
	//

	//P1
	//2 1 0
	private Vector2 []HorizontalFormation;
	//P1
	//0
	//1
	//2
	private Vector2 []VerticalFormation;
	//Reference to which of the above formations to use
	private Vector2 []currentFormation;

	//
	//Wrapping around screen
	//

	//Time delay for wrapping around
	public float TimeToWrap;

	//
	//Limbo
	//
	
	// Respawn our horses at pos
	public void RespawnHorsesAt(Vector2 pos){
		controlledSubUnits.CreateChildren(
				ChildUnit,
				HorizontalFormation.Select(x => x + pos).ToArray(),
				true
				);
	}

	// Are our horses looping around the level right now?
	private bool limbo = false;
	public bool inLimbo {
		get { return limbo; }
		set{
			if(value){
				Active = false;
			}
			limbo = value;
		}
	}

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
		//Player specifc formations
		VerticalFormation = new Vector2[3];
		if(isPlayerOne){
			HorizontalFormation = new Vector2[3] {
				new Vector2(0,  0),
				new Vector2(-2,  0),
				new Vector2(-4,  0)
			};
			VerticalFormation = new Vector2[3] {
				new Vector2(0,  0),
				new Vector2(0,  -2),
				new Vector2(0,  -4)
			};
		}else if(isPlayerTwo){
			HorizontalFormation = new Vector2[3] {
				new Vector2(0,  0),
				new Vector2(2,  0),
				new Vector2(4,  0)
			};
			VerticalFormation = new Vector2[3] {
				new Vector2(0,  0),
				new Vector2(0,  -2),
				new Vector2(0,  -4)
			};
		}

		//set initial formation
		currentFormation = VerticalFormation;
		//When this class is created, it spawns units too
		controlledSubUnits.CreateChildren(ChildUnit, currentFormation);
		//set to not active
		Active = false; 
		//timer
		backTimer = new Timer();
	}

	//Move and attack
	void FixedUpdate(){

		//
		//Process Input
		//

		//Check if active
		//
		if(!Active){
			//Non empty input wakes us up
			if(!inLimbo && !input.isEmpty)
				Active = true;
			else
				//Don't act on empty input
				return;
		}

		//Tapping back
		//

		//If back is pressed, start timer for it
		if(input.x == backdir && lastinput.x != backdir){
			backTimer.SetTimer(MaxBackButtonReformTime);
		}

		//If back is release, check if it was a tap
		if(input.x != backdir && lastinput.x == backdir){
			//Pressing back changes formation
			//Only reform if pressing back was considered a tap
			if(!backTimer.isDone){
				ChangeFormation();
				StartCatchingUp();
			}
		}

		//Attacking
		//

		if(input.a && !lastinput.a){
			StartCatchingUp(true);
		}

		//
		//Final step; passing input
		//

		Vector2 relativeTo = controlledSubUnits.At(0).GetComponent<Rigidbody2D>().position;

		//Iterate over units to give input, goal pos, and move them
		int i = 0;
		foreach(SubHorse sh in controlledSubUnits){
			sh.InputMessage(input);
			sh.GoalPosition = currentFormation[i++] + relativeTo;
			sh.Act();
		}
	}

}

