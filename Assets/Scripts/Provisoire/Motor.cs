using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable]
public class Motor
{
	public VelocityProperties SpeedProperties;

	public void SimpleMove(Transform transform, Vector3 direction, float speed)
	{
		transform.Translate(direction * speed * Time.deltaTime, Space.Self);
	}

	public void SimpleRotation(Transform transform, Vector3 direction, float angSpeed)
	{
		transform.Rotate(direction * angSpeed * Time.deltaTime, Space.Self);
	}

	public void VelocityMove(Transform transform)
	{
		transform.Translate(SpeedProperties.CurrentLinVelocity * Time.deltaTime);
	}

	public void VelocityRotation(Transform transform)
	{
		transform.Rotate(SpeedProperties.CurrentAngVelocity * Time.deltaTime);
	}

	public void LinearlyAccelerating(Vector3 direction)
	{
		if (SpeedProperties.CurrentLinVelocity == new Vector3(SpeedProperties.MaxLinVelocity, SpeedProperties.MaxLinVelocity, SpeedProperties.MaxLinVelocity))
			return;

		SpeedProperties.CurrentLinVelocity = Vector3.ClampMagnitude(SpeedProperties.CurrentLinVelocity + (direction * SpeedProperties.LinAccelerationPower), SpeedProperties.MaxLinVelocity);
	}

	public void LinearlyDecelerating()
	{
		if (SpeedProperties.CurrentLinVelocity == Vector3.zero)
			return;

		SpeedProperties.CurrentLinVelocity += Vector3.Lerp(-SpeedProperties.CurrentLinVelocity, Vector3.zero, 1 - SpeedProperties.LinDecelerationPower);

		if (SpeedProperties.CurrentLinVelocity.x < SpeedProperties.MinLinVelocityToStop && SpeedProperties.CurrentLinVelocity.x > -SpeedProperties.MinLinVelocityToStop)
			SpeedProperties.CurrentLinVelocity.x = 0;
		if (SpeedProperties.CurrentLinVelocity.y < SpeedProperties.MinLinVelocityToStop && SpeedProperties.CurrentLinVelocity.y > -SpeedProperties.MinLinVelocityToStop)
			SpeedProperties.CurrentLinVelocity.y = 0;
		if (SpeedProperties.CurrentLinVelocity.z < SpeedProperties.MinLinVelocityToStop && SpeedProperties.CurrentLinVelocity.z > -SpeedProperties.MinLinVelocityToStop)
			SpeedProperties.CurrentLinVelocity.z = 0;
	}

	public void AngularlyAccelerating(Vector3 direction)
	{
		if (SpeedProperties.CurrentAngVelocity == new Vector3(SpeedProperties.MaxAngVelocity, SpeedProperties.MaxAngVelocity, SpeedProperties.MaxAngVelocity))
			return;

		SpeedProperties.CurrentAngVelocity = Vector3.ClampMagnitude(SpeedProperties.CurrentAngVelocity + (direction * SpeedProperties.AngAccelerationPower), SpeedProperties.MaxAngVelocity);
	}

	public void AngularlyDecelerating()
	{
		if (SpeedProperties.CurrentAngVelocity == Vector3.zero)
			return;

		SpeedProperties.CurrentAngVelocity += Vector3.Lerp(-SpeedProperties.CurrentAngVelocity, Vector3.zero, 1 - SpeedProperties.AngDecelerationPower);

		if (SpeedProperties.CurrentAngVelocity.x < SpeedProperties.MinAngVelocityToStop && SpeedProperties.CurrentAngVelocity.x > -SpeedProperties.MinAngVelocityToStop)
			SpeedProperties.CurrentAngVelocity.x = 0;
		if (SpeedProperties.CurrentAngVelocity.y < SpeedProperties.MinAngVelocityToStop && SpeedProperties.CurrentAngVelocity.y > -SpeedProperties.MinAngVelocityToStop)
			SpeedProperties.CurrentAngVelocity.y = 0;
		if (SpeedProperties.CurrentAngVelocity.z < SpeedProperties.MinAngVelocityToStop && SpeedProperties.CurrentAngVelocity.z > -SpeedProperties.MinAngVelocityToStop)
			SpeedProperties.CurrentAngVelocity.z = 0;
	}
}

[System.Serializable]
public class VelocityProperties
{
	[TabGroup("Linear")]
	[Title("Velocity")]
	[Range(0.001f, 1f)]
	public float MinLinVelocityToStop;
	[TabGroup("Linear")]
	public float MaxLinVelocity;
	[TabGroup("Linear")]
	[ReadOnly]
	public Vector3 CurrentLinVelocity;
	[TabGroup("Linear")]
	[Title("Acceleration")]
	public float LinAccelerationPower;
	[TabGroup("Linear")]
	[Range(0f, 1f)]
	public float LinDecelerationPower;

	[TabGroup("Angular")]
	[Title("Velocity")]
	[Range(0.001f, 1f)]
	public float MinAngVelocityToStop;
	[TabGroup("Angular")]
	public float MaxAngVelocity;
	[TabGroup("Angular")]
	[ReadOnly]
	public Vector3 CurrentAngVelocity;
	[TabGroup("Angular")]
	[Title("Acceleration")]
	public float AngAccelerationPower;
	[TabGroup("Angular")]
	[Range(0f, 1f)]
	public float AngDecelerationPower;
}