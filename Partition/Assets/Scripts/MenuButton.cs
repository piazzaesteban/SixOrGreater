using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {
	public int level;

	public void Initialize(int lev){
		level = lev;
	}

	public void LoadScene()
	{
		if (level != -1) {
			if (level == 0){
				Application.LoadLevel (2);
			}
			else if (level == 11){
				Application.LoadLevel (6);
			}else{
				Application.LoadLevel (1);
			}
			LevelPicker.levelPick.currentLevel = level;
		} else {
			Application.LoadLevel (0);
		}
		
	}

	public void LoadTutorial()
	{
		if (level != -1) {

		} else {
			Application.LoadLevel (0);
		}
		
	}

	public void LoadMenu(int submenu){
		LevelPicker.levelPick.currentSubMenu = submenu;
		Application.LoadLevel (0);
	}

}
