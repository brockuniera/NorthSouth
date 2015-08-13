using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ChildrenList))]

//Chooses who is or isn't active at any given point in time
//Order is determined by order added, not this componenets problem
public class Chooser : MonoBehaviour {

	//
	//Private data
	//

	//ref to ChildrenList
	private ChildrenList _children;

	//chosen unit's index
	private int _index;

	// Move to next element in list in a circular way
	private void increment(){
		//Increment
		_index++;

		//Implement circular list
		if(_index >= _children.Count){
			_index = 0;
		}
	}

	//
	//Unity callbacks
	//

	void Start(){
		_children = GetComponent<ChildrenList>();
		_index = 0;
	}

	// If user presses B, changes control to next unit in list.
	public UnitController GetNextUnit(){
		if(_children.Count == 0)
			return null;

		increment();

		UnitController toReturn = _children.At(_index).GetComponent<UnitController>();

		return toReturn;
	}

	// Reset our list
	public void Reset(){
		_index = 0;
	}

	// Used by a UnitController to remove itself from our
	// children list.
	public void RemoveFromList(Component comp){
		// Increment because we lost our current unit.
		_children.RemoveFromList(comp);
	}
}

