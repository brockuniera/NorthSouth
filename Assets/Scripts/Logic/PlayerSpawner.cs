using UnityEngine;
using System.Collections;

//Handles the logic involved in giving players units
//and other initializations
public class PlayerSpawner : MonoBehaviour {


	//
	//Private data
	//

	//References to Players 1 and 2
	private GameObject _Player1, _Player2;


	//
	//Unity callbacks
	//

	void Start(){
		//Get references to Players 1 and 2
		if(!(_Player1 = GameObject.Find("Player 1")) || !(_Player2 = GameObject.Find("Player 2"))){
			Debug.LogError("Couldn't find Players in Hierchay");
			Application.Quit();
		}
	}
	

	//Adds Units to players
	public void SpawnPlayerUnits(){
		//this method does nothing right now...
		var chooser = _Player1.GetComponent<ActiveChooser>();
	}


}

