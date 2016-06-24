using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour {
	public GameObject board;
	public Text texto;
	public GameObject button;
	Button next;
	public int s;
	int snaped = 0;
	GameObject [] arrows;

	void Start(){
		next = button.GetComponent<Button>();
		arrows = new GameObject[4];
		if (s >=8){
			switchTutorial();
		}
	}

	public void  switchTutorial(){
		Debug.Log("s "+s);
		Board b = board.GetComponent<Board>();
		int[] arr = new int[5]{40,42,29,30,-1};
		switch (s){
		case 0:
			texto.text = "It is made of individual white blobs";
			b.letCollapse = false;
			b.partition(arr, 1, 41);
			b.createWhiteBlob(41);
			s++;
			break;
		case 1:
			b.stopAbsorbing = true;
			for (int i = 0; i< arr.Length;i++){
				if (arr[i]!= -1){
					Debug.Log("&&&& "+arr[i]);
					StartCoroutine (b.attractCoroutine (arr[i], 41));

				}
			}
			s++;
			break;
		case 2:
			texto.text = "A blob can't move";
			//b.setBlobSize(41, 4);
			s++;
			break;
		case 3:
			next.interactable = false;
			texto.text = "But can be stretched into an empty space";
			arrows[0] = Instantiate (Resources.Load("arrow"),DotsArray.dotsArre.dots[41].transform.position, Quaternion.identity) as GameObject;
			StartCoroutine (waitForSnap(1,0, arrows[0]));
			s++;
			break;
		case 4:
			next.interactable = false;
			texto.text = "A blob can be stretched as long as its core has at least two white blobs";
			if (arrows[0] != null){
				Destroy(arrows[0]);
			}
			Quaternion rot2 = Quaternion.Euler(0, 0, -60);
			arrows[1] = Instantiate (Resources.Load("arrow"),DotsArray.dotsArre.dots[41].transform.position, rot2) as GameObject;
			StartCoroutine (waitForSnap(2,1,arrows[1]));
			//Quaternion rot4 = Quaternion.Euler(0, 0, -135);
			//GameObject arr4 = Instantiate (Resources.Load("arrow"),DotsArray.dotsArre.dots[41].transform.position, Quaternion.identity) as GameObject;
			break;
		case 5:
			if (arrows[1] != null){
				Destroy(arrows[1]);
			}
			Quaternion rot3 = Quaternion.Euler(0, 0, -120);
			arrows[2] = Instantiate (Resources.Load("arrow"),DotsArray.dotsArre.dots[41].transform.position, rot3) as GameObject;
			StartCoroutine (waitForSnap(3,1,arrows[2]));
			break;
		case 6:
			if (arrows[2] != null){
				Destroy(arrows[2]);
			}
			texto.text = "After that, it will split";
			Quaternion rot4 = Quaternion.Euler(0, 0, -180);
			arrows[3] = Instantiate (Resources.Load("arrow"),DotsArray.dotsArre.dots[41].transform.position, rot4) as GameObject;
			StartCoroutine (waitForSnap(4,0,arrows[3]));
			s++;
			break;
		case 7:
			LevelPicker.levelPick.currentLevel = 1;
			Application.LoadLevel (3);
			break;
		case 8:
			next.interactable = false;
			snaped = 0;
			if (arrows[3] != null){
				Destroy(arrows[3]);
			}
			texto.text = "So blobs with value 2, splits after 1 stretch";
			arrows[0] = Instantiate (Resources.Load("arrow"),DotsArray.dotsArre.dots[39].transform.position, Quaternion.identity) as GameObject;
			StartCoroutine (waitForSnap(1,0,arrows[0]));
			s++;
			break;
		case 9:
			LevelPicker.levelPick.currentLevel = 2;
			Application.LoadLevel (4);
			break;
		case 10:
			next.interactable = false;
			snaped = 0;
			if (arrows[0] != null){
				Destroy(arrows[0]);
			}
			texto.text = "And with value 3, splits after 2";
			arrows[0] = Instantiate (Resources.Load("arrow"),DotsArray.dotsArre.dots[40].transform.position, Quaternion.identity) as GameObject;
			StartCoroutine (waitForSnap(1,1,arrows[0]));
			break;
		case 11:
			next.interactable = false;
			if (arrows[0] != null){
				Destroy(arrows[0]);
			}
			rot2 = Quaternion.Euler(0, 0, -60);
			arrows[1] = Instantiate (Resources.Load("arrow"),DotsArray.dotsArre.dots[40].transform.position, rot2) as GameObject;
			StartCoroutine (waitForSnap(2,0,arrows[1]));
			s++;
			break;
		case 12:
			if (arrows[1] != null){
				Destroy(arrows[1]);
			}
			texto.text = "And so on...";
			s++;
			break;
		case 13:
			LevelPicker.levelPick.currentLevel = 3;
			Application.LoadLevel (5);
			break;
		case 14:
			texto.text = "White blobs can't be streched";
			b.stopAbsorbing = true;
			s++;
			break;
		case 15:
			texto.text = "But they will be absorbed by the largest blob around them";
			StartCoroutine (waitForAbsorb(b));
			s++;
			break;
		case 16:
			b.createNewBlob(30,4);
			b.createNewBlob(29,3);
			s++;
			break;
		case 17:
			texto.text = "Which is GREAT!";
			s++;
			break;
		case 18:
			texto.text = "Because a blob will disappear if its value is SIX OR GREATER";
			b.letCollapse = true;
			s++;
			break;
		case 19:
			b.createWhiteBlob(40);
			s++;
			break;
		case 20:
			b.createWhiteBlob(28);
			s++;
			break;
		case 21:
			texto.text = "And we just can't stand blobs";
			s++;
			break;
		case 22:
			texto.text = "...They are too round...";
			s++;
			break;
		case 23:
			Application.LoadLevel (1);
			LevelPicker.levelPick.currentLevel = 4;
			break;
		}
	
	}

	public IEnumerator waitForSnap(int number, int type, GameObject arrow){
		while (snaped < number) {
			yield return null;
		}
		if (arrow != null){
			Destroy(arrow);
		}
		if (type == 0){
			next.interactable = true;
		} else{
			s++;
			switchTutorial();
		}

		yield return null;
		
	}

	public IEnumerator waitForAbsorb(Board b){
		int i = 0;
		while (i<100) {
			i++;
			yield return null;
		}
		b.stopAbsorbing = false;
		
		yield return null;
		
	}


	void Snap(){
		snaped++;
	}


}
