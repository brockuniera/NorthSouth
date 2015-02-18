using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private int damage = 1;
	public void SetDamage(int d){ damage = d; }

	//speed of bullet
	public float Speed = 50;


	//Life time in FixedUpdates
	public int LifeTime = 50;

	//setup this bullet
	public void Initialize(int playerNum){
		//player 1 shoots right; p2 shoots left
		float vel = playerNum == 1 ? Speed : -Speed;
		rigidbody2D.velocity = new Vector2(vel, 0);

		//the hitbox layer is the layer after the player layer
		gameObject.layer = LayerMask.NameToLayer(playerNum == 1 ?  "P1 Hitbox" : "P2 Hitbox");
	}

	//frame counter, for life time
	private int _frame = 0;
	void FixedUpdate(){
		if(++_frame >= LifeTime){
			Die();
		}
	}

	//TODO
	//we need to abstract the idea of subunit, or create a shootable interface
	//TODO
	void OnTriggerEnter2D(Collider2D col){
		SubSoldier ss;
		if(ss = col.transform.GetComponent<SubSoldier>()){
			ss.InflictDamage(damage);
		}
		Die();
	}

	void OnColliderEnter2D(){
		Die();
	}

	void Die(){
		Destroy(gameObject);
	}
}
