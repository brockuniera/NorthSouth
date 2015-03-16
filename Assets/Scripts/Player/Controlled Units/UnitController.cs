using UnityEngine;
using System.Collections;

//Abstract base class for all UnitControllers, like soldier, horses, etc
public abstract class UnitController : MonoBehaviour{

	//Inputs
	protected InputStruct lastinput;
	protected InputStruct input;
	public void InputMessage(InputStruct i){ lastinput = input; input = i; }

	//
	//Prefabs
	//

	//List of units
	protected ChildrenList controlledSubUnits;

	//
	//Prefabs
	//
	
	public ControlledUnit ChildUnit;
	

	//
	//Public methods
	//

	public void Die(){
		//Remove self from list
		Transform dad = transform.parent;
		ChildrenList cl = dad.GetComponent<ChildrenList>();
		cl.RemoveFromList(this);

		//destroy instance
		Destroy(gameObject);
	}

	//
	//Unity Callbacks
	//

	void Awake(){
		controlledSubUnits = GetComponent<ChildrenList>();
	}

}

