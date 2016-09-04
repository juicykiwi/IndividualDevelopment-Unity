using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChaPathFinding : MonoBehaviour {

	// Method

	void Awake () {

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FindPath(Vector3 startPos,
	                     Vector3 endPos,
	                     Action<List<Vector3>> findSuccess,
	                     Action findFail)
	{
		PathFinder.instance.FindPath(startPos, endPos, findSuccess, findFail);
	}
}
