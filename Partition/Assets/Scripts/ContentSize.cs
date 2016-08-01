using UnityEngine;
using System.Collections;

public class ContentSize : MonoBehaviour {
	RectTransform rect;
	Vector3 antDist;

	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform> ();

	}
	
	// Update is called once per frame
	void Update () {
		//rect.pivot = rect.localPosition.normalized;
		if (Vector3.Distance(antDist, rect.localPosition) != 0) {
			rect.localScale = Vector3.one + (Vector3.one*rect.localPosition.magnitude/700);
			antDist = rect.localPosition;
			//Debug.Log ("Dist: "+Vector3.Distance(antDist, rect.localScale) );
		}

		//Debug.Log ("Dist: "+Vector3.Distance(antDist, rect.localScale) );
		//Debug.Log (rect.localPosition);
			
	}
}
