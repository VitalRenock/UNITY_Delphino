using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class TimeManager : Singleton<TimeManager>
{
	[TabGroup("Cycles")][ReadOnly]
	public float CurrentTime;
	[TabGroup("Cycles")]
	public bool RunCycle = false;
	[TabGroup("Cycles")]
	public List<CycleProps> Cycles;
	[TabGroup("Cycles")]
	public OnSwitchCycleEvent OnSwitchCycle = new OnSwitchCycleEvent();

	[TabGroup("Ticks")]
	public float MasterTickRate;
	[TabGroup("Ticks")]
	public UnityEvent onMasterTick = new UnityEvent();

	float _LastTick;

	private void Start()
	{
		_LastTick = Time.time;

		StartCoroutine(LoopCycle());
	}

	private void Update()
	{
		if (Time.time >= _LastTick + MasterTickRate)
		{
			onMasterTick.Invoke();
			_LastTick = Time.time;
		}
	}

	IEnumerator LoopCycle()
	{
		Debugger.I.DebugMessage("Start > LoopCycle");

		if (Cycles.Count <= 0)
		{
			Debugger.I.DebugError("No cycle in Cycles List!");
			yield break;
		}

		RunCycle = true;

		while (RunCycle)
		{
			for (int i = 0; i < Cycles.Count; i++)
			{
				yield return StartCoroutine(StartCycle(Cycles[i]));

				if (i == Cycles.Count)
					i = 0;
			}

			yield return null;
		}
	}

	IEnumerator StartCycle(CycleProps cycleProps)
	{
		Debugger.I.DebugMessage("Start > StartCycle");

		OnSwitchCycle.Invoke(cycleProps);

		CurrentTime = 0;
		cycleProps.OnBeginCycle.Invoke();

		while (CurrentTime < cycleProps.Duration)
		{
			CurrentTime += Time.deltaTime;
			yield return null;
		}

		cycleProps.OnEndingCycle.Invoke();
	}
}

[System.Serializable]
public class CycleProps
{
	[TabGroup("Properties")]
	public Identity Identity;
	[TabGroup("Properties")]
	public float Duration;
	[TabGroup("Events")]
	public UnityEvent OnBeginCycle = new UnityEvent();
	[TabGroup("Events")]
	public UnityEvent OnEndingCycle = new UnityEvent();
}

public class OnSwitchCycleEvent : UnityEvent<CycleProps> { };