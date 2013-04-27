using System;
using UnityEngine;
using System.Collections.Generic;

public static class LevelMesh
{
	private const float BORDER = 20.0f;
		
	private static float Coord(int x, int n) {
		float v = (float)x - ((float)n)*0.5f;
		if(x < 0) {
			v += 1.0f;
		}
		if(x == -2) {
			v -= BORDER;
		}
		if(x > n) {
			v -= 1.0f;
		}
		if(x == n+2) {
			v += BORDER;
		}
		return v;
	}
	
	private static float CoordZ(int x, int m, int y, int n) {
		if((x < 0 || m < x) || (y < 0 || n < y)) {
			return -1.0f;
		}
		else {
			return 0.0f;
		}
	}
	
	private static Mesh CreateLevelMesh(int rows, int cols)
	{
		Mesh mesh = new Mesh();
		
		int M = cols + 5;
		
		Vector3[] vertices = new Vector3[(rows + 5)*M];
		for(int y=-2; y<=rows+2; y++) {
			float yf = Coord(y, rows);
			for(int x=-2; x<=cols+2; x++) {
				float xf = Coord(x, cols);
				float zf = CoordZ(x, cols, y, rows);
				vertices[(x+2) + M*(y+2)] = new Vector3(xf, yf, zf);
			}
		}
		mesh.vertices = vertices;
		
		int[] indices = new int[2*3*(rows + 4)*(cols + 4)];
		for(int y=0; y<rows+4; y++) {
			for(int x=0; x<cols+4; x++) {
				int i = 6*(x + (cols+4)*y);
				int q = x + M*y;
				indices[i] = q;
				indices[i+1] = q+1+M;
				indices[i+2] = q+1;
				indices[i+3] = q;
				indices[i+4] = q+M;
				indices[i+5] = q+1+M;
			}
		}		
		mesh.triangles = indices;
		
		mesh.RecalculateNormals();
		
		return mesh;
	}
	
	static void AddQuad(List<Vector3> vertices, List<int> indices, Vector3[] quad)
	{
		int n = vertices.Count;
		vertices.AddRange(quad);
		indices.AddRange(new int[] { n, n+2, n+1, n, n+3, n+2 });
	}

	public static bool IsUp(int[,] level, int x, int y) {
		return !(
			   0 <= x && x < level.GetLength(1)
			&& 0 <= y && y < level.GetLength(0)
			&& level[y,x] == 0);
	}
	
	public static Mesh Create(int[,] level)
	{
		int rows = level.GetLength(0);
		int cols = level.GetLength(1);
		
		Vector3 center = new Vector3(((float)cols)*0.5f, ((float)rows)*0.5f, 0.0f);
		
		List<Vector3> vertices = new List<Vector3>();
		List<int> indices = new List<int>();
		
		// first: floor and ceiling
		for(int y=0; y<rows; y++) {
			for(int x=0; x<cols; x++) {
				float h = (IsUp(level, x, y) ? -1.0f : 0.0f);
				Vector3[] quad = new Vector3[] {
					new Vector3(x  ,y  ,h) - center,
					new Vector3(x+1,y  ,h) - center,
					new Vector3(x+1,y+1,h) - center,
					new Vector3(x  ,y+1,h) - center
				};
				AddQuad(vertices, indices, quad);
			}
		}

		// second: walls iterior
		for(int y=-1; y<rows; y++) {
			for(int x=-1; x<cols; x++) {
				bool up0 = IsUp(level,x,y);
				bool upx = IsUp(level,x+1,y);
				bool upy = IsUp(level,x,y+1);
				if(up0 != upx) {
					// add wall to x side
					if(up0) {
						Vector3[] quad = new Vector3[] {
							new Vector3(x+1,y  ,0 ) - center,
							new Vector3(x+1,y+1,0 ) - center,
							new Vector3(x+1,y+1,-1) - center,
							new Vector3(x+1,y  ,-1) - center
						};
						AddQuad(vertices, indices, quad);
					}
					else {
						Vector3[] quad = new Vector3[] {
							new Vector3(x+1,y  ,0 ) - center,
							new Vector3(x+1,y  ,-1) - center,
							new Vector3(x+1,y+1,-1) - center,
							new Vector3(x+1,y+1,0 ) - center
						};
						AddQuad(vertices, indices, quad);
					}
				}
				if(up0 != upy) {
					// add wall to y side
					if(up0) {
						Vector3[] quad = new Vector3[] {
							new Vector3(x  ,y+1,0 ) - center,
							new Vector3(x  ,y+1,-1) - center,
							new Vector3(x+1,y+1,-1) - center,
							new Vector3(x+1,y+1,0 ) - center
						};
						AddQuad(vertices, indices, quad);
					}
					else {
						Vector3[] quad = new Vector3[] {
							new Vector3(x  ,y+1,0 ) - center,
							new Vector3(x+1,y+1,0 ) - center,
							new Vector3(x+1,y+1,-1) - center,
							new Vector3(x  ,y+1,-1) - center
						};
						AddQuad(vertices, indices, quad);
					}
				}
			}
		}

		// third: walls boundary
		for(int y=0; y<rows-1; y++) {
			for(int x=0; x<cols-1; x++) {
			}
		}
		
		Mesh mesh = new Mesh();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = indices.ToArray();
		mesh.RecalculateNormals();
		return mesh;
	}
}
