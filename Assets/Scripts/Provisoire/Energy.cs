using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;


[System.Serializable]
public class Energy
{
	[TabGroup("Properties")]
	public float Generated;
	[TabGroup("Properties")]
	public float Consumpted;
	[TabGroup("Properties")]
	public float Provided;
	[TabGroup("Properties")]
	public float MaxStock;
	[TabGroup("Properties")][ReadOnly]
	public float Stock = 0;
	[TabGroup("States")]
	public bool IsOn = false;
	[TabGroup("States")]
	public bool IsOverConsumption = false;
	[TabGroup("States")]
	public Color ColorStart;
	[TabGroup("States")]
	public Color ColorStop;
	[TabGroup("Events")]
	public UnityEvent onCycle = new UnityEvent();
	[TabGroup("Events")]
	public UnityEvent onOverConsumption = new UnityEvent();


	public void Cycle()
	{
		Debug.Log("=> Cycle");

		if (!IsOn)
			return;

		if (IsOverConsumption)
		{
			IsOn = false;
			return;
		}

		Generation();
		onCycle.Invoke();
		Consumption();
	}

	void Generation()
	{
		Debug.Log("=> Generation");

		Stock += Generated;
		if (Stock > MaxStock)
			Stock = MaxStock;
	}

	void Consumption()
	{
		Debug.Log("=> Consumption");

		Stock -= Consumpted + Provided;
		if (Stock < 0)
		{
			Stock = 0;
			OverConsumption();
		}
	}

	void OverConsumption()
	{
		Debug.Log("=> OverConsumption");

		onOverConsumption.Invoke();
		IsOverConsumption = true;
		IsOn = false;
	}

	[Button("Reset")]
	public void Reset()
	{
		Debug.Log("=> Reset");

		IsOverConsumption = false;
	}
}
