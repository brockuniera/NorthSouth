using UnityEngine;
using System.Collections;

//this abstracts unity's input
// TODO
// needs to work for 2 players
// include stick support, deadzone etc
// inlcude secondary, tertiary bindings, etc
// way for users to rebind keys
public class PlayerInputHandler : MonoBehaviour {

	//Properties
	public sbyte xAxis{
		get{ return x; }
	}
	public sbyte yAxis{
		get{ return y; }
	}
	public bool aButton{
		get{ return a; }
	}
	public bool bButton{
		get{ return b; }
	}

	//default values for buttons and dirs
	static readonly public sbyte XDefault = 0;
	static readonly public sbyte YDefault = 0;
	static readonly public bool ADefault = false;

	//key abstraction with defaults
	private class KeyBindings{
		//movement
		public KeyCode up    = KeyCode.W;
		public KeyCode down  = KeyCode.S;
		public KeyCode left  = KeyCode.A;
		public KeyCode right = KeyCode.D;

		//actions
		public KeyCode a = KeyCode.K;
		public KeyCode b = KeyCode.J;
	}

	//current binds
	private KeyBindings binds;

	private sbyte x, y;
	private bool a, b;

	//initialize
	void Start(){
		SetDefaultBinds();
	}

	//Defaults the binds class
	public void SetDefaultBinds(){
		//is this really how I should do this?
		binds = new KeyBindings();
	}

	//updates input variables
	public void UpdateInput(){
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


}
