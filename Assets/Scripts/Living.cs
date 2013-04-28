using UnityEngine;
using System.Collections;

public class Living : MonoBehaviour {
	
	const float HEART_ALPHA_DECAY = 0.3f;
	
	public float FriendlyFireMult = 1.0f;

	public AudioClip audioHurt;
	
	public Material matHealth;
	
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
				showHeartAlpha = 1.0f;
			}
		}
	}

	float showHeartAlpha = 0.0f;
	
	public float HealthMax;
	
	public bool IsDead { get; private set; }
	
	Material mat;
	
	// Use this for initialization
	void Start () {
		IsDead = false;
		Health = HealthMax;
		showHeartAlpha = 0.0f;
		
		mat = (Material)Instantiate(matHealth);
		GameObject go = this.transform.FindChild("obj/player/health").gameObject;
		go.renderer.material = mat;
	}
	
	// Update is called once per frame
	void Update () {
		showHeartAlpha -= MyTime.deltaTime * HEART_ALPHA_DECAY;
		float p = Health / HealthMax;
		mat.SetColor("_TintColor", new Color(p, 0.0f, 0.0f, Mathf.Clamp01(showHeartAlpha)));
	}
}
