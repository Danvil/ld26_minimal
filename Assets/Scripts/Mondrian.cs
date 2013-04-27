using UnityEngine;
using System.Collections;

public class Mondrian : MonoBehaviour
{
	
	public static void DrawHorzLine(Color32[,] img, int y, int x1, int x2, Color32 col)
	{
		for(int x=x1; x<=x2; x++) {
			img[y,x] = col;
		}
	}

	public static void DrawVertLine(Color32[,] img, int x, int y1, int y2, Color32 col)
	{
		for(int y=y1; y<=y2; y++) {
			img[y,x] = col;
		}
	}
	
	public static void DrawRect(Color32[,] img, int x1, int x2, int y1, int y2, Color32 col)
	{
		DrawVertLine(img, x1, y1, y2, col);
		DrawVertLine(img, x2, y1, y2, col);
		DrawHorzLine(img, y1, x1, x2, col);
		DrawHorzLine(img, y2, x1, x2, col);
	}
	
	public static void FillRect(Color32[,] img, int x1, int x2, int y1, int y2, Color32 col)
	{
		for(int y=y1; y<=y2; y++) {
			DrawHorzLine(img, y, x1, x2, col);
		}
	}

	public static Color32[,] CreateMondrian(int rows, int cols)
	{
		Color32[,] c = new Color32[rows,cols];
		Color32 black = new Color32(0,0,0,255);
		Color32 white = new Color32(255,255,255,255);
		FillRect(c, 0, cols-1, 0, rows-1, white);
		DrawRect(c, 0, cols-1, 0, rows-1, black);
		return c;
	}
	
	
	public static Texture2D CreateTexture(int rows, int cols)
	{
		var tex = new Texture2D(rows, cols, TextureFormat.ARGB32, false);
		tex.filterMode = FilterMode.Point;
		tex.wrapMode = TextureWrapMode.Clamp;
		return tex;
	}

	public static void SetTexture(Texture2D tex, Color32[,] colors)
	{
		int rows = colors.GetLength(0);
		int cols = colors.GetLength(1);
		Color32[] buff = new Color32[rows*cols];
		for(int y=0; y<rows; y++) {
			for(int x=0; x<cols; x++) {
				buff[x+y*cols] = colors[y,x];
			}
		}
		tex.SetPixels32(buff);
		tex.Apply();
	}
	
	Texture2D tex;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		int rows = 32;
		int cols = 32;
		if(tex == null || tex.width != cols || tex.height != rows) {
			tex = CreateTexture(rows, cols);
			renderer.material.mainTexture = tex;
		}
		SetTexture(tex, CreateMondrian(rows, cols));
	}
}
