using UnityEngine;
using System.Collections;

//Abstract base class for all units
public abstract class AbstractUnit : MonoBehaviour{
	//Relevant state
	public int Damage = 1;
	public int Health = 1;

	//input
	protected InputStruct input;
	public void SetInput(InputStruct i){ input = i; }

	//Update() should be defined, so do that in derived classes idiot

}

