using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour {

	public AudioClip audioLanding;
	public AudioClip[] audioPrincessOut;
	public AudioClip audioWin;
	public AudioClip audioLoose;

	public void GotoRoom(Room room)
	{
		Globals.RoomManager.currentRoom = room;
		if(!room.isCleared) {
			room.isCleared = true; // TODO wrong order
			Globals.IsWorld = false;
			Application.LoadLevel(1);
		}
 	}
	
	IEnumerator GotoWorldDo()
	{
		yield return new WaitForSeconds(1.5F);
		Globals.IsWorld = true;
		Application.LoadLevel(0);
	}

	public void GotoWorld()
	{
		Globals.Player.audio.PlayOneShot(audioPrincessOut[Random.Range(0,audioPrincessOut.Length-1)]);
		Globals.PlayerPosX = (int)(Globals.Player.transform.position.x) + Globals.RoomManager.currentRoom.x1;
		Globals.PlayerPosY = (int)(Globals.Player.transform.position.y) + Globals.RoomManager.currentRoom.y1;
		Camera.main.GetComponent<Fade>().fadeDir = +1.0f;
		StartCoroutine(GotoWorldDo());
 	}
	
	bool isWin = false;
	bool isLoose = false;
	
	public void Win()
	{
		if(isWin || isLoose) return;
		MyTime.Pause = true;
		Globals.Player.audio.PlayOneShot(audioWin);
		Debug.Log("WIN");
		isWin = true;
	}
	
	public void Loose()
	{
		if(isLoose || isLoose) return;
		MyTime.Pause = true;
		Globals.Player.audio.PlayOneShot(audioLoose);
		Debug.Log("LOOSE");
		isLoose = true;
	}
	
	void Awake()
	{
		Globals.SceneTransition = this;
	}

	void OnGUI()
	{
		if(isLoose) {
			GUI.Label(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2), "Game Over!");
		}
		if(isWin) {
			GUI.Label(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2), "You have won the game!");
		}
	}
	
	void Start()
	{
		if(!Globals.IsWorld) {
			Globals.Player.audio.PlayOneShot(audioLanding);
		}
	}
	
	void Update()
	{
	}
}
