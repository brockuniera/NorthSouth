using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private int damage = 1;
	public void SetDamage(int d){ damage = d; }

	//speed of bullet
	public float Speed = 50;


	//Life time in FixedUpdates
	public int LifeTime = 50;

	void Start(){
		//set velocity
		//should change based on player, ie going left or right <--TODO
		rigidbody2D.velocity = new Vector2(Speed, 0);
	}


	//frame counter, for life time
	private int _frame = 0;
	void FixedUpdate(){
		if(++_frame >= LifeTime){
			Die();
		}
	}

	void OnCollision2D(){
		//TODO
	}

	private void Die(){
		Destroy(gameObject);
	}
}
