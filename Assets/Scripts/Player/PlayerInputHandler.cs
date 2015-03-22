using UnityEngine;
using System.Collections;

//this abstracts unity's input
// TODO
// needs to work for 2 players
// include stick support, deadzone etc
// inlcude secondary, tertiary bindings, etc
// way for users to rebind keys
// THERE REALLYREALLY SHOULD BE A BETTER WAY TO DO THIS
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


	//
	//Methods
	//

	//Sets the instance's keybindings to the default for that player
	//Must be called before UpdateInput
	public void SetPlayerKeyBindings(int playernum){
		//really gross code :(
		KeyBindings kb = new KeyBindings();
		switch(playernum){
		case 1:
			//KeyBindings defaults to player one's binds
			break;

		case 2:
			kb.up    = KeyCode.UpArrow;
			kb.down  = KeyCode.DownArrow;
			kb.left  = KeyCode.LeftArrow;
			kb.right = KeyCode.RightArrow;
			kb.a = KeyCode.Z;
			kb.b = KeyCode.X;
			break;

		default:
			Debug.LogError("invalid playernum!");
			break;
		}

		binds = kb;
	}

	//updates input variables
	public void UpdateInput(){
		if(Input.GetKey(binds.right)){
			x = 1;
		}else if(Input.GetKey(binds.left)){
			x = -1;
		}else{
			x = 0;
		}

		if(Input.GetKey(binds.up)){
			y = 1;
		}else if(Input.GetKey(binds.down)){
			y = -1;
		}else{
			y = 0;
		}
		a = Input.GetKey(binds.a);
		b = Input.GetKeyDown(binds.b);
	}
}

