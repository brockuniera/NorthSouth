using UnityEngine;
using System.Collections;

public class ExtraBehaviour : MonoBehaviour {
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

	public static GameObject GetPlayerOne(){
		return GameObject.Find("/Player 1");
	}

	public static GameObject GetPlayerTwo(){
		return GameObject.Find("/Player 2");
	}
}
