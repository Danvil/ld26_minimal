using UnityEngine;
using System.Collections;

public class BombManager : MonoBehaviour {
	
	public GameObject pfBomb;
	
	void Awake()
	{
		Globals.BombManager = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ThrowBomb(Vector3 start, Vector3 vel)
	{
		GameObject go = (GameObject)Instantiate(pfBomb);
		go.transform.parent = this.transform;
		go.transform.position = start;
		go.rigidbody.velocity = vel;
	}
}
