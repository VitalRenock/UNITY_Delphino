using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlayerMotor : MonoBehaviour
{
	public GameData GameData;

	public Motor MotorCam;

	public float SpeedRotation = 1;

	public bool CanTurn;

	private void Start()
	{
		MotorCam.SpeedProperties = GameData.PlayerHeadSpeedProperties;

		CanTurn = true;
	}

	private void Update()
	{
		if (CanTurn)
			PlayerRotate();
	}

	void PlayerRotate()
	{
		Vector3 inputsRotation = new Vector3(-Input.GetAxis("Mouse Y"), 0, 0);

		if (inputsRotation != Vector3.zero)
			MotorCam.SimpleRotation(transform, inputsRotation, SpeedRotation);
	}
}
