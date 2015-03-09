using UnityEngine;
using System.Collections;

//for List<T>
using System.Collections.Generic;

//List of children of this component
//Can add children here to do cool things
public class ChildrenList : MonoBehaviour {

	//
	//Data
	//

	//GOs I'm the parent of/control
	private List<GameObject> _children;

	//
	//Unity callbacks
	//

	void Awake(){
		_children = new List<GameObject>();
		_children.Capacity = 6;

	}

	//
	//Public methods
	//

	public GameObject At(int index){
		return _children[index];
	}

	public int GetCount(){
		return _children.Count;
	}

	//Removes all children
	// This probably needs to be recursive
	public void ClearChildren(){
		foreach(GameObject go in _children){
			Destroy(go);
		}
		_children.Clear();
	}

	//Remove someone from the list
	public void RemoveFromList(Object obj){
		GameObject go = obj as GameObject;
		if(go != null)
			_children.Remove(go);
		else
			Debug.LogError("Could not remove object from list.");
	}

	//TODO Layers?
	//Uses a prefab and Instantiates it itself
	public void AddPrefabAsChild(GameObject go, Vector3 position, Quaternion rotation){
		GameObject child = Instantiate(go, position, rotation) as GameObject;
		_children.Add(child);
		child.transform.parent = transform;
	}

	public void AddPrefabAsChild(GameObject go){
		AddPrefabAsChild(go, Vector3.zero, Quaternion.identity);
	}

	//Add a child yourself!
	public void AddChild(GameObject go){
		_children.Add(go);
		go.transform.parent = transform;
	}


	//Creates children with positions specified in []position
	//and adds them to the list
	public void CreateChildren(GameObject prefab, Vector3 []positions){
		var rot = transform.rotation;
		Transform par = transform;

		//Create children in position, add to list
		foreach(Vector3 p in positions){
			GameObject go = Instantiate(prefab, p, rot) as GameObject;
			go.transform.parent = par;
			_children.Add(go);
		}
	}
	
}
