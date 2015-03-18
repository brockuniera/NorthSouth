public struct InputStruct{
	public sbyte x; //-1,0,1
	public sbyte y; //-1,0,1
	public bool a;  //t,f

	public bool IsEmpty{
		get{ return x == 0 && y == 0 && a == false; }
	}
}

