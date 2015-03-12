using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	//Damage bullet inflicts
	public int Damage = 1;

	//Velocity of bullet
	public float Velocity = 50;

	//Life time in FixedUpdates
	public int LifeTime = 50;


	//
	//Unity Callbacks
	//

	void Start(){
		rigidbody2D.velocity = new Vector2(Velocity, 0);
	}

	//frame counter, for life time
	private int _frame = 0;

	//Count frames until death
	void FixedUpdate(){
		if(++_frame >= LifeTime){
			Die();
		}
	}

	//TODO
	//we need to abstract the idea of subunit, or create a shootable interface
	//so we don't have to rewrite this a bazilly times
	void OnColliderEnter2D(Collider2D col){
		SubSoldier ss;
		if(ss = col.transform.GetComponent<SubSoldier>()){
			ss.InflictDamage(Damage);
		}
		Die();
	}

	void Die(){
		Destroy(gameObject);
	}
}
