using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {
	
	static public Globals GLOBALS;
	
	public Camera MainCamera;
	
	void Awake() {
		GLOBALS = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
