using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	static public int PlayerPosX, PlayerPosY;

	static public void GotoRoom(Room room)
	{
		Globals.RoomManager.currentRoom = room;
		if(!room.isCleared) {
			room.isCleared = true; // TODO wrong order
			Application.LoadLevel(1);
		}
 	}

	static public void GotoWorld()
	{
		PlayerPosX = (int)(Globals.Player.transform.position.x) + Globals.RoomManager.currentRoom.x1;
		PlayerPosY = (int)(Globals.Player.transform.position.y) + Globals.RoomManager.currentRoom.y1;
		Application.LoadLevel(0);
 	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
