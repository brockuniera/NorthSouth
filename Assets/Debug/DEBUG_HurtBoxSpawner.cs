using UnityEngine;
using System.Collections;

public class DEBUG_HurtBoxSpawner : ExtraBehaviour {

	// Use this for initialization
	void Start () {
		print("Spawned hurtbox lmao");
		SpawnHurtbox(Vector2.zero, 1.0f);
	}
	
}
