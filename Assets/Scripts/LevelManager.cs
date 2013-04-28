using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	static public void GotoRoom(Room room)
	{
		Globals.RoomManager.currentRoom = room;
		Application.LoadLevel(1);
 	}

	static public void GotoWorld()
	{
		Application.LoadLevel(0);
 	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}