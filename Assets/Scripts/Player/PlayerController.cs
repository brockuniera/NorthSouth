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
	private UnitController currentUnit;

	//input handler
	private PlayerInputHandler input;

	//input to be given to units
	private InputStruct unitInput;

	void Awake(){
		//
		//Setup references

		_chooser = GetComponent<Chooser>();
		input = GetComponent<PlayerInputHandler>();

		//get player number from name (probably a bad idea)
		//split name by spaces and get the second string returned
		string s = name.Split(' ')[1];
		if(s != null)
			input.SetPlayerKeyBindings(System.Convert.ToInt32(s));
		else{
			Debug.LogError("This Player has a malformed name! : " + name );
			Application.Quit(); // oh man ! :OOOO
		}

		//
		//Setup layers; layer is based off name
		gameObject.layer = LayerMask.NameToLayer(name);
	}
	
	void Update(){
		//
		//Process input
		//

		input.UpdateInput();

		//generate input struct
		//buffer inputs as well
		// ie if input.x is not default, then update it
		unitInput.x = input.xAxis != PlayerInputHandler.XDefault ?
			input.xAxis : unitInput.x;
		unitInput.y = input.yAxis != PlayerInputHandler.YDefault ?
			input.yAxis : unitInput.y;
		unitInput.a = input.aButton != PlayerInputHandler.ADefault ?
			input.aButton : unitInput.a;

		//change unit on b button
		// also have a method to directly change to a unit, thank you
		if(input.bButton){
			currentUnit = _chooser.GetNextUnit();
		}

	}

	//This FixedUpdate() should happen first, before any other unit
	// is there a better way to ensure this than by doing it manually?
	// 1) We could enumerate thru children and tell them to run
	void FixedUpdate(){
		//pass input to unit
		if(currentUnit != null)
			currentUnit.InputMessage(unitInput);

		//clear input
		// for use with buffering
		unitInput.x = PlayerInputHandler.XDefault;
		unitInput.y = PlayerInputHandler.YDefault;
		unitInput.a = PlayerInputHandler.ADefault;
	}

}

