using UnityEngine;
using System.Collections;

public class WorldmapPlayer : MonoBehaviour {

	const float MOVE_TIMEOUT = 0.1f;

	public Worldmap Worldmap;

	int posX, posY;
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
		Globals.RoomManager.ComputeStartLocation(out posX, out posY);
	}
	
	void Update()
	{
		moveTimeout += MyTime.deltaTime;
		if(moveTimeout >= MOVE_TIMEOUT) {
			moveTimeout = 0.0f;
			int x = posX;
			int y = posY;
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
				posX = x;
				posY = y;
			}
			this.transform.position = new Vector3(posX,posY,-1);
			Room newRoom = Globals.RoomManager.GetRoom(posX, posY);
			if(newRoom != null && newRoom != Globals.RoomManager.currentRoom) {
				// ENTER ROOM
				LevelManager.GotoRoom(newRoom);
			}
		}
		if(Input.GetMouseButtonDown(1)) {
			Globals.RoomManager.CreateNew();
			Worldmap.UpdateMondrian();
			Reposition();
		}
	}
}
