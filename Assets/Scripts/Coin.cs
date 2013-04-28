using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	public AudioClip[] audioCoin;

	bool pickedUp = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PickupCoin()
	{
		pickedUp = true;
		Globals.Player.audio.PlayOneShot(audioCoin[Random.Range(0,audioCoin.Length-1)]);
		Object.Destroy(this.gameObject);
		Globals.Player.NumCoinsCollected ++;
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if(pickedUp) return;
		Player player = collision.gameObject.GetComponent<Player>();
		if(player != null) {
			PickupCoin();
		}
	}

}
