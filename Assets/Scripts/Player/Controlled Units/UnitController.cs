using UnityEngine;
using System.Collections;

//Abstract base class for all UnitControllers, like soldier, horses, etc
public abstract class UnitController : MonoBehaviour{

	//Inputs
	//Make this a component? //XXX
	//that would literally get rid of this class :)
	protected InputStruct input;
	public void InputMessage(InputStruct i){ input = i; }

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
	//Unity Callbacks
	//

	void Awake(){
		controlledSubUnits = GetComponent<ChildrenList>();
	}

}

