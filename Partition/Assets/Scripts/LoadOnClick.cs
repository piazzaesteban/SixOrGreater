using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {
	
	public void LoadScene(int level)
	{
		if (level != -1) {
			Application.LoadLevel (1);
			LevelPicker.levelPick.currentLevel = level;
		} else {
			//DestroyObject(DotsArray.dotsArre);
			Debug.Log("DESTOROYAH");
			Application.LoadLevel (0);
		}

	}

	public void LoadTutorial(int level)
	{
		if (level != -1) {
			Application.LoadLevel (2);
			LevelPicker.levelPick.currentLevel = 0;
		} else {
			//DestroyObject(DotsArray.dotsArre);
			Debug.Log("DESTOROYAH");
			Application.LoadLevel (0);
		}
		
	}

	public void ReloadScene()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
