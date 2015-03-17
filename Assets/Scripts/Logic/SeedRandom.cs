using UnityEngine;
using System.Collections;

//Seeds Unity's Random, then destroys itself
public class SeedRandom : MonoBehaviour {

	//-1 means use default
	public int InitialSeed = -1;

	void Awake() {
		Random.seed = InitialSeed == -1 ? System.Environment.TickCount : InitialSeed;
		Destroy(this);
	}
	
}

