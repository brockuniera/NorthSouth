using UnityEngine;
using System.Collections;

//A single SubCanon
public class SubCanon : ControlledUnit {


	//
	//Movement
	//

	//Input speeds
	//
	//Normal movement speed
	public float MoveSpeed;
	//Speed while charging
	public float SlowSpeed;

	public bool isSlow { get; set; }

	//
	//Attacking
	//
	
	//Projectilve prefab
	public Bullet Canonball;

	//Where to spawn projectile, relative to transform.position
	public Vector2 MuzzleRelative;

	//
	//Unity Callbacks
	//

	void Start(){
	}

	void Update(){
	}

	//
	//Attacking methods
	//

	public void StartCharging(){
	}

	//Spawn projectile
	public override void Attack(){
		Instantiate(Canonball, transform.position + (Vector3)MuzzleRelative, Quaternion.identity);
	}

	public void Cooldown(){
		//Nothing
	}

	//
	//Movement Helpers
	//

	private void SetVelocityFromInput(){
		float speed = isSlow ? SlowSpeed : MoveSpeed;
		rb2d.velocity = new Vector2(0, input.y * speed);
	}

	//
	//Movement
	//

	public override void Act(){

		//Movement
		//

		SetVelocityFromInput();
	}

}

