using UnityEngine;
using System.Collections;

//for List<T>
using System.Collections.Generic;

[RequireComponent (typeof (PlayerInputHandler))]
[RequireComponent (typeof (Chooser))]

//children are controlled units
public class PlayerController : MonoBehaviour {

	//Chooses the current unit
	private Chooser _chooser;

	//The unit to give input to
	private UnitController _currentUnit;

	//input handler
	private PlayerInputHandler inputHandler;

	//input to be given to units
	private InputStruct unitInput;

	//Should input be cleared on next Update()?
	private bool toResetInput = false;

	//
	//Public methods
	//
	
	public void InitializeUnitController(){
		_chooser.Reset();
		_currentUnit = _chooser.GetNextUnit();
	}

	// Cycle to the next unit in our list of controlled units
	public void CycleUnits(){
		UnitController next = _chooser.GetNextUnit();
		if(_currentUnit != next){
			_currentUnit.InputMessage(InputStruct.Empty); //give null message
			_currentUnit = next;
		}
	}
	
	// Returns the Unit Controller currently receiving input
	public UnitController activeController {
		get { return _currentUnit; }
	}

	//
	//Unit selectors
	//

	public Horses horses{
		get { return (Horses)GetComponentsInChildren<Horses>()[0]; }
	}

	public Soldiers soldiers{
		get { return (Soldiers)GetComponentsInChildren<Soldiers>()[0]; }
	}

	public Canons cannons{
		get { return (Canons)GetComponentsInChildren<Canons>()[0]; }
	}

	//
	//Unity Callbacks
	//

	void Awake(){
		//
		//Setup references

		_chooser = GetComponent<Chooser>();
		inputHandler = GetComponent<PlayerInputHandler>();

		//get player number from name (probably a bad idea)
		//split name by spaces and get the second string returned
		if(name != "Player 1" && name != "Player 2")
			Application.Quit(); 

		string s = name.Split(' ')[1];
		inputHandler.SetPlayerKeyBindings(System.Convert.ToInt32(s));

		//
		//Setup layers; layer is based off name
		gameObject.layer = LayerMask.NameToLayer(name);
	}
	
	void Update(){
		//
		//Process and buffer input
		//

		//clear input if it was used
		// for use with buffering
		if(toResetInput){
			unitInput.x = PlayerInputHandler.XDefault;
			unitInput.y = PlayerInputHandler.YDefault;
			unitInput.a = PlayerInputHandler.ADefault;
			toResetInput = false;
		}

		inputHandler.UpdateInput();

		//Generate input struct
		//Buffer inputs as well
		// ie if input.x is not default, then update it
		unitInput.x = inputHandler.xAxis != PlayerInputHandler.XDefault ?
			inputHandler.xAxis : unitInput.x;
		unitInput.y = inputHandler.yAxis != PlayerInputHandler.YDefault ?
			inputHandler.yAxis : unitInput.y;
		unitInput.a = inputHandler.aButton != PlayerInputHandler.ADefault ?
			inputHandler.aButton : unitInput.a;

		//change unit on b button down
		// TODO also have a method to directly change to a unit, thank you
		if(inputHandler.bButton){
			CycleUnits();
		}

	}

	//This FixedUpdate() should happen first, before any other unit
	void FixedUpdate(){
		//pass input to unit, if it exists
		if(_currentUnit != null)
			_currentUnit.InputMessage(unitInput);

		//We used the input, we should mark it to be reset, but not reset it ourselves incase we dont get an update between fixed updates
		toResetInput = true;
	}

}

