using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class MoreMath
{
	public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
	{
		return source.OrderBy<T, int>(x => Random.Range(0, 1024));
	}

	public static int WeightedSampleCFS(float[] cfs) {
		float r = cfs.Last() * Random.value;
		for(int i=0; i<cfs.Length; i++) {
			if(cfs[i] >= r) {
				return i;
			}
		}
		return cfs.Length - 1;
	}

	public static int WeightedSample(float[] weights) {
		float[] cfs = new float[weights.Length];
		cfs[0] = weights[0];
		for(int i=1; i<cfs.Length; i++) {
			cfs[i] = weights[i] + cfs[i-1];
		}
		return WeightedSampleCFS(cfs);
	}

	public static int RandomRound(float v) {
		float nf = Mathf.Floor(v);
		int n = (int)nf;
		float r = v - (float)nf;
		if(Random.value < r) {
			return n + 1;
		}
		else {
			return n;
		}
	}

	/** Parabel definiert durch Scheitelpunkt (sx, sy) und Steigung a
	 * http://de.wikipedia.org/wiki/Parabel_%28Mathematik%29#Scheitelform
	 */
	public static float Parabel(float x, float sx, float sy, float a) {
		float q = x - sx;
		return a*q*q + sy;
	}

	/** Parabel definiert durch Scheitelpunkt (sx, sy) und einen weiteren Punkt (px, py) */
	public static float Parabel(float x, float sx, float sy, float px, float py) {
		float dx = px - sx;
		float a = (py - sy) / (dx * dx);
		return Parabel(x, sx, sy, a);
	}

	public static float Clamp(float x, float min, float max) {
		return Mathf.Max(min, Mathf.Min(max, x));
	}

	public static int Clamp(int x, int min, int max) {
		return (min < x ? min : (max > x ? max : x));
	}

	/** Computes a an integer and 0 <= b < n, such that a*n + b = i and returns b. */
	public static int ModPos(int i, int n) {
		while(i < 0) i += n;
		return i % n;
	}

	public static float Clamp(float x) {
		return Clamp(x, 0.0f, 1.0f);
	}

	public static float VectorAngle(Vector3 v) {
		return Mathf.Atan2(v.y, v.x);
	}

	public static Quaternion RotAngle(float angle) {
		return Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.forward);
	}

	public static Quaternion RotHeading(Vector3 heading) {
		return RotAngle(VectorAngle(heading));
	}

	public static Color IndicatorColor(float x) {
		x = Clamp(x, 0, 1);
		return new Color(1-x, x, 0);
	}

	public static Color IndicatorColorPosNeg(float x) {
		x = Clamp(x, -1, 1);
		return new Color(0.5f*(1.0f + x), 0.0f, 0.5f*(1.0f - x));
	}

	public static Vector3 RandomInsideUnitCircle3 {
		get {
			Vector2 u = Random.insideUnitCircle;
			return new Vector3(u.x, u.y, 0.0f);
		}
	}

	public static float PoissonProbability(float frequency, float dt) {
		return 1.0f - Mathf.Exp(- frequency * dt);
	}

	public static bool CheckOccurence(float frequency) {
		return Random.value < PoissonProbability(frequency, MyTime.deltaTime);
	}

}