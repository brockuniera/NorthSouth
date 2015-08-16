using UnityEngine;
using System.Collections;

//Sets the current level and has information about it
public class LevelSelector : MonoBehaviour {

	//
	//Levels in the scene
	//
	
	//References to levels
	public GameObject[] Levels;
	private GameObject currentLevel;


	//
	//Public methods
	//

	//Get the spawn points for the current level
	public Vector3[] GetPlayerSpawns(int playernum){
		if(playernum > 2 || playernum < 1){
			Debug.LogError("Player number " + playernum + " is out of range!");
			return null;
		}
			
		Vector3[] spawnPoints;
		Transform spawnsFolder = null;
		//Find spawns GO
		foreach(Transform child in currentLevel.transform){
			if(child.name == "Spawns")
				spawnsFolder = child;
		}

		if(spawnsFolder == null){
			Debug.LogError("No GO named Spawns in this level!");
			return null;
		}

		//Get the player1 folder
		Transform player = spawnsFolder.GetChild(playernum - 1);

		//Initialize list
		spawnPoints = new Vector3[player.childCount];
		//Get player's children's positions
		int i = 0;
		foreach(Transform child in player){
			spawnPoints[i++] = child.position;
		}

		return spawnPoints;
	}

	//Get name of current level
	public string GetCurrentLevelName(){
		return currentLevel.name;
	}

	//
	//Unity callbacks
	//

	void Awake(){
		Levels = GameObject.FindGameObjectsWithTag("Level");
		if(Levels == null){
			Debug.LogError("Couldn't find any levels in Hierchay!");
		}
		currentLevel = Levels[0];
	}

}


