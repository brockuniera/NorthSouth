using UnityEngine;
using System.Collections;

//for List<T>
using System.Collections.Generic;

//TODO
//	Incorporate into PlayerController.cs

//Chooses who is or isn't active at any given point in time
//Contains a list of all currently selectable units for a player
//Order is determined by adding, not this componenets problem
public class ActiveChooser : MonoBehaviour {

	//
	//Private data
	//

	//Our list of active Units
	//Every FixedUpdate, we change who can or can't move
	public List<Selectable> _activeUnits;

	//index of current Selectable in list
	public int _index = -1;


	//
	//Mutators for list
	//

	//Build list
	public void AddSelectableToList(Selectable sel){
		sel.isSelected = false;
		_activeUnits.Add(sel);
	}

	//Initialize list; setup the index
	public void InitializeList(){
		if(_activeUnits.Count == 0){
			_index = -1;
			return;
		}

		_index = 0;
		
		_activeUnits[_index].isSelected = true;
	}

	//Reset list
	public void Clear(){
		_activeUnits.Clear();
		_index = -1;
	}

	//
	//Unity callbacks
	//

	void Start(){
		_activeUnits = new List<Selectable>();
	}

	//If user presses B, changes
	void Update(){
		//We don't care if player didn't press "Fire2" to change controlled unit
		//Or if our list is empty
		if(!Input.GetButtonDown("Fire2") || _activeUnits.Count == 0 ||
				_index == -1){
			return;
		}

		//XXX
		Debug.Log("Changed Controlled!");

		//Tell current Selectable that he is no longer active
		_activeUnits[_index].isSelected = false;

		//Increment
		_index++;

		//Implement circular list
		if(_index == _activeUnits.Count){
			_index = 0;
		}

		_activeUnits[_index].isSelected = true;

	}

}

