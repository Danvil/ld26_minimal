using UnityEngine;
using System.Collections;

public class WorldmapPlayer : MonoBehaviour {

	const float MOVE_TIMEOUT = 0.1f;

	public Worldmap Worldmap;

	int posX, posY;
	Room currentRoom;
	float moveTimeout = 0.0f;

	void Awake()
	{
	}

	// Use this for initialization
	void Start () {
		Globals.RoomManager.ComputeStartLocation(out posX, out posY);
		currentRoom = Globals.RoomManager.GetRoom(posX, posY);
	}
	
	// Update is called once per frame
	void Update () {
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
			if(newRoom != null && newRoom != currentRoom) {
				currentRoom = newRoom;
				if(!currentRoom.isCleared) {
					// ENTER ROOM
					currentRoom.isCleared = true;
					LevelManager.GotoRoom(currentRoom);
				}
			}
		}
	}
}
