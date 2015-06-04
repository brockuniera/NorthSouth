using UnityEngine;
using System.Collections;

//Handles the logic involved in giving players units
//and other initializations
public class PlayerSpawner : MonoBehaviour {

	public static readonly int NumberOfPlayers = 2;

	//
	//Levels
	//

	private LevelSelector levelSel;

	//
	//Prefabs to use
	//
	
	//Units given to each player
	public UnitController[] Player1Units;
	public UnitController[] Player2Units;
	private UnitController[][] Units;

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
		//setup list of units
		Units = new UnitController[2][];
		Units[0] = Player1Units;
		Units[1] = Player2Units;

		//Setup reference to level selector
		levelSel = GetComponent<LevelSelector>();
	}

	//DEBUG
	void Update(){
		if(Input.GetKeyDown("0")){
			SpawnPlayerUnits();
		}
	}

	//
	//Useful methods
	//

	//Adds Units to players
	public void SpawnPlayerUnits(){
		for(int i = 0; i < NumberOfPlayers; i++){
			//get list
			ChildrenList list = _players[i].GetComponent<ChildrenList>();

			//add units at spawnpoints for respective players\
			//players are indexed like 1,2 by GetPlayerSpawns
			Vector3[] spawns = levelSel.GetPlayerSpawns(i + 1);
			int j = 0;
			foreach(UnitController unit in Units[i]){
				list.CreatePrefabAsChild(unit, spawns[j++], Quaternion.identity);
			}

			//Sets player's current unit to whoever was added first
			_players[i].GetComponent<PlayerController>().InitializeUnitController();
		}
	}

}

