using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class ColliderCompanion : MonoBehaviour
{
	public bool CompanionEnable = true;

	public enum ColliderMode
	{
		SingleTarget,
		MultipleTarget
	}
	[TabGroup("General")][EnumToggleButtons]
	public ColliderMode TargetMode;
	[TabGroup("General")][ShowIf("TargetMode", ColliderMode.SingleTarget)]
	public Transform TransformTarget;
	[TabGroup("General")][ShowIf("TargetMode", ColliderMode.MultipleTarget)]
	public LayerMask LayersTarget;

	[TabGroup("Events")]
	public UnityEvent OnEnterCollider;
	[TabGroup("Events")]
	public UnityEvent OnStayCollider;
	[TabGroup("Events")]
	public UnityEvent OnExitCollider;


	private void OnCollisionEnter(Collision collision)
	{
		if (TargetMode == ColliderMode.SingleTarget)
		{
			if (CompanionEnable && collision.gameObject.layer == TransformTarget.gameObject.layer)
				OnEnterCollider.Invoke();
		}
		else if (TargetMode == ColliderMode.MultipleTarget)
		{
			if (CompanionEnable && collision.gameObject.layer == LayermaskToLayer(LayersTarget))
				OnEnterCollider.Invoke();
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (TargetMode == ColliderMode.SingleTarget)
		{
			if (CompanionEnable && collision.gameObject.layer == TransformTarget.gameObject.layer)
				OnStayCollider.Invoke();
		}
		else if (TargetMode == ColliderMode.MultipleTarget)
		{
			if (CompanionEnable && collision.gameObject.layer == LayermaskToLayer(LayersTarget))
				OnStayCollider.Invoke();
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (TargetMode == ColliderMode.SingleTarget)
		{
			if (CompanionEnable && collision.gameObject.layer == TransformTarget.gameObject.layer)
				OnExitCollider.Invoke();
		}
		else if (TargetMode == ColliderMode.MultipleTarget)
		{
			if (CompanionEnable && collision.gameObject.layer == LayermaskToLayer(LayersTarget))
				OnExitCollider.Invoke();
		}
	}

	public static int LayermaskToLayer(LayerMask layerMask)
	{
		int layerNumber = 0;
		int layer = layerMask.value;
		while (layer > 0)
		{
			layer = layer >> 1;
			layerNumber++;
		}
		return layerNumber - 1;
	}
}
