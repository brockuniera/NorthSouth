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


	public void Reset(){
		_index = 0;
	}

	//
	//Unity callbacks
	//

	void Start(){
		_children = GetComponent<ChildrenList>();
		_index = 0;
	}

	//If user presses B, changes
	public UnitController GetNextUnit(){
		if(_children.Count == 0)
			return null;

		UnitController toReturn = _children.At(_index).GetComponent<UnitController>();

		//Increment
		_index++;

		//Implement circular list
		if(_index == _children.Count){
			_index = 0;
		}

		return toReturn;
	}

}

