using UnityEngine;
using System.Linq;

public class BlobMove : MonoBehaviour {

	const float cSpeedMaxBoost = 1.2f;
	const float cAvoidStrengthPlayer = 1.0f;
	const float cAvoidStrengthOther = 0.5f;
	const float cAvoidStrengthLevel = 1.0f;
	const float cAvoidStrengthBombs = 2.5f;
	const float cBombAvoidRadius = 1.0f;
	const float cRotationMixStrength = 0.5f;
	
	public float size = 0.3f;
	public float speed = 1.5f;
	public float playerFollowStrength = 0.0f;
	public bool isPlayerFollow = false;
	public float goalTolerance = 0.5f;

	public bool hasGoal { get; private set; }
	public Vector3 goal { get; private set; }
	
	public float PlayerMinDistance = 0.0f;
	
	public void SetGoal(Vector3 goal)
	{
		this.goal = goal;
		this.hasGoal = true;
	}
	
	public void DisableGoal()
	{
		this.hasGoal = false;
	}

	public bool isGoalReached() {
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

	// avoid player
	Vector3 computeAvoidPlayer() {
		Vector3 player_pos = Globals.Player.transform.position;
		Vector3 delta = player_pos - transform.position;
		float dist = delta.magnitude;
		if(dist < PlayerMinDistance) {
			return - avoidFalloff(dist, PlayerMinDistance) * delta.normalized;
		}
		else {
			return Vector3.zero;
		}
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

	// avoid bombs
	Vector3 computeAvoidBombs() {
		Vector3 force = Vector3.zero;
		foreach(Bomb x in Globals.BombManager.GetBombs()) { // FIXME reduce range
			Vector3 delta = x.transform.position - transform.position;
			float d_min = this.size + cBombAvoidRadius;
			force -= avoidFalloff(delta.magnitude, d_min) * delta.normalized;
		}
		return force;
	}

	// avoid level
	Vector3 computeAvoidLevel() {
		Vector3 force = Vector3.zero;
		Vector3 pos = this.transform.position.WithChangedZ(0.0f);
		foreach(Vector3 bc in Globals.Level.BlockedCells) {
			Vector3 delta = pos - bc;
			force += avoidFalloff(delta.magnitude, this.size) * delta.normalized;
		}
		return force;
	}
	
	Vector3 moveFollow;
	Vector3 movePlayer;
	Vector3 moveAvoid;
	Vector3 moveLevel;
	Vector3 moveBombs;

	// Update is called once per frame
	void Update () {
		if(isGoalReached()) {
			hasGoal = false;
		}
		moveFollow = speed * computeGoalFollow().normalized;
		movePlayer = cAvoidStrengthPlayer * computeAvoidPlayer();
		moveAvoid = cAvoidStrengthOther * computeAvoidOther();
		moveLevel = cAvoidStrengthLevel * computeAvoidLevel();
		moveBombs = cAvoidStrengthBombs * computeAvoidBombs();
		Vector3 move = moveFollow + movePlayer + moveAvoid + moveLevel + moveBombs;
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
			Debug.DrawRay(this.transform.position, movePlayer, new Color(1.0f, 0.5f, 0.0f));
			Debug.DrawRay(this.transform.position, moveFollow, Color.blue);
			Debug.DrawRay(this.transform.position, moveAvoid, Color.green);
			Debug.DrawRay(this.transform.position, moveLevel, Color.cyan);
			Debug.DrawRay(this.transform.position, moveBombs, Color.red);
		}
	}
	
}
