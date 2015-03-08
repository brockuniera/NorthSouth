using UnityEngine;
using System.Collections;

//For List<T>
using System.Collections.Generic;

//Abstract base class for all _controlledSubUnits
public abstract class AbstractUnitController : MonoBehaviour{

	//Player numbers
	protected int playerNumber = -1; //initialize to bad value for saftey
	public void SetPlayerNumber(int pn){ playerNumber = pn; }

	//Inputs
	protected InputStruct input;
	public void InputMessage(InputStruct i){ input = i; }

	//
	//SubUnits
	//

	//The actual list
	protected List<AbstractControlledUnit> _controlledSubUnits;

	//
	//Methods for the list
	//

	//Remove someone from the list
	public void RemoveFromList(AbstractControlledUnit acu){
		//TODO change this in the 'recursive class' version
		if(acu != null)
			_controlledSubUnits.Remove(acu);
	}


	//
	//Unity Callbacks
	//

	void Awake(){
		//Initialize list of things
		_controlledSubUnits = new List<AbstractControlledUnit>();

		foreach(Transform child in transform){
			//Add child of correct type to list
			var comp = child.GetComponent<AbstractControlledUnit>();
			if(comp){
				_controlledSubUnits.Add(comp);
			}

			//Set child's layer to correct thing
			child.gameObject.layer = LayerMask.NameToLayer("Player " + 
					(playerNumber == 1 ? '1' : '2'));
		}
	}
		


}

