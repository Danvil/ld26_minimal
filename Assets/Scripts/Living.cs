using UnityEngine;
using System.Collections;

public class Living : MonoBehaviour {
	
	public float FriendlyFireMult = 1.0f;

	public AudioClip audioHurt;
	
	public float HealthMax;
	
	private float health;
	
	public float Health
	{
		get { return health; }
		set {
			if(!IsDead) {
				if(value < health) {
					audio.PlayOneShot(audioHurt);
				}
				health = value;
				if(health <= 0.0f) {
					IsDead = true;
				}
				healthbar.Percentage = health / HealthMax;
			}
		}
	}

	public bool IsDead { get; private set; }
	
	private Healthbar healthbar;
	
	// Use this for initialization
	void Start () {
		healthbar = this.transform.FindChild("Healthbar").gameObject.GetComponent<Healthbar>();
		IsDead = false;
		Health = HealthMax;	
	}
	
	// Update is called once per frame
	void Update () {
		float p = Health / HealthMax;
		healthbar.Percentage = p;
	}
}
