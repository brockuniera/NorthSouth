using UnityEngine;
using System.Collections;
using GameObjectExtensions;

public class HorseLoop : ExtraBehaviour {

	public float TimeToRespawn = 5.0f;

	public Transform RespawnX;

	private int horsesSaved;

	// Determined by first horse to die
	private Vector2 respawnPos;

	private IEnumerator SpawnHorseAfter(){

		yield return new WaitForSeconds(TimeToRespawn);

		var otherplayer = GetOtherPlayer();
		var horses = otherplayer.GC<PlayerController>().horses;

		otherplayer.BroadcastMessage("RespawnHorsesAt", respawnPos);
		horses.inLimbo = false;
		horses.InputMessage(InputStruct.Empty);

		horsesSaved = 0;
	}

	void AddHorse(GameObject horse){

		Horses dad = horse.GetComponentsInParent<Horses>()[0];

		// Build the respawn position based off of the first
		// SubHorse to collide with us
		if(horsesSaved == 0){
			respawnPos = new Vector2(
					RespawnX.position.x,
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
