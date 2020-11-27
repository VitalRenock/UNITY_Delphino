using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class Enemy : MonoBehaviour
{
	[FoldoutGroup("Attack")]
	public Attack Attack;
	[FoldoutGroup("Attack")]
	public AttackProps AttackProps;
	[FoldoutGroup("Attack")]
	public Transform Target;


	public void StartAttack()
	{
		Debug.Log("Coucou");
		TimeManager.I.onMasterTick.AddListener(AttackTarget);
	}
	public void StopAttack()
	{
		Debug.Log("Au revoir");
		TimeManager.I.onMasterTick.RemoveListener(AttackTarget);
	}
	void AttackTarget()
	{
		//if (Vector3.Distance(transform.position, Target.position) <= AttackProps.DistanceAttack)
		//	Target.GetComponent<Entity>().Life.LostLife(AttackProps.Damage);
	}
}
