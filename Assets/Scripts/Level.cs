using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
	public Light pfLight;
	
	int[,] level;
	
	void GenerateLevel()
	{
		level = new int[6,10] {
			{0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0},
			{0,0,1,1,1,1,0,0,0,0},
			{0,0,1,0,0,1,0,0,1,1},
			{0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0},
		};
		
		Mesh mesh = LevelMesh.Create(level);
		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
		
		int rows = level.GetLength(0);
		int cols = level.GetLength(1);

		if(pfLight) {
			// only place few lights because of limit ...
			float x1, x2, y1, y2;
			if(rows < cols) {
				x1 = 0.25f;
				x2 = 0.75f;
				y1 = 0.5f;
				y2 = 0.5f;
			}		
			else {
				x1 = 0.5f;
				x2 = 0.5f;
				y1 = 0.25f;
				y2 = 0.75f;
			}
			Light go1 = (Light)Instantiate(pfLight);
			go1.transform.parent = this.transform;
			go1.transform.localPosition = new Vector3(x1 * (float)cols, y1 * (float)rows, -3.0f);
			Light go2 = (Light)Instantiate(pfLight);
			go2.transform.parent = this.transform;
			go2.transform.localPosition = new Vector3(x2 * (float)cols, y2 * (float)rows, -3.0f);

			// // many lights
			// for(int y=1; y<rows-1; y+=1) {
			// 	for(int x=1; x<cols-1; x+=1) {
			// 		if(level[y,x] == 0 && level[y-1,x] == 0 && level[y+1,x] == 0 && level[y,x-1] == 0 && level[y,x+1] == 0) {
			// 			Light go = (Light)Instantiate(pfLight);
			// 			go.transform.parent = this.transform;
			// 			go.transform.position = new Vector3(0.5f + (float)x, 0.5f + (float)y, -0.5f);
			// 		}
			// 	}
			// }
		}
		
		this.transform.position = - new Vector3(((float)cols)*0.5f, ((float)rows)*0.5f, 0.0f);

	}
	
	// Use this for initialization
	void Start () {
		GenerateLevel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}