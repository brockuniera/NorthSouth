using UnityEngine;
using System.Collections;

//Abstract base class for all units
//
public abstract class AbstractUnit : MonoBehaviour{
	//Relevant state
	public int Damage = 1; //?
	public int Health = 1; //?
	//returns true if this died

	//player number
	protected int playerNumber;
	public void SetPlayerNumber(int pn){
		playerNumber = pn;
		print("soldier pnum: " + playerNumber);
	}


	//inputks
	protected InputStruct input;
	public void InputMessage(InputStruct i){ input = i; }


	//Update() should be defined, so do that in derived classes idiot

}

