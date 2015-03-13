using UnityEngine;
using System.Collections;

using System; //for Type
using System.Collections.Generic;//for List<T>

//List of children of this component
//Can add children here to do cool things
public class ChildrenList : MonoBehaviour, IEnumerable<Component> {

	//
	//Data
	//

	//Components I control
	private List<Component> _children;

	//
	//Unity callbacks
	//

	void Awake(){
		_children = new List<Component>();
		_children.Capacity = 6;

	}

	//
	//Public methods
	//

	public Component At(int index){
		return _children[index];
	}

	public int GetCount(){
		return _children.Count;
	}

	//Removes all children
	// This probably needs to be recursive
	public void ClearChildren(){
		foreach(Component comp in _children){
			Destroy(comp.gameObject);
		}
		_children.Clear();
	}

	//Remove someone from the list without destroying them
	public void RemoveFromList(Component comp){
		_children.Remove(comp);
	}

	//TODO Layers?
	//Uses a prefab and Instantiates it itself
	public void CreatePrefabAsChild(Component prefab, Vector3 position, Quaternion rotation){
		Component child = (Component)Instantiate(prefab, position, rotation);
		AddChild(child);
	}

	public void CreatePrefabAsChild(Component  prefab){
		CreatePrefabAsChild(prefab, Vector3.zero, Quaternion.identity);
	}

	//Add a child yourself!
	public void AddChild(Component comp){
		_children.Add(comp);
		comp.transform.parent = transform;
	}


	//Creates children with positions specified in []position
	//and adds them to the list
	public void CreateChildren(Component prefab, Vector2 []positions){
		var rot = transform.rotation;

		//Create children in position, add to list
		foreach(Vector3 p in positions){ //Implicit conversion from Vector2 to Vector3
			CreatePrefabAsChild(prefab, p, rot);
		}
	}

	//
	//Enumerator Things
	//
	
	IEnumerator<Component> IEnumerable<Component>.GetEnumerator(){
		return this._children.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator(){
		return this._children.GetEnumerator();
	}
}

