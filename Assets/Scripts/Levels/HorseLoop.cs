using UnityEngine;
using System.Collections;

public class HorseLoop : ExtraBehaviour {

	public float TimeToRespawn = 8.0f;

	private int horsesSaved;

	// Determined by first horse to die
	private Vector2 respawnPos;

	IEnumerator SpawnHorseAfter(){

		yield return new WaitForSeconds(TimeToRespawn);

		print("spawning horses! at " + respawnPos);

		GetOtherPlayer().BroadcastMessage("RespawnHorsesAt", respawnPos);
		GetOtherPlayer().BroadcastMessage("CycleUnits");
		GetOtherPlayer().GetComponentsInChildren<Horses>()[0].inLimbo = false;

		horsesSaved = 0;
	}

	void AddHorse(GameObject horse){

		Horses dad = horse.GetComponentsInParent<Horses>()[0];

		if(horsesSaved == 0){
			respawnPos = new Vector2(
					-transform.position.x,
					horse.transform.position.y
					);
		}

		// Kill horse that hit us
		dad.GetComponent<ChildrenList>().RemoveFromList(horse.GetComponent<SubHorse>());
		GameObject.Destroy(horse);

		// Spawn horses once we get enough horses
		if(dad.controlledSubUnits.Count == 0){
			dad.inLimbo = true;
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
