using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;


[System.Serializable]
public class Instantiation
{
	[TabGroup("Asset Refs")]
	public Mesh Mesh;
	[TabGroup("Asset Refs")]
	public Material Material;
	//[TabGroup("Options")]
	//public bool InstantiateOnEditor = false;
	[TabGroup("Options")]
	public bool InstantiateOnStart = false;
	[TabGroup("Options")]
	public Vector3 InstantiateSize = Vector3.one;
	[TabGroup("Options")][Space]
	public ColliderType ColliderType;
	[TabGroup("Events")]
	public UnityEvent OnInstantiation = new UnityEvent();

	public void SetInstantiate(GameObject gameObject)
	{
		Debugger.I.DebugMessage("=> SetInstantiate");

		#region Set MeshFilter

		MeshFilter meshFilter;
		if (gameObject.TryGetComponent(out meshFilter))
			meshFilter.mesh = Mesh;
		else
		{
			meshFilter = gameObject.AddComponent<MeshFilter>();
			meshFilter.mesh = Mesh;
		}

		#endregion

		#region Set MeshRenderer

		MeshRenderer meshRenderer;
		if (gameObject.TryGetComponent(out meshRenderer))
			meshRenderer.material = Material;
		else
		{
			meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.material = Material;
		}

		#endregion

		#region Set GameObject

		gameObject.transform.localScale = InstantiateSize;
		//gameObject.transform.position += new Vector3(0, gameObject.transform.localScale.y * 0.5f, 0);

		#endregion

		#region Set Collider

		Collider collider;
		if (gameObject.TryGetComponent(out collider)) { }
		else
		{
			switch (ColliderType)
			{
				case ColliderType.Box:
					gameObject.AddComponent<BoxCollider>();
					break;
				case ColliderType.Sphere:
					gameObject.AddComponent<SphereCollider>();
					break;
				case ColliderType.Capsule:
					gameObject.AddComponent<CapsuleCollider>();
					break;
				case ColliderType.Mesh:
					gameObject.AddComponent<MeshCollider>();
					break;
				default:
					break;
			}
		}

		#endregion
	}
}

public enum ColliderType
{
	Box,
	Sphere,
	Capsule,
	Mesh
}
