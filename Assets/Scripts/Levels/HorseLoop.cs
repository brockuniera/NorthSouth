using UnityEngine;
using System.Collections;
using GameObjectExtensions;

public class HorseLoop : ExtraBehaviour {

	public float TimeToRespawn = 8.0f;

	private int horsesSaved;

	// Determined by first horse to die
	private Vector2 respawnPos;

	IEnumerator SpawnHorseAfter(){

		yield return new WaitForSeconds(TimeToRespawn);

		print("spawning horses! at " + respawnPos);

		GetOtherPlayer().BroadcastMessage("RespawnHorsesAt", respawnPos);
		GetOtherPlayer().GetComponentsInChildren<Horses>()[0].inLimbo = false;
		GetOtherPlayer().GCInKids<Horses>()[0].InputMessage(InputStruct.Empty);

		horsesSaved = 0;
	}

	void AddHorse(GameObject horse){

		Horses dad = horse.GetComponentsInParent<Horses>()[0];

		// Build the respawn position based off of the first
		// SubHorse to collide with us
		if(horsesSaved == 0){
			respawnPos = new Vector2(
					-transform.position.x,
					horse.transform.position.y
					);
		}

		// Remove horse that hit us
		dad.GetComponent<ChildrenList>().RemoveFromList(horse.GetComponent<SubHorse>());
		GameObject.Destroy(horse);

		// Spawn new horses once we get enough horses
		if(dad.controlledSubUnits.Count == 0){
			// Horse controller should be in limbo
			dad.inLimbo = true;

			// Automatically select new unit if Horses were active
			if(GetOtherPlayer().GetComponent<PlayerController>().activeController is Horses)
				GetOtherPlayer().SendMessage("CycleUnits");

			//GetOtherPlayer().GetComponentsInChildren<Horses>()[0].Active = false;
			StartCoroutine(SpawnHorseAfter());
		}

		horsesSaved++;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.GetComponent<SubHorse>()){
			AddHorse(other.gameObject);
		}
	}
}
