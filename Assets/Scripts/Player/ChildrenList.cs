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
	private Type _typeToUse;

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

	//Type Mutators
	//

	public void SetType(Type t){
		_typeToUse = t;
	}

	//List Mutators
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
	public void CreatePrefabAsChild(GameObject prefab, Vector3 position, Quaternion rotation){
		GameObject child = Instantiate(prefab, position, rotation) as GameObject;
		_children.Add(child.GetComponent(_typeToUse.GetType()));
		//child.transform.parent = transform;
	}

	public void CreatePrefabAsChild(GameObject go){
		CreatePrefabAsChild(go, Vector3.zero, Quaternion.identity);
	}

	//Add a child yourself!
	public void AddChild(Component comp){
		_children.Add(comp);
		//go.transform.parent = transform;
	}


	//Creates children with positions specified in []position
	//and adds them to the list
	public void CreateChildren(GameObject prefab, Vector2 []positions){
		var rot = transform.rotation;
		//Transform par = transform;

		//Create children in position, add to list
		foreach(Vector3 p in positions){ //Implicit conversion from Vector2 to Vector3
			GameObject go = Instantiate(prefab, p, rot) as GameObject;
			//go.transform.parent = par;
			_children.Add(go.GetComponent(_typeToUse.GetType()));
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

