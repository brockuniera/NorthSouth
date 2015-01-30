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
	private List<Transform> units;
	private Transform currentUnit;

	//input handler
	private PlayerInputHandler input;

	void Start(){
		units = new List<Transform>();
		units.Capacity = 3; //TODO
		foreach(Transform child in transform){
			units.Add(child);
		}

		input = GetComponent<PlayerInputHandler>();
	}
	
	void FixedUpdate(){
		//
		//Process and distribute input
		//

		//change unit then ignore other input
		if(input.bButton){
			ChangeControlledUnit();
			return;
		}

		//tell current thing to attack
		if(input.aButton){
			//units.
		}

		//tell current thing to move


	}

	private void ChangeControlledUnit(){

	}
}
