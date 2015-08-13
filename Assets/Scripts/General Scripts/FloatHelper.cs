namespace UnityEngine {
	public static class FloatHelper{
		public static bool IsApproxDelta(float a, float b, float delta){
			float dif = a - b;
			if(dif < 0)
				dif = -dif;
			return dif <= delta;
		}
	}
}
