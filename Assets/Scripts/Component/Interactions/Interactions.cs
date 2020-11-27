using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactions : MonoBehaviour
{
	public Camera MainCamera;
	public LayerMask InteractionsLayers;
	public float MaxDistance = 0;
	public List<ActionSpecified> SpecifiedActions;

	public void RayCastCamForward()
	{
		Debugger.I.DebugMessage("=> RayCastCamForward");
		Debug.DrawRay(MainCamera.transform.position, MainCamera.transform.forward  * MaxDistance, Color.yellow, 2f);

		// Tir sur les layers auttorisé...
		// Pour chaque actions spécifiées, on invoque l'event correspondant.
		RaycastHit hitInfo;
		if (Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hitInfo, MaxDistance, InteractionsLayers))
			for (int i = 0; i < SpecifiedActions.Count; i++)
				if (LayermaskToLayer(SpecifiedActions[i].InteractLayers) == hitInfo.transform.gameObject.layer)
					SpecifiedActions[i].onInteraction.Invoke(hitInfo);
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

[System.Serializable]
public class ActionSpecified
{
	public LayerMask InteractLayers;
	public OnInteractionEvent onInteraction;
}

[System.Serializable]
public class OnInteractionEvent : UnityEvent<RaycastHit> { };