using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Mondrian : MonoBehaviour
{
	
	public class Node
	{
		public int x1, x2, y1, y2;
		public Color32 color;
		public Node[] children = new Node[] {};
	};

	static bool MondrianSplit(Node n)
	{
		// x1     x2
		// |      |
		// 0123456789
		// XOOSOOX

		// 012345678901234567
		// XOOSSSOOXOOSSSOOX=
		// XOOSSOOXOOSSOOX===
		// XOOSOOXOOSOOX=====

		// can split?
		int dx = (n.x2 - n.x1) - 12;
		int dy = (n.y2 - n.y1) - 12;
		if(dx > 0 || dy > 0) {
			// decide randomly if splitting is done
			if(Random.value < -0.037f + 0.8f / Mathf.Sqrt((float)(System.Math.Max(dx+1,1)*System.Math.Max(dy+1,1)))) {
				return false;
			}
			bool splitx;
			if(dx <= 0) {
				splitx = false;
			}
			else if(dy <= 0) {
				splitx = true;
			}
			else {
				// split x or y?
				float px = (float)dx / ((float)(dx + dy));
				splitx = (Random.value < px);
			}
			// do splitting
			if(splitx) {
				int xs = Random.Range(n.x1+6, n.x2-7);
				Node n1 = new Node();
				n1.x1 = n.x1;
				n1.x2 = xs + 1;
				n1.y1 = n.y1;
				n1.y2 = n.y2;
				Node n2 = new Node();
				n2.x1 = xs;
				n2.x2 = n.x2;
				n2.y1 = n.y1;
				n2.y2 = n.y2;
				n.children = new Node[] { n1, n2 };
			}
			else {
				int ys = Random.Range(n.y1+6, n.y2-7);
				Node n1 = new Node();
				n1.x1 = n.x1;
				n1.x2 = n.x2;
				n1.y1 = n.y1;
				n1.y2 = ys + 1;
				Node n2 = new Node();
				n2.x1 = n.x1;
				n2.x2 = n.x2;
				n2.y1 = ys;
				n2.y2 = n.y2;
				n.children = new Node[] { n1, n2 };
			}
			return true;
		}
		else {
			return false;
		}
	}

	static Color32[] colors = new Color32[] {
		new Color32(255,255,255,255), // white
		new Color32(255,  0,  0,255), // red
		new Color32(  0,  0,255,255), // blue
		new Color32(255,255,  0,255)  // yellow
	};

	static float[] ColorWeights(int[] nums)
	{
		// each color at least once!
		if(nums[0] == 0) return new float[] { 1,0,0,0 };
		if(nums[1] == 0) return new float[] { 0,1,0,0 };
		if(nums[2] == 0) return new float[] { 0,0,1,0 };
		if(nums[3] == 0) return new float[] { 0,0,0,1 };
		// desired proportion:
		float[] desired = new float[] { 0.5f, 0.2f, 0.1f, 0.2f };
		// adapt
		int total = nums.Sum();
		float[] props = new float[nums.Length];
		for(int i=0; i<props.Length; i++) {
			float d = desired[i];
			float a = (float)(nums[i]) / (float)total;
			float p = 1.0f / (1.0f + Mathf.Exp(-10.0f*(d - a)));
			props[i] = d*p;
		}
		return props;
	}

	static Node MondrianGraph(int rows, int cols)
	{
		List<Node> nodes = new List<Node>();
		List<Node> closed = new List<Node>();
		// start node
		Node root = new Node();
		root.x1 = 0;
		root.x2 = cols;
		root.y1 = 0;
		root.y2 = rows;
		root.color = new Color32(255,255,255,255);
		nodes.Add(root);
		// split nodes recursively
		while(nodes.Count > 0) {
			// pick random node
			Node n = nodes[Random.Range(0, nodes.Count)];
			// split node
			MondrianSplit(n);
			// remove and add children
			nodes.Remove(n);
			closed.Add(n);
			nodes.AddRange(n.children);
		}
		// color mondrian
		int[] colornums = new int[] { 0, 0, 0, 0 };
		foreach(Node n in closed) {
			float[] w = ColorWeights(colornums);
			int i = MoreMath.WeightedSample(w);
			colornums[i] ++;
			n.color = colors[i];
		}
		return root;
	}

	public static void DrawHorzLine(Color32[,] img, int y, int x1, int x2, Color32 col)
	{
		for(int x=x1; x<x2; x++) {
			img[y,x] = col;
		}
	}

	public static void DrawVertLine(Color32[,] img, int x, int y1, int y2, Color32 col)
	{
		for(int y=y1; y<y2; y++) {
			img[y,x] = col;
		}
	}
	
	public static void DrawRect(Color32[,] img, int x1, int x2, int y1, int y2, Color32 col)
	{
		DrawVertLine(img, x1, y1, y2, col);
		DrawVertLine(img, x2-1, y1, y2, col);
		DrawHorzLine(img, y1, x1, x2, col);
		DrawHorzLine(img, y2-1, x1, x2, col);
	}
	
	public static void FillRect(Color32[,] img, int x1, int x2, int y1, int y2, Color32 col)
	{
		for(int y=y1; y<y2; y++) {
			DrawHorzLine(img, y, x1, x2, col);
		}
	}

	static void PaintMondrian(Color32[,] img, Node n)
	{
		FillRect(img, n.x1, n.x2, n.y1, n.y2, n.color);
		DrawRect(img, n.x1, n.x2, n.y1, n.y2, new Color32(0,0,0,255));
		foreach(Node c in n.children) {
			PaintMondrian(img, c);
		}
	}

	public static Color32[,] CreateMondrian(int rows, int cols)
	{
		Color32[,] c = new Color32[rows,cols];
		PaintMondrian(c, MondrianGraph(rows, cols));
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
	
	void UpdateMondrian()
	{
		int rows = 42;
		int cols = 42;
		if(tex == null || tex.width != cols || tex.height != rows) {
			tex = CreateTexture(rows, cols);
			renderer.material.mainTexture = tex;
		}
		SetTexture(tex, CreateMondrian(rows, cols));
	}
	
	// Use this for initialization
	void Start()
	{
		UpdateMondrian();
	}
	
	// Update is called once per frame
	void Update()
	{
		if(Input.GetMouseButtonDown(1)) {
			UpdateMondrian();
		}
	}
}
