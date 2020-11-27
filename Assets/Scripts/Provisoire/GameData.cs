using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GameData", menuName = "New GameData")]
public class GameData : ScriptableObject
{
	[TabGroup("Player")]
	public Life PlayerLife;
	[TabGroup("Player")]
	public VelocityProperties PlayerSpeedProperties;
	[TabGroup("Player")]
	public VelocityProperties PlayerHeadSpeedProperties;
}
