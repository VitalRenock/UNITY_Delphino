using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
	public GameData GameData;

	public Motor MotorPlayer;

	public float SpeedMovement = 1;
	public float SpeedRotation = 1;
	public float RunSpeedMultiplicator;
	float _Speed;

	public bool CanMove;
	public bool CanTurn;

	private void Start()
	{
		MotorPlayer.SpeedProperties = GameData.PlayerSpeedProperties;

		_Speed = SpeedMovement;

		CanMove = true;
		CanTurn = true;
	}

	private void Update()
	{
		if (CanMove && CanTurn)
		{
			SetCurrentSpeed();
			PlayerMove();
			PlayerRotate();
		}
	}

	void PlayerMove()
	{
		Vector3 inputsStrafe = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		if (inputsStrafe != Vector3.zero)
			MotorPlayer.SimpleMove(transform, inputsStrafe, _Speed);
	}

	void PlayerRotate()
	{
		Vector3 inputsRotation = new Vector3(0, Input.GetAxis("Mouse X"), 0);

		if (inputsRotation != Vector3.zero)
			MotorPlayer.SimpleRotation(transform, inputsRotation, SpeedRotation);
	}

	void SetCurrentSpeed()
	{
		if (Input.GetKey(KeyCode.LeftShift))
			_Speed = SpeedMovement * RunSpeedMultiplicator;
		else
		{
			_Speed = SpeedMovement;
		}
	}
}
