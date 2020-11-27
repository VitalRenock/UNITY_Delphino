using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentControl : MonoBehaviour
{
	[TabGroup("General")][ReadOnly]
	public NavAgentMode NavAgentMode;
	[TabGroup("General")][Range(0.1f, 1)]
	public float TimePauseBeforeLaunch;

	[TabGroup("Patrol")]
	public bool PatrolLoop = false;
	[TabGroup("Patrol")]
	public List<Transform> PatrolWaypoints;
	[TabGroup("Patrol")][ReadOnly]
	public int PatrolNextWaypointIndex = 0;
	[TabGroup("Patrol")][ReadOnly]
	public bool PatrolModeEnable = false;

	[TabGroup("Follow")]
	public Transform FollowTarget;
	[TabGroup("Follow")][ReadOnly]
	public bool FollowModeEnable = false;

	[TabGroup("Run Away")]
	public Transform RunAwayTarget;
	[TabGroup("Run Away")][ReadOnly]
	public bool RunAwayModeEnable = false;
	float TempStoppingDistance;

	[TabGroup("Events")]
	public UnityEvent OnGameStart;
	[TabGroup("Events")][Title("Any mode")]
	public UnityEvent OnStartMode;
	[TabGroup("Events")]
	public UnityEvent OnStopMode;
	[TabGroup("Events")][Title("Patrol mode")]
	public UnityEvent OnStartPatrol;
	[TabGroup("Events")]
	public UnityEvent OnStopPatrol;
	[TabGroup("Events")][Title("Follow mode")]
	public UnityEvent OnStartFollow;
	[TabGroup("Events")]
	public UnityEvent OnStopFollow;
	[TabGroup("Events")][Title("Run Away mode")]
	public UnityEvent OnStartRunAway;
	[TabGroup("Events")]
	public UnityEvent OnStopRunAway;

	[TabGroup("Debug")][ShowInInspector][ReadOnly]
	NavMeshAgent _Agent;
	Coroutine _ModeInWorking;


	#region Debug NavMesh
	[TabGroup("Debug")]
	public bool DebugNavMeshAgent = false;

	[TabGroup("Debug")]
	public Vector3 destination;
	[TabGroup("Debug")]
	public Vector3 nextPosition;
	[TabGroup("Debug")]
	public float remainingDistance;
	[TabGroup("Debug")]
	public float stoppingDistance;

	[TabGroup("Debug")]
	public float speed;
	[TabGroup("Debug")]
	public float angularSpeed;
	[TabGroup("Debug")]
	public float acceleration;
	[TabGroup("Debug")]
	public Vector3 desiredVelocity;
	[TabGroup("Debug")]
	public Vector3 velocity;
	[TabGroup("Debug")]
	public Vector3 steeringTarget;

	[TabGroup("Debug")]
	public bool hasPath;
	[TabGroup("Debug")]
	public bool isPathStale;
	[TabGroup("Debug")]
	public bool pathPending;
	[TabGroup("Debug")]
	public Vector3 pathEndPosition;

	[TabGroup("Debug")]
	public float baseOffset;

	[TabGroup("Debug")]
	public bool autoBraking;
	[TabGroup("Debug")]
	public bool autoRepath;
	[TabGroup("Debug")]
	public bool isOnNavMesh;
	[TabGroup("Debug")]
	public bool isOnOffMeshLink;
	[TabGroup("Debug")]
	public bool isStopped;
	#endregion

	private void Awake()
	{
		_Agent = GetComponent<NavMeshAgent>();
	}

	//private void Update()
	//{
	//	if (Input.GetKeyDown(KeyCode.Keypad0))
	//		StopMode();
	//	if (Input.GetKeyDown(KeyCode.Keypad1))
	//		StartMode(NavAgentMode.Patrol);
	//	if (Input.GetKeyDown(KeyCode.Keypad2))
	//		StartMode(NavAgentMode.Follow);
	//	if (Input.GetKeyDown(KeyCode.Keypad3))
	//		StartMode(NavAgentMode.RunAway);

	//	if (DebugNavMeshAgent)
	//	{
	//		destination = _Agent.destination;
	//		nextPosition = _Agent.nextPosition;
	//		remainingDistance = _Agent.remainingDistance;
	//		stoppingDistance =  _Agent.stoppingDistance;

	//		speed =_Agent.speed;
	//		angularSpeed = _Agent.angularSpeed;
	//		acceleration = _Agent.acceleration;
	//		desiredVelocity = _Agent.desiredVelocity;
	//		velocity = _Agent.velocity;
	//		steeringTarget = _Agent.steeringTarget;

	//		hasPath = _Agent.hasPath;
	//		isPathStale =  _Agent.isPathStale;
	//		pathPending = _Agent.pathPending;
	//		pathEndPosition = _Agent.pathEndPosition;

	//		baseOffset = _Agent.baseOffset;

	//		autoBraking = _Agent.autoBraking;
	//		autoRepath = _Agent.autoRepath;
	//		isOnNavMesh = _Agent.isOnNavMesh;
	//		isOnOffMeshLink = _Agent.isOnOffMeshLink;
	//		isStopped = _Agent.isStopped;

	//		//_Agent.gameObject;
	//		//_Agent.transform;
	//		//_Agent.name;
	//		//_Agent.tag;
	//		//_Agent.agentTypeID;
	//		//_Agent.hideFlags;
	//		//_Agent.enabled;
	//		//_Agent.isActiveAndEnabled;

	//		//_Agent.updatePosition;
	//		//_Agent.updateRotation;
	//		//_Agent.updateUpAxis;

	//		//_Agent.height;
	//		//_Agent.radius;

	//		//_Agent.areaMask;
	//		//_Agent.walkableMask;
	//		//_Agent.avoidancePriority;
	//		//_Agent.obstacleAvoidanceType;

	//		//_Agent.navMeshOwner;

	//		//_Agent.autoTraverseOffMeshLink;
	//		//_Agent.currentOffMeshLinkData;
	//		//_Agent.nextOffMeshLinkData;

	//		//_Agent.path;
	//		//_Agent.pathStatus;
	//	}
	//}


	public void StartPatrol()
	{
		StartMode(NavAgentMode.Patrol);
	}
	public void StartFollow()
	{
		StartMode(NavAgentMode.Follow);
	}
	public void StartRunAway()
	{
		StartMode(NavAgentMode.RunAway);
	}


	public void StartMode(NavAgentMode navAgentMode)
	{
		Debugger.I.DebugMessage("=> StartMode()");

		if (NavAgentMode == navAgentMode)
			return;

		#region Stop current mode

		switch (NavAgentMode)
		{
			case NavAgentMode.DoNothing:
				break;

			case NavAgentMode.Patrol:
				PatrolModeEnable = false;
				break;

			case NavAgentMode.Follow:
				FollowModeEnable = false;
				break;

			case NavAgentMode.RunAway:
				RunAwayModeEnable = false;
				_Agent.stoppingDistance = TempStoppingDistance;
				break;

			default:
				break;
		}

		if (_ModeInWorking != null)
		{
			NavAgentMode = NavAgentMode.DoNothing;

			StopCoroutine(_ModeInWorking);
			_ModeInWorking = null;
		}

		#endregion

		#region Start next mode

		switch (navAgentMode)
		{
			case NavAgentMode.DoNothing:
				break;

			case NavAgentMode.Patrol:
				NavAgentMode = NavAgentMode.Patrol;

				if (PatrolNextWaypointIndex >= PatrolWaypoints.Count)
					PatrolNextWaypointIndex = 0;

				_ModeInWorking = StartCoroutine(ModePatrol());
				PatrolModeEnable = true;

				// For Debug
				transform.GetComponent<MeshRenderer>().material.color = Color.cyan;
				break;

			case NavAgentMode.Follow:
				NavAgentMode = NavAgentMode.Follow;

				_ModeInWorking = StartCoroutine(ModeFollow());
				FollowModeEnable = true;

				// For Debug
				transform.GetComponent<MeshRenderer>().material.color = Color.red;
				break;

			case NavAgentMode.RunAway:
				NavAgentMode = NavAgentMode.RunAway;

				TempStoppingDistance = _Agent.stoppingDistance;

				_ModeInWorking = StartCoroutine(ModeRunAway());
				RunAwayModeEnable = true;

				// For Debug
				transform.GetComponent<MeshRenderer>().material.color = Color.yellow;
				break;

			default:
				break;
		}

		#endregion
	}
	public void StopMode()
	{
		Debugger.I.DebugMessage("=> StopMode()");

		#region Stop current mode

		switch (NavAgentMode)
		{
			case NavAgentMode.DoNothing:
				break;

			case NavAgentMode.Patrol:
				PatrolModeEnable = false;
				break;

			case NavAgentMode.Follow:
				FollowModeEnable = false;
				break;

			case NavAgentMode.RunAway:
				RunAwayModeEnable = false;
				_Agent.stoppingDistance = TempStoppingDistance;
				break;

			default:
				break;
		}

		if (_ModeInWorking != null)
		{
			NavAgentMode = NavAgentMode.DoNothing;

			StopCoroutine(_ModeInWorking);
			_ModeInWorking = null;

			_Agent.ResetPath();

			// For Debug
			transform.GetComponent<MeshRenderer>().material.color = Color.white;
		}

		#endregion
	}

	IEnumerator ModePatrol()
	{
		Debugger.I.DebugMessage("=> Start ModeGoto()");

		do
		{
			_Agent.SetDestination(PatrolWaypoints[PatrolNextWaypointIndex].position);
			yield return new WaitForSeconds(TimePauseBeforeLaunch);

			while (_Agent.remainingDistance > _Agent.stoppingDistance)
				yield return null;

			PatrolNextWaypointIndex++;

			if (PatrolNextWaypointIndex == PatrolWaypoints.Count && PatrolLoop)
				PatrolNextWaypointIndex = 0;
		}
		while (PatrolNextWaypointIndex < PatrolWaypoints.Count);

		Debug.Log("=> End ModeGoto()");
	}
	IEnumerator ModeFollow()
	{
		Debugger.I.DebugMessage("=> Start ModeFollow()");

		yield return new WaitForSeconds(TimePauseBeforeLaunch);

		while (FollowModeEnable)
		{
			if (Vector3.Distance(transform.position, FollowTarget.position) > _Agent.stoppingDistance)
				_Agent.SetDestination(FollowTarget.position);

			yield return null;
		}

		Debug.Log("=> End ModeFollow()");
	}
	IEnumerator ModeRunAway()
	{
		Debugger.I.DebugMessage("=> Start ModeRunAway()");

		yield return new WaitForSeconds(TimePauseBeforeLaunch);

		_Agent.ResetPath();
		_Agent.stoppingDistance = 0;

		while (RunAwayModeEnable)
		{
			if (Vector3.Distance(transform.position, RunAwayTarget.position) <= TempStoppingDistance && !_Agent.hasPath)
			{
				Vector3 agentDestination = transform.position - ((RunAwayTarget.position - transform.position).normalized * 20);
				_Agent.SetDestination(agentDestination);
			}

			yield return null;
		}

		Debug.Log("=> End ModeRunAway()");
	}
}

[SerializeField]
public enum NavAgentMode
{
	DoNothing,
	Patrol,
	Follow,
	RunAway
}