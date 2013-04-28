using UnityEngine;
using System.Collections;

public class Princess : MonoBehaviour {

	public AudioClip audioPrincessIn;

	bool pickedUp = false;

	void Awake()
	{
		Globals.Princess = this;
	}
	
	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Pickup()
	{
		pickedUp = true;
		Globals.SceneTransition.GotoWorld();
	}

	public void ShowPrincess()
	{
		if(!this.gameObject.activeSelf) {
			this.gameObject.SetActive(true);
			Globals.Player.audio.PlayOneShot(audioPrincessIn);
		}
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
