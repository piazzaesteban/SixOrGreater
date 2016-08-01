using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	public Button button;
	public Button [] buttons = new Button[100];
	public Transform scrollView;
	public Image dot;
	public Transform gameCanvas;
	public RectTransform panel;


	// Use this for initialization
	void Start () {
		int h = 0;
		Image whiteDot0 = Instantiate (dot) as Image;
		whiteDot0.transform.SetParent(scrollView);
		whiteDot0.rectTransform.anchoredPosition = Vector2.zero;
		Button bu = Instantiate(button) as Button;
		MenuButton mb0 = bu.GetComponent<MenuButton>();
		mb0.Initialize(0);
		bu.transform.SetParent(scrollView);
		RectTransform r = bu.GetComponent<RectTransform>();
		r.anchoredPosition = Vector2.zero;
		int mod = 6;
		int k = 0;
		float dist = 1.2f;
		for (int i = 0; i< 90; i++){
			if (i != 0 && k%mod==0){
				mod*=2;
				k = 0;
				dist ++;
			}
			Image whiteDot = Instantiate (dot) as Image;
			float ratio = ((100*dist)-(dist*mod)/4);
			Vector2 cis = new Vector2(ratio*Mathf.Cos(2*Mathf.PI*k/mod),ratio*Mathf.Sin(2*Mathf.PI*k/mod));
			k++;
			whiteDot.transform.SetParent(scrollView);
			whiteDot.rectTransform.anchoredPosition = cis;//transform.position = cis;						
			whiteDot.rectTransform.localScale = new Vector2(whiteDot.rectTransform.localScale.x-(0.1f*dist),whiteDot.rectTransform.localScale.y-(0.1f*dist));
			if (LevelPicker.levelPick.levels[i+4]!= null && (LevelPicker.levelPick.levels[i+4].unlocked || i+4==4)){
				buttons[i] = Instantiate(button) as Button;
				if (LevelPicker.levelPick.levels[i+4].passed){
					setColor(buttons[i].image,mod);
				}
				MenuButton mb = buttons[i].GetComponent<MenuButton>();
				mb.Initialize(i+4);
				buttons[i].transform.SetParent(scrollView);
				RectTransform rtrans = buttons[i].GetComponent<RectTransform>();
				rtrans.anchoredPosition = cis;
				buttons[i].image.rectTransform.sizeDelta = new Vector2(buttons[i].image.rectTransform.sizeDelta.x-(10f*dist),buttons[i].image.rectTransform.sizeDelta.y-(10f*dist));
				
			}

		}
	}

	public virtual void setColor(Image image, int mod){
		if (image != null) {
			switch (mod) {
			case 0:
				image.color = new Color(1F, 1F, 1F, 1F);
				break;
			case 6:
				//rend.color = new Color(0.5F, 0.5F, 0.95F, 1F);
				image.color = new Color(1F, 0.9F, 0.5F, 1F);
				break;
			case 12:
				image.color = new Color(1F, 0.8F, 0F, 1F);
				break;
			case 24:
				image.color = new Color(1F, 0.4F, 0.11F, 1F);
				break;
			case 48:
				image.color = new Color(1F, 0.15F, 0.4F, 1F);
				break;		
				
			}
		}
		
		
	}

}
