using UnityEngine;
using System.Collections;

//for List<T>
using System.Collections.Generic;

//probably make an abstract class or something
//like an interface?
[RequireComponent (typeof (PlayerInputHandler))]

//children are controlled units
public class PlayerController : MonoBehaviour {

	//GOs I can control
	private List<AbstractUnit> units;
	private AbstractUnit currentUnit;

	//input handler
	private PlayerInputHandler input;

	//input to be given to units
	private InputStruct unitInput;

	void Start(){
		//populate list of units, who are probably children?
		units = new List<AbstractUnit>();
		
		foreach(Transform child in transform){
			var comp = child.GetComponent<AbstractUnit>();
			if(comp)
				units.Add(comp);
		}

		//assign the first child to currentUnit
		if(units.Count == 0)
			Debug.LogError("No children of player with AbstractUnit script!");
		else
			currentUnit = units[0];

		//input reference
		input = GetComponent<PlayerInputHandler>();
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
			ChangeControlledUnit();
		}

	}

	//This FixedUpdate() should happen first, before any other unit
	// is there a better way to ensure this than by doing it manually?
	// 1) We could enumerate thru children and tell them to run
	void FixedUpdate(){
		//pass input to unit
		currentUnit.SetInput(unitInput);

		//clear input
		// for use with buffering
		unitInput.x = PlayerInputHandler.XDefault;
		unitInput.y = PlayerInputHandler.YDefault;
		unitInput.a = PlayerInputHandler.ADefault;
	}

	private void ChangeControlledUnit(){
		//TODO lol
	}
}
