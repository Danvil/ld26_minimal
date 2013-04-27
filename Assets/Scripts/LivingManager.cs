using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LivingManager : MonoBehaviour {

	List<Living> livings = new List<Living>();

	public void Add(Living x)
	{
		livings.Add(x);
	}
	
	public Living[] GetLivings()
	{
		return livings.ToArray();
	}
	
	void Awake()
	{
		Globals.LivingManager = this;
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// remove dead
		foreach(Living x in livings.Where(x => x.IsDead && x.gameObject != Globals.Player.gameObject)) {
			Object.Destroy(x.gameObject);
		}
		livings.RemoveAll(x => x.IsDead);
	}

}
