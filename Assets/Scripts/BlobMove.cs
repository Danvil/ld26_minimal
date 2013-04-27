using UnityEngine;
using System.Linq;

public class BlobMove : MonoBehaviour {

	const float cSpeedMaxBoost = 1.2f;
	const float cAvoidStrengthOther = 0.5f;
	const float cAvoidStrengthLevel = 0.5f;
	const float cRotationMixStrength = 0.5f;
	
	public float size = 0.3f;
	public float speed = 1.5f;
	public float playerFollowStrength = 0.0f;
	public bool isPlayerFollow = false;
	public float goalTolerance = 0.5f;

	public bool hasGoal { get; private set; }
	public Vector3 goal { get; private set; }
	public float goalTime { get; private set; }
	
	public void SetGoal(Vector3 goal)
	{
		this.goal = goal;
		this.goalTime = MyTime.time;
		this.hasGoal = true;
	}

	bool isGoalReached() {
		return (transform.position - goal).magnitude < goalTolerance;
	}

	// Use this for initialization
	void Start () {
		hasGoal = false;
	}

	float slerpAngle(float x, float y, float p) {
		float d = y - x;
		if(d > Mathf.PI) {
			d = 2.0f * Mathf.PI - d;
		}
		return x + p * d;
	}

	Vector3 computePlayerFollow() {
		const float cMinRadius = 1.0f;
		Vector3 d = Globals.Player.transform.position - transform.position;
		float m = d.magnitude;
		if(m == 0) {
			return Vector3.zero; // FIXME
		}
		if(playerFollowStrength >= 0) {
			if(m < cMinRadius) {
				return Vector3.zero;
			}
			else {
				return playerFollowStrength * d.normalized;
			}
		}
		else {
			m = 0.5f + 0.4f*Mathf.Max(0.0f, m - 3.0f);
			return playerFollowStrength / (m*m) * d.normalized;
		}
	}

	Vector3 computeGoalFollow() {
		if(hasGoal) {
			return (goal - transform.position).normalized;
		}
		else {
			return Vector3.zero;
		}
	}

	float avoidFalloff(float d, float d_min) {
		float z = Mathf.Max(d/d_min, 0.4f);
		return 1.0f / (z*z);
	}

	// avoid other
	Vector3 computeAvoidOther() {
		Vector3 force = Vector3.zero;
		foreach(BlobMove x in Globals.BlobManager.GetMoveBehaviours()) { // FIXME reduce range
			Vector3 delta = x.transform.position - transform.position;
			float d_min = this.size + x.size;
			force -= avoidFalloff(delta.magnitude, 0.5f*d_min) * delta.normalized;
		}
		return force;
	}

	// avoid level
	Vector3 computeAvoidLevel() {
		float size_this = this.size;
		Vector3 force = Vector3.zero;
		float x = this.transform.position.x;
		float y = this.transform.position.y;
		// FIXME
//		force += avoidFalloff(x - Globals.LevelRect.xMin, size_this) * new Vector3(1,0,0);
//		force += avoidFalloff(Globals.LevelRect.xMax - x, size_this) * new Vector3(-1,0,0);
//		force += avoidFalloff(y - Globals.LevelRect.yMin, size_this) * new Vector3(0,1,0);
//		force += avoidFalloff(Globals.LevelRect.yMax - y, size_this) * new Vector3(0,-1,0);
/*		Debug.DrawLine(new Vector3(Globals.LevelRect.xMin, Globals.LevelRect.yMin, -1.0f),
					   new Vector3(Globals.LevelRect.xMax, Globals.LevelRect.yMin, -1.0f));
		Debug.DrawLine(new Vector3(Globals.LevelRect.xMin, Globals.LevelRect.yMax, -1.0f),
					   new Vector3(Globals.LevelRect.xMax, Globals.LevelRect.yMax, -1.0f));
		Debug.DrawLine(new Vector3(Globals.LevelRect.xMin, Globals.LevelRect.yMin, -1.0f),
					   new Vector3(Globals.LevelRect.xMin, Globals.LevelRect.yMax, -1.0f));
		Debug.DrawLine(new Vector3(Globals.LevelRect.xMax, Globals.LevelRect.yMin, -1.0f),
					   new Vector3(Globals.LevelRect.xMax, Globals.LevelRect.yMax, -1.0f));
*/		return force;
	}
	
	// Update is called once per frame
	void Update () {
		if(isGoalReached()) {
			hasGoal = false;
		}
		Vector3 moveFollow = speed * computeGoalFollow().normalized;
		Vector3 moveAvoid = cAvoidStrengthOther * computeAvoidOther();
		Vector3 moveLevel = cAvoidStrengthLevel * computeAvoidLevel();
		Vector3 move = moveFollow + moveAvoid + moveLevel;
		Debug.DrawRay(this.transform.position, moveFollow, Color.red);
		Debug.DrawRay(this.transform.position, moveAvoid, Color.green);
		Debug.DrawRay(this.transform.position, moveLevel, Color.blue);
		// some randomness
		move += MyTime.deltaTime * 0.05f * MoreMath.RandomInsideUnitCircle3;
		move.WithChangedZ(0.0f);
		// limit max velocity
		float mag = move.magnitude;
		float speedMax = cSpeedMaxBoost * speed;
		if(mag > speedMax) {
			move *= speedMax / mag;
		}
		// compute new position
		transform.position += MyTime.deltaTime * move;
		// compute new rotation
		float angle_old = MoreMath.VectorAngle(transform.localRotation * Vector3.right);
		float angle_new = MoreMath.VectorAngle(move.normalized);
		float angle_final = slerpAngle(angle_old, angle_new, cRotationMixStrength * MyTime.deltaTime);
		transform.localRotation = MoreMath.RotAngle(angle_final);
	}
	
	void OnDrawGizmos()
	{
		if(hasGoal) {
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, goal);
		}
	}
	
}
