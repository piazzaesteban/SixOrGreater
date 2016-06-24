using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public class LevelGenerator : MonoBehaviour {
	Dot [] dots;
	int[][] arrays = new int[14][];
	int[][][] posibilities = new int[14][][];
	int [] neighs = new int[]{1,-11,-12,-1,11,12};
	int neighsPointer;
	int [] neighs2grade = new int[]{13,2,-10,-22,-23,-24,-13,-2,10,22,23,24};

	// Use this for initialization
	void Start () {
		dots = DotsArray.dotsArre.dots;
		generateLevels ();
	
	}

	void generateLevels(){
//		int count = 0;
//		String res = "";
//		for (int i = 5; i > 1; i--) {
//			for (int j = i; j > 0; j--) {
//				res += "int[] lev" +count + " = "+generateLevels (i, j,false)+";\n";
//				res += "arrays[" +count+"] = lev" +count +";\n";
//				count++;
////				if (aux!= null){
////					arrays[count] = aux;
////					
////				}
//			}
//		}
//		Debug.Log(res);

		int[] lev0 = {5,5,5,5,5,5};
		arrays[0] = lev0;
		int[] lev1 = {5,4,5,5,5};
		arrays[1] = lev1;
		int[] lev2 = {5,4,4,5};
		arrays[2] = lev2;
		int[] lev3 = {5,3,4};
		arrays[3] = lev3;
		int[] lev5 = {4,5,5,5,5};
		arrays[4] = lev5;
		int[] lev6 = {4,4,5,5};
		arrays[5] = lev6;
		int[] lev7 = {4,4,4};
		arrays[6] = lev7;
		int[] lev8 = {4,2};
		arrays[7] = lev8;
		int[] lev9 = {3,5,5,5};
		arrays[8] = lev9;
		int[] lev10 = {3,4,5};
		arrays[9] = lev10;
		int[] lev11 = {3,3};
		arrays[10] = lev11;
		int[] lev12 = {2,5,5};
		arrays[11] = lev12;
		int[] lev13 = {2,4};
		arrays[12] = lev13;

		neighsPointer = UnityEngine.Random.Range(0, 5);
		Posibility pos = new Posibility(arrays[5],0);
		Debug.Log(printIntArray(pos.mapLevelDescriptor()));

		/*Posibility posN1 = new Posibility(arrays[5],0);
		String posibStr = "data.levels[0].levelDesc = new int[80]";
		posibStr += printIntArray(posN1.mapLevelDescriptor())+";";
		posibStr += "\n data.levels [0].partitions = 1;";
		int[] levelN1 = posN1.mapLevelDescriptor();

		Posibility posN2 = new Posibility(arrays[5],0);
		posibStr += "data.levels[1].levelDesc = new int[80]";
		posibStr += printIntArray(posN2.mapLevelDescriptor(levelN1))+";";
		posibStr += "\n data.levels [1].partitions = 2;";
		Debug.Log(posibStr);*/

		int k = 40;
		String posibStr = "";
		for(int i = 0 ; i< 13; i++){
			for (int j = 0; j < 13; j++){
				Posibility posN1 = new Posibility(arrays[12],0);
				//posibStr += "data.levels ["+(i+k)+"] = new LevelDescriptor ();data.levels["+(i+k)+"].levelDesc = new int[80]";
				//posibStr += printIntArray(posN1.mapLevelDescriptor())+";";
				//posibStr += "\n data.levels ["+(i+k)+"].partitions = 1;";
				//k++;
				int[] levelN1 = posN1.mapLevelDescriptor();

				Posibility posN2 = new Posibility(arrays[1],0);
				posibStr += "data.levels ["+(i+k)+"] = new LevelDescriptor ();data.levels["+(i+k)+"].levelDesc = new int[80]";
				posibStr += printIntArray(posN2.mapLevelDescriptor(levelN1))+";";
				posibStr += "\n data.levels ["+(i+k)+"].partitions = 2;";
				k++;
				Debug.Log(posibStr);
			}


		}

	}

	String generateLevels(int tamBolaIni, int numBol, bool pair){
		int mult = numBol * 6;
		int []bols = new int[numBol+1];
		bols [0] = tamBolaIni;
		int i = 1;
		int cont = 5;
		mult -= tamBolaIni;
		int c = 0;
		for (int j = numBol; j>0;j--){
			if (mult > 8){
				bols[j] = cont; 
				mult -= cont;			
			}
			else{
				if (mult - cont >=2){
					if (pair){
						bols[j]=mult -3;
						j--;
						bols[j]=3;
					}else{
						bols[j]=mult -4;
						j--;
						bols[j]=4;					
						generateLevels (tamBolaIni, numBol, !pair);
					}

					return printIntArray(bols);
				}else if (mult == 1){
					return null;
				}

				bols[j] = mult; 
				mult = 0;
			}
		}
		return printIntArray (bols);
	}

	String printIntArray(int [] arre){
		String cad = "{";
		for (int h = 0; h<arre.Length; h++) {
			cad += arre[h];
			if (h != arre.Length -1){
				cad+=",";
			}

		}
		return cad+"}";
	}

}

public class Posibility{
	public int center;
	int [] values;
	int [] marked;
	int [] granVals;
	int [] sonVals;
	int [] nodes;
	int valuesIndex;
	int [] n1Map = new int[6]{11,12,1,-11,-12,-1};
	int [] n2Map = new int[12]{22,23,24,13,2,-10,-22,-23,-24,-13,-2,10};
	
	public Posibility(int [] nod, int point)
	{
		values = nod;
		marked = new int[]{0,0,0,0,0,0,0,0,0,0,0,0};
		granVals = new int[]{-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
		valuesIndex = 0;
		LinkedNode parent = createTree();
		LinkedNode aNode = parent.sons[1];
		//printTree(parent);
		for (int k = 0; k<50; k++){}
		firstChild(parent);
		//printTree(parent);
		sonVals = new int[7];
		sonVals[0] = parent.value;
		sonVals = treeToArrays(parent, sonVals,1);
		String toPrint = "";
		for (int i = 0; i<sonVals.Length;i++){
			toPrint += ","+sonVals[i];
		}
		toPrint+="  ";
		for (int i = 0; i<granVals.Length;i++){
			toPrint += ","+granVals[i];
		}
		//Debug.Log(toPrint);
	}

	LinkedNode createTree(){
		LinkedNode parent = new LinkedNode();
		//Debug.Log("p");
		//Debug.Log("Created parent");
		int index = 0;
		for (int i = 0; i< 6; i++){
			LinkedNode son;
			son = new LinkedNode();
			//Debug.Log("s");
			parent.addNode(son);
			for (int j = 0;j < 2; j++){
				LinkedNode grandson = new LinkedNode(index);
				//Debug.Log("grandson.index: "+grandson.index);
				son.addNode(grandson);
				index++;
			}
		}
		int k = 0;
		LinkedNode aux = null;
		foreach(LinkedNode node in parent.sons) {
			if (k == 0){			
				aux = parent.sons[5];
				node.addNode(aux.sons[1]);
			}
			else{
				node.addNode(aux.sons[1]);
				aux = node;
			}
			k++;
		}
		return parent;
	}

	LinkedNode firstChild(LinkedNode parent){
		parent.value = values[0]-1;
		int rand = UnityEngine.Random.Range(0, 5);
		LinkedNode second = parent.sons[rand];
		second.value = values[1]-1;
		second.thereIs = true;
		markChildren(second);
		valuesIndex = 2;
		//Debug.Log("Has inserted");
		bool s = randomChildPositioner(parent, 6);
		return parent;
	}

	bool randomChildPositioner(LinkedNode parent, int tries){
		//Debug.Log("Entered method: "+valuesIndex+ " tries: " +tries);
		while (valuesIndex < values.Length && tries > 0){
			//Debug.Log("Entered while");
			int rand = UnityEngine.Random.Range(0, 6);
			//Debug.Log("rand: "+rand);
			LinkedNode second = parent.sons[rand];
			if (!second.thereIs){
				//Debug.Log("!second.thereIs");
				int random = UnityEngine.Random.Range(0, 3);
				//Debug.Log("random: "+random);
				LinkedNode third = second.sons[random];
				if (marked[third.index]==0){
					third.value = values[valuesIndex]-1;
					//Debug.Log("Value "+values[valuesIndex]+" into "+ third.index);
					valuesIndex++;
					markAround(third.index,third.value);
					tries--;
				}
				else{
					bool res = randomChildPositioner(parent, tries);
				}
			}else{
				//Debug.Log("second.thereIs");
				bool res2 = randomChildPositioner(parent, tries);
			}
		}
		if (tries <= 0){
			//Debug.Log("Tries ended");
			return false;
		}else{
			//Debug.Log("Method completed");
			return true;
		}

	}

	void markAround(int index, int value){
		marked[index]=-1;
		granVals[index] = value;
		if (index == 0){
			marked[marked.Length -1] = -1;
			marked[index+1] = -1;
		} else if (index == (marked.Length-1)){
			marked[0] = -1;
			marked[index-1] = -1;
		} else {
			marked[index-1]=-1;
			marked[index+1]=-1;
		}
	}

	void markChildren (LinkedNode parent){
		//marked[parent.sons[0].index]=-1;  
		//marked[parent.sons[1].index]=-1;
		//marked[parent.sons[2].index]=-1;
	}

	public void printTree(LinkedNode node){
		Debug.Log("sons.Count "+node.sons.Count);
		if (node.sons.Count > 0){
			for (int i = 0;i<node.sons.Count;i++){
				Debug.Log("Index: "+node.sons[i].index+" Value: "+node.sons[i].value);
				printTree(node.sons[i]);
			}
		}
	}

	public int[] treeToArrays(LinkedNode node, int[] array, int index){
		//Debug.Log("sons.Count "+node.sons.Count);
		for (int i = 0;i<node.sons.Count;i++){
			if (index < 19){
				array[index] = node.sons[i].value;
				index++;
			}
		}
		return array;
	}

	public int[] mapLevelDescriptor(){
		int[] levelDesc = new int[80]{-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
									-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
									  -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
									-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
									  -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
									-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
									  -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
		int rand = randomCenterRange();
		//Debug.Log("Rand: " +rand);
		levelDesc[rand] = sonVals[0];
		for (int i = 1; i<n1Map.Length; i++){
			levelDesc[rand + n1Map[i]] = sonVals[i];
		}
		for (int j = 1; j<n2Map.Length; j++){
			levelDesc[rand + n2Map[j]] = granVals[j];
		}
		bool check = checkValidSolution(levelDesc);
		if (check){
			return levelDesc;
		}else{
			int[] checkFalse = new int[80]{-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
				-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
				-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
				-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
				-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
				-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
				-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
			return checkFalse;
		};
	}

	public int[] mapLevelDescriptor(int[] levelDesc){
		int rand = randomCenterRange();
		int toS = doToSix(levelDesc[rand],sonVals[0]);
		if (toS < 5){
			levelDesc[rand] = toS;
		}
		else{
			int[] none = new int[1];
			return none;
		}
		levelDesc[rand] = sonVals[0];
		for (int i = 1; i<n1Map.Length; i++){
			if (levelDesc[rand + n1Map[i]]!= -1 && sonVals[i]!=-1){
				int toSix = doToSix(levelDesc[rand + n1Map[i]], sonVals[i]);

				if (toSix < 5){
					levelDesc[rand + n1Map[i]] = toSix;
				}
				else{
					int[] none = new int[1];
					return none;
				}
			}else{
				if (sonVals[i]!= -1){
					levelDesc[rand + n1Map[i]] = sonVals[i];
				}
			}

		}
		for (int j = 1; j<n2Map.Length; j++){
			if (levelDesc[rand + n2Map[j]]!= -1 && granVals[j]!=-1){
				int toSix = doToSix(levelDesc[rand + n2Map[j]], granVals[j]);
				if (toSix < 5){
					levelDesc[rand + n2Map[j]] = toSix;
				}
				else{
					int[] none = new int[1];
					return none;
				}				
			}
			else{
				if (granVals[j]!= -1){
					levelDesc[rand + n2Map[j]] = granVals[j];
				}
			}

		}
		bool check = checkValidSolution(levelDesc);
		if (check){
			return levelDesc;
		}else{
			int[] checkFalse = new int[80]{-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
				-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
				-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
				-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
				-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
				-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
				-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
			return checkFalse;
		}
	}

	public int randomCenterRange(){
		int rand1 = UnityEngine.Random.Range(1, 3);
		int rand = -1;
		switch (rand1){
		case 0:
			rand = UnityEngine.Random.Range(25, 32);
			break;
		case 1:
			rand = UnityEngine.Random.Range(36, 44);
			break;
		case 2:
			rand = UnityEngine.Random.Range(48, 55);
			break;			
		}
		return rand;
	}

	public int doToSix(int levelDesc, int sonVal){
		int toSix1 = -(levelDesc -5);
		int toSix2 = -(sonVal -5);
		int toSix = toSix1 + toSix2;
		
		if (toSix < 5){
			levelDesc = 5 - toSix;
			//Debug.Log(toSix+" toSix1: "+levelDesc);
		}
		else{
			levelDesc = -1;
		}
		return levelDesc;

	}

	bool checkValidSolution(int[] levelDesc){
		int total = 0;
		for (int i = 0; i<levelDesc.Length;i++){
			if (levelDesc[i]!= -1){
				total ++;
				total += levelDesc[i];
			}
		} 
		return total%6 == 0;
	}

}

public class LinkedNode{
	public List <LinkedNode> sons = new List<LinkedNode>();
	public bool thereIs;
	public int value;
	public int index;

	public LinkedNode(){
		thereIs = false;
		value = -1;
		sons  = new List<LinkedNode>();
	}

	public LinkedNode(int ind){
		thereIs = false;
		value = -1;
		sons  = new List<LinkedNode>();
		index = ind;
	}

	public void addNode(LinkedNode node){
		sons.Add(node);
	}

}


