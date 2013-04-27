using System;
using UnityEngine;
using System.Collections.Generic;

public static class LevelMesh
{
	private const int BORDER_NUM = 5;
	
		
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
		
		List<Vector3> vertices = new List<Vector3>();
		List<int> indices = new List<int>();
		
		// first: floor and ceiling
		for(int y=-BORDER_NUM; y<rows+BORDER_NUM; y++) {
			for(int x=-BORDER_NUM; x<cols+BORDER_NUM; x++) {
				float h = (IsUp(level, x, y) ? -1.0f : 0.0f);
				Vector3[] quad = new Vector3[] {
					new Vector3(x  ,y  ,h),
					new Vector3(x+1,y  ,h),
					new Vector3(x+1,y+1,h),
					new Vector3(x  ,y+1,h)
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
							new Vector3(x+1,y  ,0 ),
							new Vector3(x+1,y+1,0 ),
							new Vector3(x+1,y+1,-1),
							new Vector3(x+1,y  ,-1)
						};
						AddQuad(vertices, indices, quad);
					}
					else {
						Vector3[] quad = new Vector3[] {
							new Vector3(x+1,y  ,0 ),
							new Vector3(x+1,y  ,-1),
							new Vector3(x+1,y+1,-1),
							new Vector3(x+1,y+1,0 )
						};
						AddQuad(vertices, indices, quad);
					}
				}
				if(up0 != upy) {
					// add wall to y side
					if(up0) {
						Vector3[] quad = new Vector3[] {
							new Vector3(x  ,y+1,0 ),
							new Vector3(x  ,y+1,-1),
							new Vector3(x+1,y+1,-1),
							new Vector3(x+1,y+1,0 )
						};
						AddQuad(vertices, indices, quad);
					}
					else {
						Vector3[] quad = new Vector3[] {
							new Vector3(x  ,y+1,0 ),
							new Vector3(x+1,y+1,0 ),
							new Vector3(x+1,y+1,-1),
							new Vector3(x  ,y+1,-1)
						};
						AddQuad(vertices, indices, quad);
					}
				}
			}
		}

		Mesh mesh = new Mesh();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = indices.ToArray();
		mesh.RecalculateNormals();
		return mesh;
	}
}
