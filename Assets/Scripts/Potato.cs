using UnityEngine;
using System.Collections;

public class Potato : MonoBehaviour {

	public AudioClip[] audioPickup;

	bool pickedUp = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Pickup()
	{
		pickedUp = true;
		Globals.Player.audio.PlayOneShot(audioPickup[Random.Range(0,audioPickup.Length-1)]);
		Object.Destroy(this.gameObject);
		Globals.Player.EatPotatoe();
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if(pickedUp) return;
		Player player = collision.gameObject.GetComponent<Player>();
		if(player != null) {
			Pickup();
		}
	}

}
