using UnityEngine;
using System.Collections;

public class Exploder : Blob {

	public void Initialize(int blobMax, int i){
		blobMaxSize = blobMax;
		blobCurrentSize = blobMax;
		index = i;
		anim = GetComponent<Animator> ();
		rend = GetComponent<SpriteRenderer> ();
	}

//	// Use this for initialization
//	void Start () {
//
//	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public override int collapse(){
		hasCollapsed = true;
		return 1;
	}

	public override void setColor(SpriteRenderer render){
		if (render != null) {
			switch (blobCurrentSize) {
			case 0:
				render.color = new Color(1F, 1F, 1F, 1F);
				break;
			case 1:
				//rend.color = new Color(0.5F, 0.5F, 0.95F, 1F);
				render.color = new Color(1F, 0.8F, 0.8F, 1F);
				break;
			case 2:
				//rend.color = new Color(1F, 0.9F, 0.5F, 1F);
				render.color = new Color(1F, 0.4F, 0.9F, 1F);
				break;
			case 3:
				//rend.color = new Color(1F, 0.4F, 0.11F, 1F);
				render.color = new Color(0.55F, 0F,1F, 1F);
				break;
			case 4:
				//rend.color = new Color(1F, 0.15F, 0.4F, 1F);
				render.color = new Color(0.15F, 0.4F, 1F, 1F);
				//rend.color = new Color(1F, 0.15F, 0.4F, 1F);
				break;		
				
			}
		}
	}

}
