using UnityEngine;
using System.Collections;

//Hurtbox for horses
public class Hurtbox : MonoBehaviour  {

	//Damage Hurtbox inflicts
	public int Damage = 1;

	// We deal damage to whoever we hit
	void OnTriggerEnter2D(Collider2D col){
		ControlledUnit cu;
		if(cu = col.transform.GetComponent<ControlledUnit>()){
			cu.InflictDamage(Damage);
		}
	}

	private IEnumerator coro_delay(float time){
		yield return new WaitForSeconds(time);
		Die();
	}

	public void DieAfter(float time){
		StartCoroutine(coro_delay(time));
	}

	public void Die(){
		Destroy(gameObject);
	}
}

