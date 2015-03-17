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
	public UnitController []Player1Units;
	public UnitController []Player2Units;
	private UnitController [][]Units;

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
		//setup list
		Units = new UnitController[2][];
		Units[0] = Player1Units;
		Units[1] = Player2Units;
	}

	//DEBUG
	void Update(){
		if(Input.GetKeyDown("0")){
			SpawnPlayerUnits();
		}
	}


	//Adds Units to players
	public void SpawnPlayerUnits(){
		for(int i = 0; i < NumberOfPlayers; i++){
			//get list
			ChildrenList list = _players[i].GetComponent<ChildrenList>();

			//add units
			foreach(UnitController unit in Units[i]){
				list.CreatePrefabAsChild(unit);
			}

			PlayerController pc = _players[i].GetComponent<PlayerController>();	
			pc.InitializeUnitController();
		}
	}


}

