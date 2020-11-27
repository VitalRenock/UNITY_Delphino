using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkyboxManager : Singleton<SkyboxManager>
{
	[TabGroup("Skyboxes")][ReadOnly]
	public SkyboxProps CurrentSkybox = new SkyboxProps();
	[TabGroup("Skyboxes")]
	public List<SkyboxProps> SkyboxesList;

	CycleProps _CurrentCycle = new CycleProps();
	//Light _CurrentLight;

	private void Start()
	{
		CurrentSkybox = SkyboxesList[0];

		TimeManager.I.OnSwitchCycle.AddListener(SetSkybox);
		SetSkybox(TimeManager.I.Cycles[0]);
	}

	private void Update()
	{
		UpdateSkybox();
	}

	public void SetSkybox(CycleProps cycleProps)
	{
		Debugger.I.DebugMessage("Dans " + cycleProps.Identity.Name);

		_CurrentCycle = cycleProps;

		CurrentSkybox.Light.gameObject.SetActive(false);

		// Set Material
		if (cycleProps.Identity.Name == "Jour")
			CurrentSkybox = SkyboxesList[0];
		else if (cycleProps.Identity.Name == "Nuit")
			CurrentSkybox = SkyboxesList[1];

		CurrentSkybox.Light.gameObject.SetActive(true);

		// Set RenderSettings
		RenderSettings.skybox = CurrentSkybox.Material;
		RenderSettings.sun = CurrentSkybox.Light;
		//_CurrentLight = CurrentSkybox.Light;
		//RenderSettings.sun = _CurrentLight;
	}

	public void UpdateSkybox()
	{
		float lightRotation = (TimeManager.I.CurrentTime / _CurrentCycle.Duration) * 180;

		RenderSettings.sun.transform.eulerAngles = new Vector3(lightRotation, 0, 0);
		//_CurrentLight.transform.eulerAngles = new Vector3(lightRotation, 0, 0);
		//CurrentSkybox.Light.transform.Rotate(new Vector3(lightRotation, 0, 0) * Time.deltaTime);
	}
}

[System.Serializable]
public class SkyboxProps
{
	public Identity Identity;
	public Material Material;
	public Light Light;
}
