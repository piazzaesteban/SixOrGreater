using UnityEngine;
using System.Collections;

public class Blob : MonoBehaviour {
 	public int index;
	public int blobMaxSize;
	public int blobCurrentSize;
	protected Animator anim;
	protected SpriteRenderer rend;
	protected BlobSon lastSon;
	protected int [] childPointers;
	public bool beenAbsorbed;
	public bool hasCollapsed;
	public float timer;
	public bool falling;
	public bool isSon;

	public void Initialize(int blobMax, int i){
		blobMaxSize = blobMax;
		blobCurrentSize = blobMax;
		index = i;
		anim = GetComponent<Animator> ();
		rend = GetComponent<SpriteRenderer> ();
		beenAbsorbed = false;
		hasCollapsed = false;
		isSon = false;

	}


	// Use this for initialization
	void Start (){
	
	}
	
	// Update is called once per frame
	void Update () {
		setColor (rend);
		anim.SetInteger ("size", blobCurrentSize);

		foreach(Transform child in transform)
		{
			BlobSon blobSon = child.GetComponent<BlobSon>();
			blobSon.parentCurrentSize = blobCurrentSize;
			setColor(blobSon.rend);
		}
	}

	public BlobSon createSon(int id){
		Debug.Log("createSon");
		Debug.Log("blobCurrentSize1: "+ blobCurrentSize);
		BlobSon bs = null;
		if (blobCurrentSize >= 1) {
			Debug.Log("blobCurrentSize >= 1");
			GameObject blobSon = Instantiate (Resources.Load("blobSon"),transform.position, Quaternion.identity) as GameObject;
			blobSon.transform.parent = transform;
			bs = blobSon.GetComponent<BlobSon>();
			lastSon = bs;
			blobCurrentSize --;
			bs.Initialize(blobCurrentSize, id, index);
		}
		Debug.Log("blobCurrentSize2: "+ blobCurrentSize);
		return bs;
	}

	public BlobSon getSon(int id){
		foreach(Transform child in transform)
		{
			BlobSon blobSon = child.GetComponent<BlobSon>();
			if (blobSon != null && blobSon.touchID == id){
				return blobSon;
			}
		}
		return null;
	}

	/*public void sonMovement(Vector2 pos, int id){
		Vector2 v2 = (Vector2)pos - (Vector2)transform.position;

		float angle = (Mathf.Atan2 (v2.y, v2.x)* Mathf.Rad2Deg) + 90;
		foreach(Transform child in transform)
		{
			BlobSon blobSon = child.GetComponent<BlobSon>();
			blobSon.parentCurrentSize = blobCurrentSize;
			if (blobSon != null && blobSon.touchID == id){
				child.rotation = Quaternion.Euler(new Vector3(0,0,angle));
			}
			else{
				//Debug.Log("Child not found!!!: " + blobSon.touchID + " "+id );
			}
		}
	}*/

	public void sonMovement1(Vector2 pos, BlobSon blobSon){
		Vector2 v2 = (Vector2)pos - (Vector2)transform.position;
		
		float angle = (Mathf.Atan2 (v2.y, v2.x)* Mathf.Rad2Deg) + 90;
		blobSon.parentCurrentSize = blobCurrentSize;

		if (blobSon != null){
			//Debug.Log("blobSon.transform"+blobSon.transform);
			//Debug.Log("blobSon.transform"+blobSon.transform);
			blobSon.transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
		}
	}

	public bool sonMovement(Vector2 pos,BlobSon blobSon, int best){
		bool res = false;
		Vector2 v2 = (Vector2)pos - (Vector2)transform.position;
		
		float angle = (Mathf.Atan2 (v2.y, v2.x)* Mathf.Rad2Deg) + 90;
		if (blobSon!=null){
			blobSon.transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
			blobSon.sonIndex = best;
		}
		else{
			Debug.Log("BLOB SON IS NULL!!");
			return false;
		}

		if (blobCurrentSize == 0) {
			res = true;
		}
		return res;
	}


	/*public bool sonMovement(Vector2 pos, int id, int best){
		bool res = false;
		Vector2 v2 = (Vector2)pos - (Vector2)transform.position;
		
		float angle = (Mathf.Atan2 (v2.y, v2.x)* Mathf.Rad2Deg) + 90;
		foreach(Transform child in transform)
		{
			BlobSon blobSon = child.GetComponent<BlobSon>();
			blobSon.parentCurrentSize = blobCurrentSize;
			if (blobSon != null && blobSon.touchID == id){
				child.rotation = Quaternion.Euler(new Vector3(0,0,angle));
				blobSon.sonIndex = best;
			}
			else{
				//Debug.Log("Child not found!!!: " + blobSon.touchID + " "+id );
			}
		}
		if (blobCurrentSize == 0) {
			res = true;
		}		
		return res;
	}*/

	public bool notAlredyChild(int best){
		bool res = true;
		foreach(Transform child in transform)
		{
			BlobSon blobSon = child.GetComponent<BlobSon>();
			blobSon.parentCurrentSize = blobCurrentSize;
			if (blobSon.sonIndex == best){
				res = false;
			}
		}
		return res;
	}

	public int [] partition(){
		int [] part = new int[5]{-1,-1,-1,-1,-1};
		int i = 0;
		foreach(Transform child in transform){
			BlobSon blobSon = child.GetComponent<BlobSon>();
			if (i <5){
				part[i] = blobSon.sonIndex;
				i++;
			}
		}
		return part;
	}


	public void deleteSon (int id){
		foreach(Transform child in transform){
			BlobSon blobSon = child.GetComponent<BlobSon>();
			if (blobSon != null && blobSon.touchID == id){
				blobSon.autodestruction();
				blobCurrentSize++;
			}
		}
	}

	public void deleteAllSons(){
		foreach(Transform child in transform){
			BlobSon blobSon = child.GetComponent<BlobSon>();
			blobSon.autodestruction();
		}
	}

	public virtual int collapse(){
		StartCoroutine (collapseCoroutine ());
		hasCollapsed = true;
		return 0;
	}

	public virtual void fall(Dot dot){}
	
	IEnumerator collapseCoroutine(){
		while (transform.localScale.x>0.1f) {
			transform.localScale -= new Vector3(0.05F, 0.05F, 0);			
			yield return null;
		}
		selfdestroy ();
		yield return null;
		
	}

	public void selfdestroy(){
		Destroy (gameObject);
	}

	public void setSize(int size){
		blobCurrentSize = size;
	}

	public virtual void setColor(SpriteRenderer render){
		if (render != null) {
			switch (blobCurrentSize) {
			case 0:
				render.color = new Color(1F, 1F, 1F, 1F);
				break;
			case 1:
				//rend.color = new Color(0.5F, 0.5F, 0.95F, 1F);
				render.color = new Color(1F, 0.9F, 0.5F, 1F);
				break;
			case 2:
				render.color = new Color(1F, 0.8F, 0F, 1F);
				break;
			case 3:
				render.color = new Color(1F, 0.4F, 0.11F, 1F);
				break;
			case 4:
				render.color = new Color(1F, 0.15F, 0.4F, 1F);
				break;		
				
			}
		}


	}

}
