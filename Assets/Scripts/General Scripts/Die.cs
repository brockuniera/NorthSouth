using UnityEngine;
using System.Collections;

public class Die : MonoBehaviour{

	// Call this to kill the attached GO after
	// TIME time
	public void DieAfter(float time){
		StartCoroutine(coro_DieAfterTime(time));
	}

	private IEnumerator coro_DieAfterTime(float time){
		yield return new WaitForSeconds(time);
		Destroy(gameObject);
	}
}
