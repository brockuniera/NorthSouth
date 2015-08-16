using UnityEngine;
using System.Collections;

public class ExtraBehaviour : MonoBehaviour {

	//
	// Player numbers
	//

	public int playerNumber {
		get {
			return LayerMask.LayerToName(gameObject.layer).Contains("1") ? 1 : 2;
		}
	}

	public bool isPlayerOne {
		get { return playerNumber == 1; }
	}

	public bool isPlayerTwo {
		get { return playerNumber == 2; }
	}

	public GameObject GetMyPlayer(){
		return isPlayerOne ? GetPlayerOne() : GetPlayerTwo();
	}

	public GameObject GetOtherPlayer(){
		return isPlayerTwo ? GetPlayerOne() : GetPlayerTwo();
	}

	// Player finding
	//

	public static GameObject GetPlayerOne(){
		return GameObject.Find("/Player 1");
	}

	public static GameObject GetPlayerTwo(){
		return GameObject.Find("/Player 2");
	}

	//
	// Hurtbox spawning
	//

	// Spawns a hurtbox at POSITION, lasting TIME seconds, dealing (by default)
	// 1 DAMAGE, and RADIUS Unity units.
	//
	// The hurtbox is spawned for whichever player this MonoBehaviour is.
	protected void SpawnHurtbox(
			Vector2 position,
			float time,
			float radius=10.0f,
			int damage=1,
			Transform followthis=null
		){
		Hurtbox hbox = ((GameObject)Instantiate(
				Resources.Load("Hurtbox"),
				position,
				Quaternion.identity
		)).GetComponent<Hurtbox>();

		// Setup hurtbox with correct settings
		hbox.Damage = damage;
		hbox.DieAfter(time);
		hbox.transform.parent = followthis;
		hbox.gameObject.layer = LayerMask.NameToLayer("P" + playerNumber + " Hitbox");
	}
}
