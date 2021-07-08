using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
	public int currentLevel;
	public int[] unlockedLevels;

	public PlayerData(CharacterController2D player)
	{
		currentLevel = player.currentLevel;
		unlockedLevels = player.unlockedLevels;
	}
}
