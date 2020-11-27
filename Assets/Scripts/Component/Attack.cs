using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Attack : MonoBehaviour
{
	public AttackProps AttackProps;

	public void AttackLife(RaycastHit hit)
	{
		Debugger.I.DebugMessage("=> AttackTarget");

		if (!AttackProps.CanAttack)
			return;

		Life life = hit.transform.GetComponent<Life>();
		life.LostLife(AttackProps.Damage);

		AttackProps.OnStartAttack.Invoke();
	}
	public void AttackLife(Transform target)
	{
		//Debugger.I.DebugMessage("=> AttackTarget");

		if (!AttackProps.CanAttack)
		{
			//Debugger.I.DebugWarning("CanAttack is false.");
			return;
		}
		if (Vector3.Distance(transform.position, target.position) > AttackProps.MaxDistance)
		{
			//Debugger.I.DebugWarning("Target is too far.");
			return;
		}
		if (Time.time < AttackProps.LastTimeOfAttack + AttackProps.Cooldown)
		{
			//Debugger.I.DebugWarning("Attack cooldown is not finish.");
			return;
		}

		Life life = target.transform.GetComponent<Life>();
		life.LostLife(AttackProps.Damage);

		AttackProps.LastTimeOfAttack = Time.time;
		AttackProps.OnStartAttack.Invoke();
	}
}

[System.Serializable]
public class AttackProps
{
	[TabGroup("Attack")]
	public bool CanAttack;
	[TabGroup("Attack")]
	public float Damage = 0;
	[TabGroup("Attack")]
	public float MaxDistance = 0;
	[TabGroup("Attack")]
	public float Cooldown = 0;
	[TabGroup("Events")]
	public UnityEvent OnStartAttack = new UnityEvent();

	[ReadOnly]
	public float LastTimeOfAttack = 0;
}