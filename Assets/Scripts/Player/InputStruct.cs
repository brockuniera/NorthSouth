public struct InputStruct{
	public sbyte x; //-1,0,1
	public sbyte y; //-1,0,1
	public bool a;  //t,f

	public bool isEmpty{
		get{ return x == 0 && y == 0 && a == false; }
	}

	public InputStruct(sbyte x, sbyte y, bool a){
		this.x = x;
		this.y = y;
		this.a = a;
	}

	public static InputStruct Empty {
		get { return new InputStruct(0, 0, false); }
	}
}

