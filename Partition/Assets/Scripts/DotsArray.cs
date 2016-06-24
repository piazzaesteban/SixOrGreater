using UnityEngine;
using System.Collections;

public class DotsArray : MonoBehaviour {
	float heightLimit;
	float widthLimit;
	int boardSizeW = 12;
	int boardSizeH = 7;
	public Dot [] dots;
	public static DotsArray dotsArre;

	void Awake () {
		if (dotsArre == null) {
			Debug.Log("ARRE IS NULL");
			DontDestroyOnLoad (gameObject);
			dotsArre = this;
			
			float windowaspect = (float)Screen.width / (float)Screen.height;
			//float targetaspect = 16.0f / 9.0f;
			dots = new Dot[80];
			Debug.Log("New dot array");
			float unit = windowaspect * 0.6f;
			//GameObject dot1 = Instantiate (Resources.Load("dot"),new Vector2(unit,unit), Quaternion.identity) as GameObject;
			int i = 0;
			for (int h = 0; h < boardSizeH; h++) {
				if (h%2 == 0){
					float posX = -(((boardSizeW -1)/2)*unit);
					float posY = -(boardSizeH/2)*(unit) + h*(unit);
					for (int w = 0; w < boardSizeW -1; w++) {
						Vector2 pos = new Vector2(posX,posY);
						int totNei  = 6;
						GameObject dot = Instantiate (Resources.Load("dot"),pos, Quaternion.identity) as GameObject;
						Dot dotScr = dot.GetComponent<Dot>();
						dot.transform.parent  = transform;
						dots[i] = dotScr;
						
						if (h==0 || h == boardSizeH-1 ){
							totNei -=2;
						}
						if (w == 0 || w == boardSizeW-2 ){
							totNei -= 1;
						}
						
						dotScr.Initialize(i,h,w,totNei);
						posX += unit;
						i++;
					}
				}
				else{
					float posX = -((boardSizeW)/2)*unit + 0.5f;
					//Debug.Log("!=%2" + posX);
					float posY = -(boardSizeH/2)*unit + h*unit;
					for (int w = 0; w < boardSizeW; w++) {
						Vector2 pos = new Vector2(posX,posY);
						int totNei  = 6;
						GameObject dot = Instantiate (Resources.Load("dot"),pos, Quaternion.identity) as GameObject;
						Dot dotScr = dot.GetComponent<Dot>();
						dot.transform.parent  = transform;
						dots[i] = dotScr;
						
						if (h==0 || h == boardSizeH-1 ){
							totNei -=2;
						}
						if (w == 0 || w == boardSizeW-1 ){
							totNei -= 3;
						}
						dotScr.Initialize(i,h,w,totNei);
						i++;
						posX += unit;
					}
				}
				
			}
			Debug.Log("OK");
			fillNeighbors ();
			
		} else if (dotsArre != this){
			Debug.Log("ARRE NOT NULL");
			Destroy(gameObject);
		}
	}

	void fillNeighbors (){
		Debug.Log("Fill neighs");
		for (int i =0; i< dots.Length; i++) {
			int j = 0;
			if (dots[i].index -1 >=0 && dots[i-1].row == dots[i].row){
				dots[i].neighbors[j] = dots[i].index -1;
				j++;
			}
			if (dots[i].index +1 < dots.Length && dots[i+1].row == dots[i].row){
				dots[i].neighbors[j] = dots[i].index +1;
				j++;			
			}
			if (dots[i].index - 12 >=0 && dots[i-12].row == dots[i].row-1){
				dots[i].neighbors[j] = dots[i].index -12;
				dots[i].down[0] = dots[i].index -12;
				dots[i].left[0] = dots[i].index -12;
				j++;			
			}
			if (dots[i].index - 11 >=0 && dots[i-11].row == dots[i].row-1){
				dots[i].neighbors[j] = dots[i].index -11;
				dots[i].down[1] = dots[i].index -11;
				dots[i].right[0] = dots[i].index -11;
				j++;			
			}
			if (dots[i].index + 12 < dots.Length && dots[i+12].row == dots[i].row+1){
				dots[i].neighbors[j] = dots[i].index +12;
				dots[i].right[1] = dots[i].index +12;
				dots[i].up[0] = dots[i].index +12;
				j++;
			}
			if (dots[i].index + 11 < dots.Length && dots[i+11].row == dots[i].row+1){
				dots[i].neighbors[j] = dots[i].index +11;
				dots[i].up[1] = dots[i].index +11;
				dots[i].left[1] = dots[i].index +11;
				j++;
			}
		}
		Debug.Log("OK2");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
