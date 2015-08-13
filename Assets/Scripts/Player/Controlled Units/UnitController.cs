using UnityEngine;
using System.Collections;

//Abstract base class for all UnitControllers, like soldier, horses, etc
public abstract class UnitController : ExtraBehaviour{

	//Inputs
	//
	protected InputStruct lastinput;
	protected InputStruct input;
	public void InputMessage(InputStruct i){ lastinput = input; input = i; }

	//The direction the player presses to input 'back'
	// P1: -1
	// P2:  1
	protected sbyte backdir{ get; private set; }

	//
	//Selectability
	//
	
	private bool selectable = true;
	public bool isSelectable {
		get { return selectable; }
		set { selectable = value; }
	}

	//
	//Component caching
	//
	
	//List of units
	public ChildrenList controlledSubUnits;

	//
	//Prefab
	//

	//Child to spawn copies of
	public ControlledUnit ChildUnit;
	
	//
	//Unity Callbacks
	//

	void Awake(){
		controlledSubUnits = GetComponent<ChildrenList>();
		string layername = LayerMask.LayerToName(gameObject.layer);
		if(layername != "Player 1" && layername != "Player 2")
			Debug.LogError("UnitController layer not setup correctly!");

		backdir = playerNumber == 1 ? (sbyte)-1 : (sbyte)1;
	}

	//
	//Public methods
	//

	public void Die(){
		//Remove self from list
		GetComponentInParent<Chooser>().RemoveFromList(this);

		//destroy instance
		Destroy(gameObject);
	}

}

