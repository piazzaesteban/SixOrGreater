using UnityEngine;
using System.Collections;

public class BlobSon : Blob {
	Animator anim;
	public int parentCurrentSize;
	public int sonIndex;
	public int touchID;
	public int parentIndex;
	public SpriteRenderer rend;

	public void Initialize(int pCS, int touch, int index){
		parentCurrentSize = pCS;
		touchID = touch;
		isSon = true;
		parentIndex = index;
	}

	// Use this for initialization
	void Start () {
		sonIndex = -1;
		anim = GetComponent<Animator> ();
		rend = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetInteger ("size", parentCurrentSize);
	}
	

	public void autodestruction(){
		Destroy (gameObject);
	}

//	public void setColor(){
//		if (rend != null) {
//			switch (parentCurrentSize) {
//			case 0:
//				rend.color = new Color(1F, 1F, 1F, 1F);
//				break;
//			case 1:
//				rend.color = new Color(0.5F, 0.5F, 0.95F, 1F);
//				break;
//			case 2:
//				rend.color = new Color(1F, 0.8F, 0F, 1F);
//				break;
//			case 3:
//				rend.color = new Color(1F, 0.4F, 0.11F, 1F);
//				break;
//			case 4:
//				rend.color = new Color(1F, 0.15F, 0.4F, 1F);
//				break;		
//				
//			}
//		}		
//	}
}
