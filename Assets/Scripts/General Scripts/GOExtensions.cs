namespace GameObjectExtensions {
	using UnityEngine;

	public static class GOExtensions {
		// Shorter syntax version of GetComponent<T>()
		public static T GC<T>(this GameObject go){
			return go.GetComponent<T>();
		}

		public static T[] GCInKids<T>(this GameObject go){
			return go.GetComponentsInChildren<T>();
		}
	}
}
