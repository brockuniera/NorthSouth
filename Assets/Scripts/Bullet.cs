using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private int damage = 1;
	public void SetDamage(int d){ damage = d; }

	private int _frame = 0;

	//Life time in FixedUpdates
	public int LifeTime = 50;

	void FixedUpdate(){
		if(++_frame >= LifeTime){
			Die();
		}
	}

	void OnCollision2D(){
	}

	private void Die(){
		Object.Destroy(gameObject);
	}
}
