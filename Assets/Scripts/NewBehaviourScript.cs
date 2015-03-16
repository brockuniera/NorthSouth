using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	public Transform TargetPos;
	
	// Update is called once per frame
	void Update () {
		if(TargetPos != null)
			transform.position = TargetPos.position;
	}
}
