using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {
	
	public Texture2D fadeTexture;
	public float fadeDelay = 0.0f;
	public float fadeSpeed = 1.0f / 1.5f;
	public float fadeDir = -1.0f;
	public float alpha = 1.0f;

	float delay = 0.0f;
	bool doFade = true;	
	int drawDepth = -1000;
	
	// Use this for initialization
	void Start()
	{
		if(Globals.ShowTutorial) {
			delay = fadeDelay;
			Globals.ShowTutorial = false;
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		delay -= MyTime.deltaTime;
		if(delay <= 0.0f) {
			alpha += fadeDir * fadeSpeed * MyTime.deltaTime;
		}
		alpha = Mathf.Clamp01(alpha);
		doFade = (alpha > 0.01f);
		
		if(Input.GetMouseButton(0)) {
			delay = 0.0f;
		}
	}
	
	void OnGUI()
	{
		if(doFade) {
			GUI.color = new Color(1.0f, 1.0f, 1.0f, alpha);
			GUI.depth = drawDepth;	
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
		}
	}
}
