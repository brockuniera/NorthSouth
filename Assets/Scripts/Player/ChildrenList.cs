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

	public Component this[int i]{
		get { return _children[i]; }
		set { _children[i] = value; }
	}

	public Component At(int index){
		return _children[index];
	}

	public int Count{
		get{ return _children.Count; }
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

	//Add a child yourself!
	public void AddChild(Component comp){
		_children.Add(comp);
		comp.transform.parent = transform;
	}

	//Uses a prefab and Instantiates it itself
	public void CreatePrefabAsChild(Component prefab, Vector3 position, Quaternion rotation){
		Component child = (Component)Instantiate(prefab, position, rotation);
		AddChild(child);
	}

	//Spawn unit with default position as this object's location
	public void CreatePrefabAsChild(Component prefab){
		CreatePrefabAsChild(prefab, transform.position, Quaternion.identity);
	}



	//Creates children with positions specified in []positions
	//and adds them to the list
	public void CreateChildren(Component prefab, Vector2 []positions, bool absolute=false){
		var rot = transform.rotation;
		Vector3 relpos = absolute ? Vector3.zero : transform.position;

		//Create children in position, add to list
		foreach(Vector3 p in positions){
			CreatePrefabAsChild(prefab, p + relpos, rot);
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

