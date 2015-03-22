using UnityEngine;
using System.Collections;

public class DeathPit : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col){
		ControlledUnit cu;
		if(cu = col.transform.GetComponent<ControlledUnit>()){
			cu.InflictDamage(1000);
		}
	}

}

