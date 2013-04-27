using UnityEngine;
using System.Collections;

public class Living : MonoBehaviour {
	
	public AudioClip audioHurt;
	
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
			}
		}
	}
	
	public float HealthMax;
	
	public bool IsDead { get; private set; }
	
	// Use this for initialization
	void Start () {
		IsDead = false;
		Health = HealthMax;
		Globals.LivingManager.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
