using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	#region Variables
	private static T _I = null;
	public static T I
	{
		get
		{
			//if (_I == null)
			//    Debug.Assert(false, "No Instance of " + typeof(T));

			return _I;
		}
	}
	#endregion

	#region Unity functions

	private void Awake()
	{
		if (_I != this && _I != null)
		{
			Debug.Log(gameObject.name);
			DestroyImmediate(gameObject);
			return;
		}
		_I = this as T;
	}

	#endregion
}