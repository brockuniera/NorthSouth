using UnityEngine;
using System.Collections;

//this abstracts unity's input
// needs to work for 2 players
// include stick support
public class PlayerInputHandler : MonoBehaviour {

	//Properties
	public int xAxis{
		get{ return x; }
	}
	public int yAxis{
		get{ return y; }
	}
	public bool aButton{
		get{ return a; }
	}
	public bool bButton{
		get{ return b; }
	}

	//current binds
	private KeyBindings binds;

	//key abstraction with defaults
	private class KeyBindings{
		//movement
		public KeyCode up    = KeyCode.W;
		public KeyCode down  = KeyCode.S;
		public KeyCode left  = KeyCode.A;
		public KeyCode right = KeyCode.D;

		//actions
		public KeyCode a = KeyCode.J;
		public KeyCode b = KeyCode.K;
	}

	private int x, y;
	private bool a, b;


	//read input
	void Update(){
		if(Input.GetKey(binds.right))
			x = 1;
		else if(Input.GetKey(binds.left))
			x = -1;
		else
			x = 0;

		if(Input.GetKey(binds.up))
			y = 1;
		else if(Input.GetKey(binds.down))
			y = -1;
		else
			y = 0;

		a = Input.GetKey(binds.a);
		b = Input.GetKey(binds.b);
	}

	public void SetDefaultBinds(){
		//is this really how I should do this?
		binds = new KeyBindings();
	}

}
