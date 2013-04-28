using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour {

	public AudioClip audioLanding;
	public AudioClip[] audioPrincessOut;

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
		yield return new WaitForSeconds(1.0F);
		Globals.IsWorld = true;
		Application.LoadLevel(0);
	}

	public void GotoWorld()
	{
		Globals.Player.audio.PlayOneShot(audioPrincessOut[Random.Range(0,audioPrincessOut.Length-1)]);
		Globals.PlayerPosX = (int)(Globals.Player.transform.position.x) + Globals.RoomManager.currentRoom.x1;
		Globals.PlayerPosY = (int)(Globals.Player.transform.position.y) + Globals.RoomManager.currentRoom.y1;
		StartCoroutine(GotoWorldDo());
 	}
	
	void Awake()
	{
		Globals.SceneTransition = this;
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
