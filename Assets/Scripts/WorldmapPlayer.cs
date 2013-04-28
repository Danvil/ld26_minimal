using UnityEngine;
using System.Collections;

public class WorldmapPlayer : MonoBehaviour {

	const float MOVE_TIMEOUT = 0.1f;

	public Worldmap Worldmap;

	float moveTimeout = 0.0f;

	void Awake()
	{
	}

	void Start()
	{
		Reposition();
	}

	public void Reposition()
	{
	}
	
	void Update()
	{
		moveTimeout += MyTime.deltaTime;
		if(moveTimeout >= MOVE_TIMEOUT) {
			moveTimeout = 0.0f;
			int x = Globals.PlayerPosX;
			int y = Globals.PlayerPosY;
			if(Input.GetKey(KeyCode.A)) {
				x--;
			}
			if(Input.GetKey(KeyCode.D)) {
				x++;
			}
			if(Input.GetKey(KeyCode.W)) {
				y++;
			}
			if(Input.GetKey(KeyCode.S)) {
				y--;
			}
			if(Globals.RoomManager.IsValid(x,y)) {
				Globals.PlayerPosX = x;
				Globals.PlayerPosY = y;
			}
			this.transform.position = new Vector3(Globals.PlayerPosX, Globals.PlayerPosY,-1);
			Room newRoom = Globals.RoomManager.GetRoom(Globals.PlayerPosX, Globals.PlayerPosY);
			if(newRoom != null && newRoom != Globals.RoomManager.currentRoom) {
				// ENTER ROOM
				Globals.SceneTransition.GotoRoom(newRoom);
			}
		}
//		if(Input.GetMouseButtonDown(1)) {
//			Globals.RoomManager.CreateNew();
//			Worldmap.UpdateMondrian();
//		}
	}
}
