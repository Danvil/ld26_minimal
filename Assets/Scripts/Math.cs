using System;
using UnityEngine;


public static class Math
{
	public static Vector3 WithChangedX(this Vector3 v, float x)
	{
		return new Vector3(x, v.y, v.z);
	}
	
	public static Vector3 WithChangedY(this Vector3 v, float y)
	{
		return new Vector3(v.x, y, v.z);
	}
	
	public static Vector3 WithChangedZ(this Vector3 v, float z)
	{
		return new Vector3(v.x, v.y, z);
	}
	
}


