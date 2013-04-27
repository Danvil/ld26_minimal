using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BlobManager : MonoBehaviour {

	List<Living> blobsLife = new List<Living>();
	List<BlobMove> blobsMove = new List<BlobMove>();

	public void AddBlob(GameObject x)
	{
		Living blobLife = x.GetComponent<Living>();
		if(blobLife) {
			blobsLife.Add(blobLife);
		}
		BlobMove blobMove = x.GetComponent<BlobMove>();
		if(blobMove) {
			blobsMove.Add(blobMove);
		}
	}
	
	public Living[] GetLifeBehaviours()
	{
		return blobsLife.ToArray();
	}
	
	public BlobMove[] GetMoveBehaviours()
	{
		return blobsMove.ToArray();
	}
	
	void Awake()
	{
		Globals.BlobManager = this;
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// remove dead
		foreach(Living x in blobsLife.Where(x => x.IsDead && x.gameObject != Globals.Player.gameObject)) {
			Object.Destroy(x.gameObject);
		}
		blobsLife.RemoveAll(x => x.IsDead);
	}

}
