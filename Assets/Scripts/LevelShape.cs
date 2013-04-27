using UnityEngine;
using System.Collections;

public class LevelShape : MonoBehaviour
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
	
	// Use this for initialization
	void Start () {
		GetComponent<MeshFilter>().mesh = CreateLevelMesh(6,10);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
