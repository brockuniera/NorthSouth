using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	//Damage bullet inflicts
	public int Damage = 1;

	//Velocity of bullet
	public float Velocity = 15;

	//Life time in FixedUpdates
	public int LifeTime = 20;


	//
	//Unity Callbacks
	//

	void Start(){
		GetComponent<Rigidbody2D>().velocity = new Vector2(Velocity, 0);
	}

	//frame counter, for life time
	private int _frame = 0;

	//Count frames until death
	void FixedUpdate(){
		if(++_frame >= LifeTime){
			Die();
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		ControlledUnit cu;
		if(cu = col.transform.GetComponent<ControlledUnit>()){
			cu.InflictDamage(Damage);
		}
		Die();
	}

	void Die(){
		Destroy(gameObject);
	}
}

