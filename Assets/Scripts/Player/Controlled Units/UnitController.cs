using UnityEngine;
using System.Collections;

//Abstract base class for all UnitControllers, like soldier, horses, etc
public abstract class UnitController : MonoBehaviour{

	//Inputs
	//
	protected InputStruct lastinput;
	protected InputStruct input;
	public void InputMessage(InputStruct i){ lastinput = input; input = i; }

	//PlayerSpecific
	//
	//Player this belongs to
	protected int playerNumber;
	//The direction the player presses to input 'back'
	// P1: -1
	// P2:  1
	protected sbyte backdir{ get; private set; }

	//
	//Component caching
	//
	
	//List of units
	protected ChildrenList controlledSubUnits;

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
		if(layername.Contains("Default"))
			Debug.LogError("UnitController layer not setup correctly!");
		playerNumber = layername.Contains("1") ? 1 : 2;
		backdir = playerNumber == 1 ? (sbyte)-1 : (sbyte)1;
	}

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

}

