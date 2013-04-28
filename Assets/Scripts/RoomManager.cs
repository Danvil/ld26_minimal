using UnityEngine;
using System.Collections.Generic;

public class Room
{
	public int x1, x2;
	public int y1, y2;
	public Color32 color;
	public bool isCleared = false;

	public bool Inside(int x, int y)
	{
		return x1 <= x && x < x2 && y1 <= y && y < y2;
	}
}

public class RoomManager
{

	int rows, cols;

	Color32[,] colors;

	List<Room> rooms = new List<Room>();

	public Room currentRoom;

	public int Rows { get { return rows; } }

	public int Cols { get { return cols; } }

	public Color32[,] Colors { get { return colors; } }

	public Room[] Rooms { get { return rooms.ToArray(); } }

	public RoomManager()
	{
		CreateNew();
	}

	public void CreateNew()
	{
		rows = 42;
		cols = 42;
		colors = Mondrian.CreateMondrian(rows, cols);
		rooms = CreateRooms();
		// FIXME find start room
		currentRoom = rooms[0];
		currentRoom.isCleared = true;
	}

	public void ComputeStartLocation(out int x, out int y)
	{
		x = currentRoom.x1+1;
		y = currentRoom.y1+1;
	}

	public bool IsValid(int x, int y)
	{
		if(x < 0 || cols <= x || y < 0 || rows <= y) {
			return false;
		}
		//// ignore border
		//if(SameColor(colors[y,x], Mondrian.BORDER)) {
		//	return false;
		//}
		return true;
	}

	public static bool SameColor(Color32 x, Color32 y) {
		return x.r == y.r && x.g == y.g && x.b == y.b && x.a == y.a;
	}

	bool CreateRoomsIsValid(int x, int y, int[] visited) {
		if(x < 0 || cols <= x || y < 0 || rows <= y) {
			return false;
		}
		// ignore visited
		if(visited[x+y*cols] == 1) {
			return false;
		}
		// ignore border
		if(SameColor(colors[y,x], Mondrian.BORDER)) {
			return false;
		}
		return true;
	}

	List<Room> CreateRooms()
	{
		List<Room> rooms = new List<Room>();
		int[] visited = new int[rows*cols];
		for(int i=0; i<visited.Length; i++) visited[i] = 0;
		for(int y=0; y<rows; y++) {
			for(int x=0; x<cols; x++) {
				if(!CreateRoomsIsValid(x, y, visited)) {
					continue;
				}
				// find room
				// HACK assume rooms are rectangular and unicolor
				int j = x;
				while(CreateRoomsIsValid(j,y,visited)) j++;
				int i = y;
				while(CreateRoomsIsValid(x,i,visited)) i++;
				// create room
				Room room = new Room();
				room.x1 = x;
				room.x2 = j;
				room.y1 = y;
				room.y2 = i;
				room.color = colors[y,x];
				// mark invalid
				for(int s=room.y1; s<room.y2; s++) {
					for(int t=room.x1; t<room.x2; t++) {
						visited[t+s*cols] = 1;
					}
				}
				rooms.Add(room);
			}
		}
		return rooms;
	}

	public Room GetRoom(int x, int y)
	{
		foreach(Room r in rooms) {
			if(r.Inside(x,y)) {
				return r;
			}
		}
		return null;
	}


}
