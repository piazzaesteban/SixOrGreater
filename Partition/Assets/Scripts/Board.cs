using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Board : MonoBehaviour {
	int levelNumber;
	float heightLimit;
	float widthLimit;
	int boardSizeW = 12;
	int boardSizeH = 7;
	Dot [] dots;
	Blob [] blobs;
	public RectTransform panel;
	public Text multipleUsesText;
	public Camera camera;
	public Text partitionsText;
	public Text levelText;
	int partitions;
	public int maxPartitions;
	int currentBlob = -1;
	BlobSon currentSon = null;
	int nID = 0;
	int antTouchID = 0;
	int touchID = 0;
	bool beingAttract = false;
	bool collapsing = false;
	float timeLeft;
	float tryAgainTime;
	bool tryAgain;
	public bool isTutorial;
	public bool letCollapse = true;
	public bool stopAbsorbing;
	
	// Use this for initialization
	void Awake () {
		blobs = new Blob[80];
		timeLeft = 0.5f;
		tryAgainTime = 1.0f;
		tryAgain = false;
		float windowaspect = (float)Screen.width / (float)Screen.height;
	}

	void Start () {
		dots = DotsArray.dotsArre.dots;
		initializeLevel ();	
	}


	public void initializeLevel (){
		if (panel != null){
			panel.gameObject.SetActive(false);
		}
		if (multipleUsesText != null){
			multipleUsesText.gameObject.SetActive(false);
		}
		levelNumber = LevelPicker.levelPick.currentLevel;
		Debug.Log ("Current level: " + LevelPicker.levelPick.currentLevel);
		LevelDescriptor ld = LevelPicker.levelPick.levels[LevelPicker.levelPick.currentLevel];
		for (int i = 0; i< ld.levelDesc.Length; i++) {
			if (blobs[i] != null){
				blobs[i].selfdestroy();
			}
			if (ld.levelDesc[i]!=-1){
				createNewBlob(i, ld.levelDesc[i]);
			}
		}
		partitions = ld.partitions; 
		specialInitialization();
	}

	public void createNewBlob(int i, int size){
		if (size < 10){
			GameObject blob = Instantiate (Resources.Load("blob"),dots[i].transform.position, Quaternion.identity) as GameObject;
			Blob blobScr = blob.GetComponent<Blob>();
			blobScr.Initialize(size,i);
			blobs[i] = blobScr;
		}
		else if (size < 20){
			GameObject blob = Instantiate (Resources.Load("exploder"),dots[i].transform.position, Quaternion.identity) as GameObject;
			Exploder expScr = blob.GetComponent<Exploder>();
			expScr.Initialize(size-10,i);
			blobs[i] = expScr;
		}
		else{
			GameObject blob = Instantiate (Resources.Load("tear"),dots[i].transform.position, Quaternion.identity) as GameObject;
			Tear expScr = blob.GetComponent<Tear>();
			expScr.Initialize(size-20,i);
			blobs[i] = expScr;
		}
	}


	// Update is called once per frame
	void Update () {
		//Debug.Log ("DO: "+ Input.deviceOrientation);
		int count = 0;
		int sonCount = 0;
		int fallers = 0;
		for (int h = 0; h< blobs.Length; h++) {
			if (blobs[h]!= null){
				if (blobs[h].blobCurrentSize ==0 &&
				    !blobs[h].beenAbsorbed && blobs[h].transform.childCount == 0 &&
				    !blobs[h].hasCollapsed &&
				    !blobs[h].falling &&
				    !stopAbsorbing &&
				    !blobs[h].isSon){
					int att = getAttracted(h);
					if (att!= -1){
						blobs[h].beenAbsorbed = true;
						beingAttract = true;
					}
				}else if (blobs[h].blobCurrentSize >=5 && !blobs[h].hasCollapsed){
					if (letCollapse){
						timeLeft +=0.2f;
						int col = blobs[h].collapse();
						if (col == 1){
							partition(dots[h].neighbors,col, h);
							//blobs[h].selfdestroy();
							blobs[h] = null;
						}
						else{
							if (col == 2){
								fall(h,false); // pair?
							}
						}
					}
				} else if (blobs[h].falling){
					fallers++;
				} else if (blobs[h].isSon){
					sonCount++;
				}
			}
			else{
				count++;
			}
		}
		if (!isTutorial){
			if (count >= 80) {
				createPanel();
				//LoadNextLevel ();
			}
			
			if (partitions <= 0 && sonCount == 0 && !collapsing && !beingAttract && count < 80 && fallers ==0) {
				timeLeft -= Time.deltaTime;
			}
			if ( timeLeft < 0 )
			{
				multipleUsesText.gameObject.SetActive(true);
				multipleUsesText.text = "No more splits left!";
				tryAgain = true;
			}
			if(tryAgain){
				tryAgainTime -= Time.deltaTime;
				if (tryAgainTime <=0){
					ReloadLevel(1);
					Debug.Log("RELOAD RELOAD");
				}
			}
		}

		if (Input.touchCount > 0) {
			Touch myTouch = Input.GetTouch(0);
			Vector2 dot0Pos = dots [0].transform.position;
			Vector2 dot79Pos = dots [79].transform.position;
			Vector2 touchPos = camera.ScreenToWorldPoint(myTouch.position);

			//Si el toque esta dentro del tablero
			switch(myTouch.phase)
			{
			case TouchPhase.Began:
				if (touchID == antTouchID){
					touchID += nID;
					nID++;
				} 
				else{
					nID = 0;
				}
				if (touchPos.x > dot0Pos.x - 1 && touchPos.x < dot79Pos.x + 1 && touchPos.y < dot79Pos.y + 1 && touchPos.y > dot0Pos.y - 1) {
					currentBlob = getBestDot2(touchPos); // GET BEST DOT

					if (currentBlob != -1) {
						if (blobs[currentBlob].isSon){
							currentSon = (BlobSon)blobs[currentBlob];

							blobs[currentBlob]= null;
							currentBlob= currentSon.parentIndex;					

						}
						else{
							if(myTouch.tapCount >=2){
								int [] sons = blobs[currentBlob].retrieveSons();
								int i = 0;
								while (sons[i] != -1){
									blobs[sons[i]] = null;
									i++;
								} 
								if (isTutorial){
									camera.SendMessage("Snap2");
								}
							}else{
								currentSon = blobs [currentBlob].createSon(touchID);
							}
						}
					} 
				}
				break;
			case TouchPhase.Moved:
				//Debug.Log("Touch moved!");
				if (currentSon != null){
					blobs [currentBlob].sonMovement1(touchPos, currentSon);
				}
				else{
					//Debug.Log("No current blob!");
				}
				break;			
			case TouchPhase.Ended:
				if (currentBlob != -1){
					Debug.Log("Touch ended");
					int best = snapSon(touchPos, currentBlob, touchID);
					if (best != -1){
						Debug.Log("best no -1: "+best);
						Debug.Log("currentBlob: "+currentBlob);
						if (blobs [best] == null && touchID != -1){
							Debug.Log("blobs best null");
							//TODO: Changing dots-son relation
							//dots[best].father = currentBlob;
							//dots[best].childTouch = touchID;
							blobs[best] = currentSon;//blobs[currentBlob].getSon(touchID);
							Debug.Log("$$$$$ currentBlob: "+dots[best].father);
							Debug.Log("$$$$$ currentTouch: "+dots[best].childTouch);
							if (blobs [currentBlob].sonMovement(dots [best].transform.position, (BlobSon)blobs[best],best)){
								partition (blobs [currentBlob].partition (),0,currentBlob);
							}
							if (isTutorial){
								camera.SendMessage("Snap");
							}
						}
						else{
							Debug.Log("Else best no null");
							if (blobs[best].isSon){
								Debug.Log("Destroy son 1");
								BlobSon bs =  (BlobSon)blobs[best];
								blobs[currentBlob].deleteSon(bs.touchID);
								blobs[currentSon.sonIndex] = null;
							}
						}
					}
					else{
						Debug.Log("Else best es 1");
						if (currentSon!= null){
							Debug.Log("Destroy son 2");
							blobs[currentBlob].deleteSon(currentSon.touchID);
							blobs[currentSon.sonIndex] = null;
							Debug.Log("Destroy son 3");
							if (isTutorial){
								camera.SendMessage("Snap2");
							}
						}
					}
				}
				currentSon = null;
				currentBlob = -1;
				touchID = 0;
				break;
			case TouchPhase.Canceled:
				if (currentBlob != -1){
					Debug.Log("Touch ended");
					int best = snapSon(touchPos, currentBlob, touchID);
					if (best != -1){
						Debug.Log("best no -1");
						if (blobs [best] == null){
							Debug.Log("blobs best null");
							//TODO: Changing dots-son relation
							//dots[best].father = currentBlob;
							//dots[best].childTouch = touchID;
							blobs[best] = blobs[currentBlob].getSon(touchID);
							Debug.Log("$$$$$ currentBlob: "+dots[best].father);
							Debug.Log("$$$$$ currentTouch: "+dots[best].childTouch);
							if (blobs [currentBlob].sonMovement(dots [best].transform.position, (BlobSon)blobs[best],best)){
								partition (blobs [currentBlob].partition (),0,currentBlob);
							}
							if (isTutorial){
								camera.SendMessage("Snap");
							}
						}
						else{
							Debug.Log("Else best no null");
							if (blobs[best].isSon){
								Debug.Log("Destroy son 1");
								BlobSon bs =  (BlobSon)blobs[best];
								blobs[currentBlob].deleteSon(bs.touchID);
								blobs[currentSon.sonIndex] = null;
							}
						}
					}
					else{
						Debug.Log("Else best es 1");
						if (currentSon!= null){
							Debug.Log("Destroy son 2");
							blobs[currentBlob].deleteSon(currentSon.touchID);
							blobs[currentSon.sonIndex] = null;
							Debug.Log("Destroy son 3");
						}
					}
				}
				currentSon = null;
				currentBlob = -1;
				touchID = 0;
				break;
			}
			antTouchID = touchID;
		}
	}

	int getBestDot1(Vector2 touchPos){
		int bestI = -1;
		float minDist = float.MaxValue;
		for (int i = 0; i < dots.Length; i++) {
			float newDist = Vector2.Distance(touchPos,dots[i].transform.position);
			if (newDist<minDist){
				minDist = newDist;
				bestI = i;
			}
		}
		return bestI;
	}

	int getBestDot2(Vector2 touchPos){
		float newDist = Vector2.Distance(touchPos,dots[40].transform.position);
		return getBestDot2 (touchPos, 40, newDist, 40);
	}

	int getBestDot2(Vector2 touchPos, int currentBest, float bestDist, int lastNeig){
		float newBest = -1;
		int bestI = -1;
		for (int i = 0; i<dots[currentBest].neighbors.Length; i++) {
			if (dots[currentBest].neighbors[i] != lastNeig){
				float newDist = Vector2.Distance(touchPos,dots[dots[currentBest].neighbors[i]].transform.position);			
				if (newDist<bestDist){
					newBest = newDist;
					bestI = dots[currentBest].neighbors[i];
				}
			}
		}
		if (newBest == -1) {
			return currentBest;
		} else {
			return getBestDot2 (touchPos, bestI, newBest, currentBest);
		}

	} 

	int snapSon(Vector2 touchPos, int currentBlob, int id){
		int bestI = -1;
		float minDist = 30f;
		Vector2 v2 = (Vector2)touchPos - (Vector2)dots[currentBlob].transform.position;			
		float angle = (Mathf.Atan2 (v2.y, v2.x)* Mathf.Rad2Deg) + 90;
		for (int i = 0; i < dots[currentBlob].neighbors.Length; i++) {
			if (Vector2.Distance(touchPos,dots[currentBlob].transform.position)>0.4){
				int index = dots[currentBlob].neighbors[i];
				
				Vector2 vec2 = (Vector2)dots[index].transform.position - (Vector2)dots[currentBlob].transform.position;			
				float angle1 = (Mathf.Atan2 (vec2.y, vec2.x)* Mathf.Rad2Deg) + 90;
				float newDist = Vector2.Distance(touchPos,dots[index].transform.position);
				if (Mathf.Abs(angle1- angle)<minDist){
					minDist = Mathf.Abs(angle1- angle);
					if (blobs[index] == null){
						bestI = index;
					}
				}
				else if((angle1==270 && angle<-45)){
					minDist = 0;
					if (blobs[index] == null){
						bestI = index;
					}
				}
			}
		}
		return bestI;
	}

	public void partition(int [] arr, int type, int current){
		partitions--;
		if (!isTutorial){
			partitionsText.text = ""+partitions;
		}
		for (int i = 0; i< arr.Length; i++) {
			if (arr[i] != -1){
				if(blobs[arr[i]] != null){
					if (!blobs[arr[i]].isSon){
						blobs[arr[i]].blobCurrentSize++;
					}
					else{
						blobs [arr[i]] = null;
						createWhiteBlob(arr[i]);
					}
				}
			}
		}
		if (type == 0) {
			blobs [current].deleteAllSons ();
		}
		else{
			blobs [current].selfdestroy();
		}

	}

	public int getAttracted(int blobInd){
		//Debug.Log (blobInd);
		int maxBlobS = -1;
		int maxBlob = -1;
		for (int i = 0; i< dots[blobInd].neighbors.Length; i++) {
			if (blobs[dots[blobInd].neighbors[i]] != null){
				Blob aBlob = blobs[dots[blobInd].neighbors[i]]; 
				if (aBlob.blobCurrentSize != 0 && aBlob.blobCurrentSize > maxBlobS && blobs[blobInd].transform.childCount == 0 && !aBlob.hasCollapsed){
					maxBlobS = aBlob.blobCurrentSize;
					maxBlob = dots[blobInd].neighbors[i];
				}
			}
		}
		if (maxBlob != -1) {
			StartCoroutine (attractCoroutine (blobInd, maxBlob));
			Debug.Log("Attract indirect call");
		}
		return maxBlob;
	}

	public IEnumerator attractCoroutine(int now, int to){
		Debug.Log("attract Coroutine now: "+now +" to " + to);
		while (blobs[to]!=null && blobs[now]!=null && Vector2.Distance(blobs[now].transform.position,blobs[to].transform.position)>0.1f) {
			blobs[now].transform.position = Vector2.Lerp(blobs[now].transform.position,blobs[to].transform.position,0.07f);
			yield return null;
		}
		if (blobs [to] == null) {
			blobs [now].transform.position = dots [now].transform.position;
			blobs [now].beenAbsorbed = false;
		} else {
			Debug.Log("attract Coroutine 2 now: "+now +" to " + to);
			Debug.Log("blob "+blobs[to].blobCurrentSize);
			blobs [to].blobCurrentSize++;

			Debug.Log("blob "+blobs[to].blobCurrentSize);
			if(blobs [now]!= null) blobs [now].selfdestroy ();
			//Destroy(blobs[now].transform.parent);
			//blobs[now] = null;
		}
		beingAttract = false;
		yield return null;
		
	}

	public void fall(int h, bool left){
		timeLeft +=0.2f;
		int [] direction = null;
		Debug.Log ("BLOB FALLING: "+blobs[h]);
		if (blobs [h].blobCurrentSize > 0) {
			DeviceOrientation deviceOr = Input.deviceOrientation;
			if (deviceOr == DeviceOrientation.LandscapeRight || deviceOr == DeviceOrientation.FaceUp || deviceOr == DeviceOrientation.FaceDown) {
				direction = dots [h].down;
			} else  if (deviceOr == DeviceOrientation.Portrait) {
				direction = dots [h].left;
			} else  if (deviceOr == DeviceOrientation.LandscapeLeft) {
				direction = dots [h].up;
			} else  if (deviceOr == DeviceOrientation.PortraitUpsideDown) {
				direction = dots [h].right;
			}

			int nextDir = -1;
			if (left) { //try right
				Debug.Log ("NEXT LEFT");
				if (dots [direction [1]] != null && blobs [direction [1]] == null) {
					nextDir = direction [1];
					left = false;
				} else if (dots [direction [0]] != null && blobs [direction [0]] == null) {
					nextDir = direction [0];
				}
			} else {
				Debug.Log ("NEXT RIGHT");
				if (dots [direction [0]] != null && blobs [direction [0]] == null) {
					nextDir = direction [0];
					left = true;
				} else if (dots [direction [1]] != null && blobs [direction [1]] == null) {
					nextDir = direction [1];
				} else {
					Debug.Log ("DIR NONE");
				}

			}
			Debug.Log ("NEXT DIR " + nextDir);

			if (nextDir != -1) {
				blobs [h].falling = true;
				blobs [h].blobCurrentSize--;
				StartCoroutine (fallCoroutine (h, nextDir, left));
			} else {
				blobs [h].falling = false;
			}
		} else {
			blobs [h].falling = false;
		}
	}

	IEnumerator fallCoroutine(int now, int to, bool left){
		while (blobs[now]!=null && Vector2.Distance(blobs[now].transform.position,dots[to].transform.position)>0.1f) {
			blobs[now].transform.position = Vector2.Lerp(blobs[now].transform.position,dots[to].transform.position,0.1f);
			yield return null;
		}
		blobs [now].transform.position = dots [to].transform.position;
		blobs [to] = blobs [now];
		blobs [now] = null;
		createWhiteBlob(now);
		fall(to,left); // ?
		yield return null;
		
	}

	public void createWhiteBlob(int index){
		GameObject blob = Instantiate (Resources.Load("blob"),dots[index].transform.position, Quaternion.identity) as GameObject;
		Blob blobScr = blob.GetComponent<Blob>();
		blobScr.Initialize(0,index);
		blobs[index] = blobScr;
		dots[index].father = -1;
		dots[index].childTouch = -1;
	}

	public void setBlobSize(int index, int size){
		blobs[index].setSize(size);
	}

	public void specialInitialization(){
		if(!isTutorial){
			partitionsText.text = ""+partitions;
			levelText.text = ""+ levelNumber;
		}
		if (levelNumber == 4){
			multipleUsesText.gameObject.SetActive(true);
			multipleUsesText.text = "Split the smallest blob, so the bigger ones absorb one blob each";
		}
	}

	public void createPanel(){
		multipleUsesText.gameObject.SetActive(false);
		panel.gameObject.SetActive(true);
	}

	public void LoadNextLevel(){
		Debug.Log("load next");
		LevelPicker.levelPick.levels[levelNumber].passed = true;
		LevelPicker.levelPick.currentLevel++;
		LevelPicker.levelPick.levels[levelNumber+1].unlocked = true;
		LevelPicker.levelPick.save();
		if (LevelPicker.levelPick.currentLevel != 11){
			initializeLevel ();
		}
		else{
			Application.LoadLevel (6);
			Debug.Log("Tutorial loaded");
		}

		//Debug.Log("Next Level: "+LevelPicker.levelPick.currentLevel);
	}

	public void ReloadLevel(int level){
		Application.LoadLevel (1);
	}
	
	public void destroyBlobs(int [] arrBlobs){
		for(int i = 0; i<arrBlobs.Length; i++){
			if (blobs[arrBlobs[i]] != null){
				blobs[arrBlobs[i]].selfdestroy();
			}
		}
	}

}










