using UnityEngine;
using System.Collections;

public class Tear : Blob {
	public void Initialize(int blobMax, int i){
		blobMaxSize = blobMax;
		blobCurrentSize = blobMax;
		index = i;
		anim = GetComponent<Animator> ();
		rend = GetComponent<SpriteRenderer> ();
	}
	
	public override int collapse(){
		hasCollapsed = true;
		return 2;
	}

//	public override void fall(int dot){
//		if (blobCurrentSize > 0) {
//			DeviceOrientation deviceOr = Input.deviceOrientation;
//			int devOr = -1;
//			int [] direction;
//			if (deviceOr == DeviceOrientation.LandscapeRight) {
//				devOr = 0;
//				direction = DotsArray.dotsArre.dots[dot].down;
//			} else  if (deviceOr == DeviceOrientation.Portrait){
//				devOr = 1;
//				direction = DotsArray.dotsArre.dots[dot].left;
//			}
//			else  if (deviceOr == DeviceOrientation.LandscapeLeft){
//				devOr = 2;
//				direction = DotsArray.dotsArre.dots[dot].up;
//			}
//			else  if (deviceOr == DeviceOrientation.PortraitUpsideDown){
//				devOr = 3;
//				direction = DotsArray.dotsArre.dots[dot].right;
//			}
//
//		}
//	}


	public override void setColor(SpriteRenderer render){
		if (render != null) {
			switch (blobCurrentSize) {
			case 0:
				render.color = new Color(1F, 1F, 1F, 1F);
				break;
			case 1:
				//rend.color = new Color(0.5F, 0.5F, 0.95F, 1F);
				//render.color = new Color(1F, 0.8F, 0.8F, 1F);
				render.color = new Color(0.6F,0.7F, 1F, 1F);
				break;
			case 2:
				//rend.color = new Color(1F, 0.9F, 0.5F, 1F);
				//render.color = new Color(1F, 0.5F, 0.9F, 1F);
				render.color = new Color(0.3F, 0.50F,0.9F, 1F);
				break;
			case 3:
				//rend.color = new   Color(1F, 0.4F, 0.11F, 1F);
				//render.color = new Color(0.4F, 0.11F,1F, 1F);
				render.color = new Color(0F, 0.8F,0.3F, 1F);
				break;
			case 4:
				//rend.color = new   Color(1F, 0.15F, 0.4F, 1F);
				//render.color = new Color(0.15F, 0.4F, 1F, 1F);
				//new Color(1F, 0.9F, 0.5F, 1F);
				render.color = new Color(0.9F, 0.9F, 0F, 1F);

				break;		
				
			}
		}
	}

}
