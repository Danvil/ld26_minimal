using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour {
	
	const float HEART_ALPHA_DECAY = 0.3f;
	
	public Material matHeart;
	
	public Material matHeartBlack;
	
	public float Percentage = 1.0f;
	
	Mesh mesh;	
	float currentP = 1.0f;
	
	static void UpdateMesh(Mesh mesh, float p)
	{
		mesh.vertices = new Vector3[] {
			new Vector3(0,0,0),
			new Vector3(1,0,0),
			new Vector3(1,p,0),
			new Vector3(0,p,0)
		};
		mesh.normals = new Vector3[] {
			new Vector3(0,0,-1),
			new Vector3(0,0,-1),
			new Vector3(0,0,-1),
			new Vector3(0,0,-1)
		};
		mesh.uv = new Vector2[] {
			new Vector2(0,0),
			new Vector2(1,0),
			new Vector2(1,p),
			new Vector2(0,p)
		};
		mesh.triangles = new int[] {
			0,1,2,
			0,2,3
		};
	}
	
	Material mat1, mat2;
	
	float showHeartAlpha = 0.0f;
	
	public void ShowHeart()
	{
		showHeartAlpha = 1.0f;
	}	
	
	void Start()
	{
		mesh = new Mesh();
		mesh.MarkDynamic();
		
		Transform t1 = this.transform;
		mat1 = (Material)Instantiate(matHeart);
		t1.gameObject.renderer.material = mat1;
		
		Transform t2 = this.transform.FindChild("backgr");
		mat2 = (Material)Instantiate(matHeartBlack);
		t2.gameObject.renderer.material = mat2;
		
		showHeartAlpha = 0.0f;
	}
	
	void Update()
	{
		float p = Mathf.Clamp01(Percentage);
		if(p != currentP) {
			ShowHeart();
			currentP = p;
			UpdateMesh(mesh, p);
			GetComponent<MeshFilter>().mesh = mesh;
		}
		showHeartAlpha -= MyTime.deltaTime * HEART_ALPHA_DECAY;
		mat1.SetColor("_TintColor", new Color(p, 0.0f, 0.0f, Mathf.Clamp01(showHeartAlpha)));
		mat2.SetColor("_TintColor", new Color(0.3f*p, 0.0f, 0.0f, Mathf.Clamp01(showHeartAlpha)));
	}
}
