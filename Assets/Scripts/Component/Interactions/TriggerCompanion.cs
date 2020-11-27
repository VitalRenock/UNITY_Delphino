using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Collider))]
public class TriggerCompanion : MonoBehaviour
{
	public bool CompanionEnable = true;

	public enum TriggerMode
	{
		SingleTarget,
		MultipleTarget
	}
	[TabGroup("General")]
	[EnumToggleButtons]
	public TriggerMode TargetMode;
	[TabGroup("General")]
	[ShowIf("TargetMode", TriggerMode.SingleTarget)]
	public Transform TransformTarget;
	[TabGroup("General")]
	[ShowIf("TargetMode", TriggerMode.MultipleTarget)]
	public LayerMask LayersTarget;

	[TabGroup("Events")]
	public UnityEvent OnEnterTrigger;
	[TabGroup("Events")]
	public UnityEvent OnStayTrigger;
	[TabGroup("Events")]
	public UnityEvent OnExitTrigger;


	private void OnTriggerEnter(Collider other)
	{
		if (TargetMode == TriggerMode.SingleTarget)
		{
			if (CompanionEnable && other.gameObject.layer == TransformTarget.gameObject.layer)
				OnEnterTrigger.Invoke();
		}
		else if (TargetMode == TriggerMode.MultipleTarget)
		{
			if (CompanionEnable && other.gameObject.layer == LayermaskToLayer(LayersTarget))
				OnEnterTrigger.Invoke();
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if (TargetMode == TriggerMode.SingleTarget)
		{
			if (CompanionEnable && other.gameObject.layer == TransformTarget.gameObject.layer)
				OnStayTrigger.Invoke();
		}
		else if (TargetMode == TriggerMode.MultipleTarget)
		{
			if (CompanionEnable && other.gameObject.layer == LayermaskToLayer(LayersTarget))
				OnStayTrigger.Invoke();
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (TargetMode == TriggerMode.SingleTarget)
		{
			if (CompanionEnable && other.gameObject.layer == TransformTarget.gameObject.layer)
				OnExitTrigger.Invoke();
		}
		else if (TargetMode == TriggerMode.MultipleTarget)
		{
			if (CompanionEnable && other.gameObject.layer == LayermaskToLayer(LayersTarget))
				OnExitTrigger.Invoke();
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