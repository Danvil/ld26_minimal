using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class LevelShapePlacer
{
	public static void Place(int[,] level)
	{
		int rows = level.GetLength(0);
		int cols = level.GetLength(1);
		List<int[,]> shapes = CreateShapesAll();
		List<Loc> locs0 = GetAllLocations(rows, cols);
		while(shapes.Count > 0) {
			// select random shape
			int[,] s = shapes[Random.Range(0, shapes.Count)];
			// try to place shape
			bool canPlace = false;
			foreach(Loc p in locs0.Randomize()) {
				if(CanPlace(level, p.x, p.y, s)) {
					// place
					Place(level, p.x, p.y, s);
					canPlace = true;
					break;
				}
			}
			if(!canPlace) {
				shapes.Remove(s);
			}
		}
	}

	public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
	{
		return source.OrderBy<T, int>(x => Random.Range(0, 1024));
	}

	struct Loc
	{
		public int x, y;

		public Loc(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	static bool CanPlace(int[,] level, int cx, int cy, int[,] shape) {
		int rows = level.GetLength(0);
		int cols = level.GetLength(1);
		int srows = shape.GetLength(0);
		int scols = shape.GetLength(1);
		if(!(2 <= cx && cx+scols <= cols-2 && 2 <= cy && cy+srows <= rows-2)) {
			return false;
		}
		for(int y=-2; y<srows+2; y++) {
			for(int x=-2; x<scols+2; x++) {
				if(level[cy+y,cx+x] == 1) {
					return false;
				}
			}
		}
		return true;
	}

	static void Place(int[,] level, int cx, int cy, int[,] shape) {
		int srows = shape.GetLength(0);
		int scols = shape.GetLength(1);
		for(int y=0; y<srows; y++) {
			for(int x=0; x<scols; x++) {
				level[cy+y,cx+x] = shape[y,x];
			}
		}
	}

	static List<Loc> GetAllLocations(int rows, int cols) {
		List<Loc> locs = new List<Loc>();
		for(int y=0; y<rows; y++) {
			for(int x=0; x<cols; x++) {
				locs.Add(new Loc(x,y));
			}
		}
		return locs;
	}

	static List<int[,]> CreateShapesAll()
	{
		return CreateShapes().SelectMany(s => TransformAll(s).MyDistinctBy()).ToList();
	}

	static bool Equal(int[,] a, int[,] b) {
		if(a.GetLength(0) != b.GetLength(0)) return false;
		if(a.GetLength(1) != b.GetLength(1)) return false;
		int rows = a.GetLength(0);
		int cols = a.GetLength(1);
		for(int y=0; y<rows; y++) {
			for(int x=0; x<cols; x++) {
				if(a[y,x] != b[y,x]) return false;
			}
		}
		return true;
	}

	public static List<int[,]> MyDistinctBy(this List<int[,]> e) {
		List<int[,]> v = new List<int[,]>();
		for(int i=0; i<e.Count; i++) {
			bool has = false;
			for(int j=0; j<v.Count; j++) {
				if(Equal(e[i], v[j])) {
					has = true;
					break;
				}
			}
			if(!has) {
				v.Add(e[i]);
			}
		}
		return v;
	}

	static int[,] Rotate(int[,] s) {
		int rows = s.GetLength(0);
		int cols = s.GetLength(1);
		int[,] r = new int[cols,rows];
		for(int y=0; y<rows; y++) {
			for(int x=0; x<cols; x++) {
				r[cols-1-x,y] = s[y,x];
			}
		}
		return r;
	}

	static int[,] FlipX(int[,] s) {
		int rows = s.GetLength(0);
		int cols = s.GetLength(1);
		int[,] r = new int[rows,cols];
		for(int y=0; y<rows; y++) {
			for(int x=0; x<cols; x++) {
				r[y,cols-1-x] = s[y,x];
			}
		}
		return r;
	}

	static List<int[,]> TransformAll(int[,] s) {
		var r1 = Rotate(s);
		var r2 = Rotate(r1);
		var r3 = Rotate(r2);
		var f1 = FlipX(s);
		var f2 = Rotate(f1);
		var f3 = Rotate(f2);
		var f4 = Rotate(f3);
		List<int[,]> v = new List<int[,]>();
		v.Add(s);
		v.Add(r1);
		v.Add(r2);
		v.Add(r3);
		v.Add(f1);
		v.Add(f2);
		v.Add(f3);
		v.Add(f4);
		return v;
	}

	static List<int[,]> CreateShapes()
	{
		List<int[,]> shapes = new List<int[,]>();
		shapes.Add(new int[,] {
			{1}
		});
		shapes.Add(new int[,] {
			{1,1}
		});
		shapes.Add(new int[,] {
			{1,1,1}
		});
		shapes.Add(new int[,] {
			{1,1,1,1}
		});
		shapes.Add(new int[,] {
			{1,1},
			{1,1},
		});
		shapes.Add(new int[,] {
			{1,0},
			{1,0},
			{1,1}
		});
		shapes.Add(new int[,] {
			{1,0},
			{1,1},
			{0,1}
		});
		shapes.Add(new int[,] {
			{1,0},
			{1,1},
			{1,0}
		});
		shapes.Add(new int[,] {
			{1,1,1},
			{1,1,1},
			{1,1,1}
		});
		shapes.Add(new int[,] {
			{0,1,0},
			{1,1,1},
			{0,1,0}
		});
		shapes.Add(new int[,] {
			{1,1,0},
			{0,1,0},
			{0,1,1}
		});
		shapes.Add(new int[,] {
			{1,1,1},
			{1,0,0},
			{1,0,0}
		});
		shapes.Add(new int[,] {
			{1,1,1,1},
			{1,0,0,1},
			{1,0,0,1}
		});
		shapes.Add(new int[,] {
			{1,0,0,0},
			{1,1,1,1},
			{0,0,0,1}
		});
		shapes.Add(new int[,] {
			{0,0,1,0},
			{1,1,1,1},
			{0,0,1,0}
		});
		shapes.Add(new int[,] {
			{1,0,0,1},
			{1,1,1,1},
			{1,0,0,1}
		});
		return shapes;
	}
}
