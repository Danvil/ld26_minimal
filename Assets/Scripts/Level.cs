using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
	int[,] level;
	
	void GenerateLevel()
	{
		level = new int[6,10] {
			{0,0,1,0,0,0,1,0,0,0},
			{0,0,0,0,0,0,1,0,0,0},
			{0,1,1,1,1,0,1,1,0,0},
			{0,1,0,0,1,0,0,0,0,0},
			{0,1,0,0,1,0,0,0,1,0},
			{0,0,0,0,0,0,0,0,1,0},
		};
	}
	
	// Use this for initialization
	void Start () {
		GenerateLevel();
		Mesh mesh = LevelMesh.Create(level);
		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
