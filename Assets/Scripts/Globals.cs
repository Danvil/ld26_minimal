using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {
	
	static public Camera MainCamera;

	static public BombManager BombManager;

	void Awake() {
		MainCamera = GetComponentInChildren<Camera>();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
