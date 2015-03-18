using UnityEngine;

public class Timer{

	//The time to wait for
	private float _goalTime;

	//returns true if enough time passed
	public bool IsDone {
		get{ return Time.time >= _goalTime; }
	}

	//Sets how long the timer should time for
	public void SetTimer(float length){
		_goalTime = length + Time.time;
	}

}
