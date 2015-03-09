using UnityEngine;
using System.Collections;

//Handles the logic involved in giving players units
//and other initializations
public class PlayerSpawner : MonoBehaviour {

	//
	//Prefabs to use
	//
	
	public GameObject []Units;

	//
	//Private data
	//

	//References to Players
	private GameObject []_players;


	//
	//Unity callbacks
	//

	void Awake(){
		//Get references to Players 1 and 2
		_players = GameObject.FindGameObjectsWithTag("Player");
		if(_players.Length < 2){
			Debug.LogError("Couldn't find at least 2 Players in Hierchay");
			Application.Quit();
		}

		//setup array
		//Units = new GameObject[3]; //Not need because of Unity?
	}

	//DEBUG
	void Update(){
		if(Input.GetKeyDown("0"))
			SpawnPlayerUnits();
	}


	//Adds Units to players
	public void SpawnPlayerUnits(){
		foreach(GameObject player in _players){
			ChildrenList list = player.GetComponent<ChildrenList>();
			if(list == null){
				Debug.LogError("Player " + player + " has no ChildrenList!");
				return;
			}
			foreach(GameObject unit in Units){
				list.AddPrefabAsChild(unit);
			}
		}
	}


}

