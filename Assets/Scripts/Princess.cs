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
			
			// place near player
			float R1 = 1.5f;
			float R2 = 2.0f;
			while(true) {
				float phi = Random.Range(0.0f, 360.0f);
				float r = Random.Range(R1, R2);
				Vector3 pos = Globals.Player.transform.position + new Vector3(r*Mathf.Cos(phi), r*Mathf.Sin(phi), 0.0f);
				pos = pos.WithChangedZ(0.0f);
				if(!Globals.Level.IsBlocking(pos)) {
					this.transform.position = pos;
					break;
				}
			}
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
