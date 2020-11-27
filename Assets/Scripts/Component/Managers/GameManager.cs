using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	public GameData GameData;

	public void ReloadLevel()
	{
		SceneManager.LoadScene("MainScene");
	}
}