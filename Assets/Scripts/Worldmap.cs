using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Worldmap : MonoBehaviour {

	Texture2D mondrianTex;

	Texture2D highlightTex;

	static Color32[] ImageToArray(Color32[,] img)
	{
		int rows = img.GetLength(0);
		int cols = img.GetLength(1);
		Color32[] buff = new Color32[rows*cols];
		for(int y=0; y<rows; y++) {
			for(int x=0; x<cols; x++) {
				buff[x+y*cols] = img[y,x];
			}
		}
		return buff;
	}
	
	Texture2D CreateTexture(Color32[,] img)
	{
		int rows = img.GetLength(0);
		int cols = img.GetLength(1);
		var tex = new Texture2D(cols, rows, TextureFormat.ARGB32, false);
		tex.filterMode = FilterMode.Point;
		tex.wrapMode = TextureWrapMode.Clamp;
		tex.SetPixels32(ImageToArray(img));
		tex.Apply();
		return tex;
	}

	Texture2D ComputeHighlightTexture()
	{
		int rows = Globals.RoomManager.Rows;
		int cols = Globals.RoomManager.Cols;
		Color32[,] img = new Color32[rows,cols];
		Mondrian.DrawRect(img, 0, cols, 0, rows, new Color32(0,0,0,255));
		foreach(Room r in Globals.RoomManager.Rooms) {
			Mondrian.FillRect(img, r.x1, r.x2, r.y1, r.y2, new Color32(255,128,0,255));
		}
		return CreateTexture(img);
	}
	
	// public static Texture2D UpdateMondrian(Texture2D tex)
	// {
	// 	int rows = 42;
	// 	int cols = 42;
	// 	if(tex == null || tex.width != cols || tex.height != rows) {
	// 		tex = CreateTexture(rows, cols);
	// 	}
	// 	SetTexture(tex, ImageToArray(CreateMondrian(rows, cols)));
	// 	return tex;
	// }

	public void UpdateMondrian()
	{
		mondrianTex = CreateTexture(Globals.RoomManager.Colors);
		highlightTex = ComputeHighlightTexture();
		renderer.material.mainTexture = mondrianTex;
	}

	void Awake()
	{
		if(Globals.RoomManager == null) {
			Globals.RoomManager = new RoomManager();
			UpdateMondrian();
			Globals.RoomManager.ComputeStartLocation(out LevelManager.PlayerPosX, out LevelManager.PlayerPosY);
		}
		else {
			UpdateMondrian();
		}
	}

	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
	}
}
