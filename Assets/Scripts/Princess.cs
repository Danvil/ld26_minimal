using UnityEngine;
using System.Collections;

public class Princess : MonoBehaviour {

	public AudioClip audioPrincessIn;
	
	public float PrincessHeal = 2.5f;
	
	public GameObject particlesPrincessIn;
	public GameObject particlesPrincessOut;

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
		
		// player health up
		Globals.Player.living.Health += PrincessHeal;
		
		// particle effect
		GameObject go = (GameObject)Instantiate(particlesPrincessOut);
		go.transform.parent = this.transform;
		go.transform.localPosition = Vector3.zero;
		
		this.GetComponent<MeshRenderer>().enabled = false;
		this.GetComponent<SphereCollider>().enabled = false;
		
		Globals.SceneTransition.GotoWorld();
	}

	public void ShowPrincess()
	{
		if(!this.gameObject.activeSelf) {
			this.gameObject.SetActive(true);
			Globals.Player.audio.PlayOneShot(audioPrincessIn);
			GameObject go = (GameObject)Instantiate(particlesPrincessIn);
			go.transform.parent = this.transform;
			go.transform.localPosition = Vector3.zero;
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
