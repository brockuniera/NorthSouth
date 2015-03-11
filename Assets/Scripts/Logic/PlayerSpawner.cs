using UnityEngine;
using System.Collections;

//Handles the logic involved in giving players units
//and other initializations
public class PlayerSpawner : MonoBehaviour {

	public static readonly int NumberOfPlayers = 2;

	//
	//Prefabs to use
	//
	
	//Units given to each player
	public AbstractUnitController [][]Units;

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
		if(_players.Length != 2){
			Debug.LogError("Couldn't find two Players in Hierchay");
			Application.Quit();
		}
	}

	//DEBUG
	void Update(){
		if(Input.GetKeyDown("0")){
			Debug.Log("Spawning units for each Player");
			SpawnPlayerUnits();
		}
	}


	//Adds Units to players
	public void SpawnPlayerUnits(){
		for(int i = 0; i < NumberOfPlayers; i++){
			//get list
			ChildrenList list = _players[i].GetComponent<ChildrenList>();
			if(list == null){
				Debug.LogError("Player " + i + " has no ChildrenList!");
				return;
			}

			//add units
			foreach(AbstractUnitController unit in Units[i]){
				list.CreatePrefabAsChild(unit);
			}
		}
	}


}

