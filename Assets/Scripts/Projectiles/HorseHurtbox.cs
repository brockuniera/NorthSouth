using UnityEngine;
using System.Collections;

//Hurtbox for horses
public class HorseHurtbox : MonoBehaviour  {

	//Damage Hurtbox inflicts
	public int Damage = 1;

	void OnTriggerEnter2D(Collider2D col){
		ControlledUnit cu;
		if(cu = col.transform.GetComponent<ControlledUnit>()){
			cu.InflictDamage(Damage);
		}
	}

	public void Die(){
		Destroy(gameObject);
	}
}

