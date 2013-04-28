using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Level : MonoBehaviour
{
	public Light pfLight;
	public GameObject pfCoin;
	public GameObject pfEnemy;

	public Material matLevel;
	public Material matCoin;
	public Material matBomb;
	public Material matBombExplosion;
	public Material matEnemy;

	public Color32 ColorWhite	= new Color32(183,183,183,255);
	public Color32 ColorBlue	= new Color32( 41, 89,238,255);
	public Color32 ColorRed		= new Color32(221, 40, 40,255);
	public Color32 ColorYellow	= new Color32(181,179, 20,255);
	public Color32 ColorBlack	= new Color32(  0,  0,  0,255);

	Color32 colorLevel;
	Color32 colorCoin;
	Color32 colorBomb;
	Color32 colorEnemy;
	
	int[,] levelPlan;
	
	int[,] level;
	
	public Vector3[] BlockedCells { get; private set; }
	
	public Vector3 PlayerStart { get; private set; }
	
	public Vector3 LevelCenter {
		get {
			return new Vector3(0.5f * (float)level.GetLength(1), 0.5f * (float)level.GetLength(0), 0.0f);
		}
	}

	public int NumCoins { get; private set; }
	
	public int NumEnemies { get; private set; }
	
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

	public int Rows { get { return level.GetLength(0); } }

	public int Cols { get { return level.GetLength(1); } }
	
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
	
	void PlanLevelTest1()
	{
		// 0 = free, 1 = blocked
		// 9 = player
		// 8 = enemy, 7 = coin
		levelPlan = new int[6,10] {
			{7,0,7,0,7,0,7,0,7,0},
			{0,0,0,0,0,0,8,0,0,7},
			{7,0,1,0,8,1,0,0,2,1},
			{0,0,1,1,1,1,0,8,0,7},
			{7,0,0,0,0,0,0,0,0,0},
			{9,7,0,7,0,7,0,7,0,7},
		};
		ChooseTheme(Mondrian.WHITE);
	}

	void PlanLevelTest2()
	{
		levelPlan = new int[,] {
			{0,7,0,7,8},
			{7,0,1,0,7},
			{0,0,2,0,0},
			{7,0,1,0,7},
			{9,7,0,7,0},
		};
		ChooseTheme(Mondrian.WHITE);
	}

	void PlanShapes()
	{
		LevelShapePlacer.Place(levelPlan);
	}

	void PlanEnemies(int area, float mul)
	{
		int num = MoreMath.RandomRound((float)(area) * mul / 10.0f);
		List<Loc> locs = LevelShapePlacer.GetFreeLocations(levelPlan).Randomize().ToList();
		int numfree = locs.Count;
		int k = 0;
		int placed = 0;
		foreach(Loc p in locs) {
			k += num;
			if(k >= numfree) {
				k -= numfree;
				levelPlan[p.y,p.x] = 8;
				placed ++;
				if(placed == num) {
					return;
				}
			}
		}
	}

	void PlanCoins(int mult)
	{
		int d = 4 - mult;
		int rows = levelPlan.GetLength(0);
		int cols = levelPlan.GetLength(1);
		for(int y=0; y<rows; y+=d) {
			levelPlan[y,     0] += 7;
			levelPlan[y,cols-1] += 7;
		}
		for(int x=0; x<cols; x+=d) {
			levelPlan[0     , x] += 7;
			levelPlan[rows-1, x] += 7;
		}
	}

	void PlanLevel(Room room)
	{
		// create empty room
		int rows = room.y2 - room.y1;
		int cols = room.x2 - room.x1;
		levelPlan = new int[rows,cols];
		for(int y=0; y<rows; y++) {
			for(int x=0; x<cols; x++) {
				levelPlan[y,x] = 0;
			}
		}
		// choose theme
		Color32 theme = room.color;
		ChooseTheme(theme);
		// place stuff
		if(RoomManager.SameColor(theme, Mondrian.WHITE)) {
			PlanShapes();
			PlanEnemies(room.Area, 1.0f);
			PlanCoins(1);
		}
		else if(RoomManager.SameColor(theme, Mondrian.RED)) {
			PlanShapes();
			PlanEnemies(room.Area, 1.8f);
			PlanCoins(2);
		}
		else if(RoomManager.SameColor(theme, Mondrian.BLUE)) {
			PlanShapes();
			PlanEnemies(room.Area, 0.5f);
			PlanCoins(1);
		}
		else if(RoomManager.SameColor(theme, Mondrian.YELLOW)) {
			PlanShapes();
			PlanCoins(3);
		}
		else throw new System.ApplicationException();
		// place player
		int playerX = Globals.PlayerPosX - room.x1;
		int playerY = Globals.PlayerPosY - room.y1;
		levelPlan[playerY,playerX] = 9;
	}

	void ChooseTheme(Color theme)
	{
		if(RoomManager.SameColor(theme, Mondrian.WHITE)) {
			colorLevel = ColorWhite;
			colorCoin = ColorYellow;
			colorBomb = ColorRed;
			colorEnemy = ColorBlue;
		}
		else if(RoomManager.SameColor(theme, Mondrian.RED)) {
			colorLevel = ColorRed;
			colorCoin = ColorYellow;
			colorBomb = ColorWhite;
			colorEnemy = ColorBlue;
		}
		else if(RoomManager.SameColor(theme, Mondrian.BLUE)) {
			colorLevel = ColorBlue;
			colorCoin = ColorYellow;
			colorBomb = ColorRed;
			colorEnemy = ColorBlack;
		}
		else if(RoomManager.SameColor(theme, Mondrian.YELLOW)) {
			colorLevel = ColorYellow;
			colorCoin = ColorRed;
			colorBomb = ColorBlue;
			colorEnemy = ColorBlack;
		}
		else throw new System.ApplicationException();
	}

	void ChangeColors()
	{
		matLevel.color = colorLevel;
		matCoin.color = colorCoin;
		matBomb.color = colorBomb;
		matBombExplosion.SetColor("_TintColor", colorBomb);
		matEnemy.color = colorEnemy;
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
		float w = (float)level.GetLength(1);
		float h = (float)level.GetLength(0);

		int numW = Mathf.Max(1, Mathf.RoundToInt(w / 5.0f));
		int numH = Mathf.Max(1, Mathf.RoundToInt(h / 5.0f));

		float dx = w / (float)numW;
		float dy = h / (float)numH;

		for(int i=0; i<numH; i++) {
			for(int j=0; j<numW; j++) {
				float x = dx * (0.5f + (float)j);
				float y = dy * (0.5f + (float)i);
				Light go = (Light)Instantiate(pfLight);
				go.transform.parent = this.transform;
				go.transform.localPosition = new Vector3(x, y, -3.0f);
			}
		}
		
		// // only place few lights because of limit ...
		// float x1, x2, y1, y2;
		// if(rows < cols) {
		// 	x1 = 0.25f;
		// 	x2 = 0.75f;
		// 	y1 = 0.5f;
		// 	y2 = 0.5f;
		// }		
		// else {
		// 	x1 = 0.5f;
		// 	x2 = 0.5f;
		// 	y1 = 0.25f;
		// 	y2 = 0.75f;
		// }
		// Light go1 = (Light)Instantiate(pfLight);
		// go1.transform.parent = this.transform;
		// go1.transform.localPosition = new Vector3(x1 * (float)cols, y1 * (float)rows, -3.0f);
		// Light go2 = (Light)Instantiate(pfLight);
		// go2.transform.parent = this.transform;
		// go2.transform.localPosition = new Vector3(x2 * (float)cols, y2 * (float)rows, -3.0f);

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
		NumCoins = 0;
		NumEnemies = 0;
		List<Vector3> blockedcells = new List<Vector3>();
		level = new int[levelPlan.GetLength(0), levelPlan.GetLength(1)];
		for(int y=0; y<level.GetLength(0); y++) {
			for(int x=0; x<level.GetLength(1); x++) {
				int q = levelPlan[y,x];
				Vector3 pos = new Vector3(0.5f + (float)x, 0.5f + (float)y, 0.0f);
				// blocking
				level[y,x] = (1 <= q && q <= 3 ? 1 : 0);
				if(level[y,x] == 1) {
					blockedcells.Add(pos);
				}
				// coin
				if(q == 7 || q == 15) {
					NumCoins++;
					GameObject go = (GameObject)Instantiate(pfCoin);
					go.transform.parent = this.transform;
					go.transform.localPosition = pos;
				}
				// enemy
				if(q == 8 || q == 15) {
					NumEnemies++;
					GameObject go = (GameObject)Instantiate(pfEnemy);
					go.transform.parent = this.transform;
					go.transform.localPosition = pos;
				}
				// player start
				if(q == 9) {
					PlayerStart = pos;
				}
			}
		}
		BlockedCells = blockedcells.ToArray();
	}

	void Awake()
	{
		Globals.Level = this;
		if(Globals.RoomManager != null) {
			PlanLevel(Globals.RoomManager.currentRoom);
		}
		else {
			PlanLevelTest2();	
		}
	}

	void Start ()
	{
		ChangeColors();
		GenerateLevel();
		GenerateMesh();
		GenerateLights();
	}
	
	int tmp = 0;

	void Update ()
	{
		if(Input.GetMouseButtonDown(1)) {
			tmp = (tmp + 1) % 4;
			Color32 theme;
			switch(tmp) {
				default: case 0: theme = Mondrian.WHITE; break;
				case 1: theme = Mondrian.RED; break;
				case 2: theme = Mondrian.BLUE; break;
				case 3: theme = Mondrian.YELLOW; break;
			}
			ChooseTheme(theme);
			ChangeColors();
		}
	}

}
