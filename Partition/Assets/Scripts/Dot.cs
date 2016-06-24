using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour {
	public int index;
	public int row;
	public int column;
	public int totalNeigh;
	public int [] neighbors;
	public bool hasBlob;
	public int father;
	public int childTouch;
	public int[] up;
	public int[] down;
	public int[] left;
	public int[] right;

	// Use this for initialization
	public void Initialize (int i, int r,int col, int tNeig) {
		index = i;
		totalNeigh = tNeig;
		row = r;
		column = col;
		neighbors = new int[totalNeigh];
		father = -1;
		childTouch = -1;
		up = new int[2];
		down = new int[2];
		left = new int[2];
		right = new int[2];
	}

}
