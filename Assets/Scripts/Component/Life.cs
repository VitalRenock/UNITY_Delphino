using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;


public class Life : MonoBehaviour
{
	[TabGroup("Life")][ReadOnly]
	public float CurrentLife;
	[TabGroup("Life")]
	public float StartLife;
	[TabGroup("Life")]
	public float MinLife;
	[TabGroup("Life")]
	public float MaxLife;
	[TabGroup("Life")]
	public float NaturalGain;
	[TabGroup("Life")]
	public float NaturalLoss;
	[TabGroup("Options")]
	public bool InitiateLifeOnStart = true;
	[TabGroup("Events")]
	public OnModifiedLifeEvent OnModifiedLife = new OnModifiedLifeEvent();
	[TabGroup("Events")]
	public UnityEvent OnMaxLife = new UnityEvent();
	[TabGroup("Events")]
	public UnityEvent OnMinLife = new UnityEvent();


	private void Start()
	{
		if (InitiateLifeOnStart)
			LifeToStart();
	}

	public void LifeToStart()
	{
		CurrentLife = StartLife;
	}
	public void LifeToMax()
	{
		Debugger.I.DebugMessage("=> LifeToMax");

		CurrentLife = MaxLife;
		OnMaxLife.Invoke();
	}
	public void LifeToMin()
	{
		Debugger.I.DebugMessage("=> LifeToMin");

		CurrentLife = MinLife;
		OnMinLife.Invoke();
	}

	public void GainLife(float lifeToRegain)
	{
		Debugger.I.DebugMessage("=> GainLife");

		if (CurrentLife == MaxLife)
			return;

		CurrentLife += lifeToRegain;
		OnModifiedLife.Invoke(CurrentLife);

		if (CurrentLife >= MaxLife)
			LifeToMax();
	}
	public void LostLife(float lifeToLost)
	{
		Debugger.I.DebugMessage("=> LostLife");

		if (CurrentLife == MinLife)
		{
			Debugger.I.DebugWarning("Life of entity at 0.");
			return;
		}

		CurrentLife -= lifeToLost;
		OnModifiedLife.Invoke(CurrentLife);

		if (CurrentLife <= MinLife)
			LifeToMin();
	}

	public void NaturallyLifeRegain()
	{
		Debugger.I.DebugMessage("=> NaturallyLifeRegain");

		if (CurrentLife == MaxLife)
			return;

		GainLife(NaturalGain);
	}
	public void NaturallyLifeLost()
	{
		Debugger.I.DebugMessage("=> NaturallyLifeLost");

		if (CurrentLife == MinLife)
			return;

		LostLife(NaturalLoss);
	}
}

[System.Serializable]
public class OnModifiedLifeEvent : UnityEvent<float> { }