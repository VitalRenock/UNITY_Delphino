using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMotor : MonoBehaviour
{
	public GameData GameData;

	public Motor MotorShip;
	public bool CanMove;
	public bool CanTurn;

	private void Start()
	{
		MotorShip.SpeedProperties = GameData.PlayerSpeedProperties;

		CanMove = true;
		CanTurn = true;
	}

	private void Update()
	{
		if(CanMove && CanTurn)
		{
			ShipRotate();
			ShipMove();
		}
	}

	void ShipMove()
	{
		Vector3 inputsStrafe = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		if (inputsStrafe != Vector3.zero)
			MotorShip.LinearlyAccelerating(inputsStrafe);
		else
			MotorShip.LinearlyDecelerating();

		MotorShip.VelocityMove(transform);
	}

	void ShipRotate()
	{
		Vector3 inputsRotation = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

		if (inputsRotation != Vector3.zero)
			MotorShip.AngularlyAccelerating(inputsRotation);
		else
			MotorShip.AngularlyDecelerating();

		MotorShip.VelocityRotation(transform);
	}
}
