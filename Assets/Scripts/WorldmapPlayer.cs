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
			int x = LevelManager.PlayerPosX;
			int y = LevelManager.PlayerPosY;
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
				LevelManager.PlayerPosX = x;
				LevelManager.PlayerPosY = y;
			}
			this.transform.position = new Vector3(LevelManager.PlayerPosX, LevelManager.PlayerPosY,-1);
			Room newRoom = Globals.RoomManager.GetRoom(LevelManager.PlayerPosX, LevelManager.PlayerPosY);
			if(newRoom != null && newRoom != Globals.RoomManager.currentRoom) {
				// ENTER ROOM
				LevelManager.GotoRoom(newRoom);
			}
		}
		if(Input.GetMouseButtonDown(1)) {
			Globals.RoomManager.CreateNew();
			Worldmap.UpdateMondrian();
		}
	}
}
