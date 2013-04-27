using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
	public Light pfLight;

	public GameObject pfCoin;
	
	public GameObject pfEnemy;
	
	int[,] levelPlan;
	
	int[,] level;
	
	public Vector3 PlayerStart { get; private set; }
	
	public Vector3 LevelCenter {
		get {
			return new Vector3(0.5f * (float)level.GetLength(1), 0.5f * (float)level.GetLength(0), 0.0f);
		}
	}
	
	public bool IsPathBlocked(Vector3 a, Vector3 b)
	{
		a = new Vector3(a.x, a.y, -0.5f);
		b = new Vector3(b.x, b.y, -0.5f);
		Vector3 d = b - a;
		Ray ray = new Ray(a, d.normalized);
		RaycastHit hitInfo;
		return Collider.Raycast(ray, out hitInfo, d.magnitude);
	}
	
	public Collider Collider { get; private set; }
	
	public bool IsFree(int x, int y)
	{
		return 0 <= x && x < level.GetLength(1)
			&& 0 <= y && y < level.GetLength(0)
 			&& level[y,x] == 0;
	}
	
	public bool IsBlocking(int x, int y)
	{
		return !IsFree(x,y);
	}
	
	public bool IsBlocking(Vector3 v)
	{
		return IsBlocking((int)v.x, (int)v.y);
	}
	
	public bool IsBlocking(Vector3 v, float r)
	{
		return IsBlocking(v + new Vector3(0,0,0))
			|| IsBlocking(v + new Vector3(-r,0,0))
			|| IsBlocking(v + new Vector3(+r,0,0))
			|| IsBlocking(v + new Vector3(0,-r,0))
			|| IsBlocking(v + new Vector3(0,+r,0));
	}

	void PlanLevel()
	{
		// 0 = free, 1 = blocked
		// 3 = player
		// 8 = enemy, 7 = coin
		levelPlan = new int[6,10] {
			{7,0,7,0,7,0,7,0,7,0},
			{0,0,0,0,0,0,8,0,0,7},
			{7,0,1,0,8,1,0,0,1,1},
			{0,0,1,1,1,1,0,8,0,7},
			{7,0,0,0,0,0,0,0,0,0},
			{3,7,0,7,0,7,0,7,0,7},
		};
	}
	
	void GenerateMesh()
	{
		Mesh mesh = LevelMesh.Create(level);
		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;	
		this.Collider = GetComponent<MeshCollider>();
	}

	void GenerateLights()
	{
		int rows = level.GetLength(0);
		int cols = level.GetLength(1);
		
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
	
	void GenerateLevel()
	{
		level = new int[levelPlan.GetLength(0), levelPlan.GetLength(1)];
		for(int y=0; y<level.GetLength(0); y++) {
			for(int x=0; x<level.GetLength(1); x++) {
				int q = levelPlan[y,x];
				Vector3 pos = new Vector3(0.5f + (float)x, 0.5f + (float)y, 0.0f);
				// blocking
				level[y,x] = (q == 1 ? 1 : 0);
				// coin
				if(q == 7) {
					GameObject go = (GameObject)Instantiate(pfCoin);
					go.transform.parent = this.transform;
					go.transform.localPosition = pos;
				}
				// enemy
				if(q == 8) {
					GameObject go = (GameObject)Instantiate(pfEnemy);
					go.transform.parent = this.transform;
					go.transform.localPosition = pos;
				}
				// player start
				if(q == 3) {
					PlayerStart = pos;
				}
			}
		}
	}

	void Awake()
	{
		Globals.Level = this;
		PlanLevel();
	}
	
	void Start ()
	{
		GenerateLevel();
		GenerateMesh();
		GenerateLights();
	}
	
	void Update ()
	{}

}
