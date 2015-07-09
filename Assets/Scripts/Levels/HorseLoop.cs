using UnityEngine;
using System.Collections;

public class HorseLoop : MonoBehaviour {

	public int HorsesUntilActive = 3;
	private int horsesSaved = 0;
	public float TimeToRespawn = 5.0f;

	// Determined by first horse to die
	private Vector2 respawnPos;

	IEnumerator SpawnHorseAfter(){
		yield return new WaitForSeconds(TimeToRespawn);

		print("spawning horses!");
	}

	void AddHorse(GameObject horse){
		if(horsesSaved == 0){
			respawnPos = new Vector2(
					-transform.position.x,
					horse.transform.position.y
					);
		}

		// Kill horse that hit us
		horse.SendMessage("InflictDamage", 100);

		// Spawn horses once we get enough horses
		if(++horsesSaved >= HorsesUntilActive){
			StartCoroutine(SpawnHorseAfter());
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.GetComponent<SubHorse>()){
			AddHorse(other.gameObject);
		}
	}
}
