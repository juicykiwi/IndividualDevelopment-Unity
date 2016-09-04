using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Node
{
	public int valueG = 0;
	public int valueH = 0;
	public int valueF = 0;

	public Vector3 pos = Vector3.zero;

	public Node parentNode = null;

	public List<Node> neighborList = new List<Node>();

	public void Init(Vector3 nodePos, Vector3 startPos, Vector3 endpos, int weight, int parentValueG)
	{
		pos = nodePos;

		valueG = parentValueG + weight;
		valueH = Mathf.Abs((int)(nodePos.x - endpos.x)) + Mathf.Abs((int)(nodePos.y - endpos.y));
		valueF = valueG + valueH;
	}

	public void AddNeighborNode(Direction direction, Vector3 startPos, Vector3 endPos)
	{
		Vector3 newNodePos = this.pos;

		switch (direction)
		{
		case Direction.Right:
			newNodePos += Vector3.right;
			break;

		case Direction.Left:
			newNodePos += Vector3.left;
			break;

		case Direction.Up:
			newNodePos += Vector3.up;
			break;

		case Direction.Down:
			newNodePos += Vector3.down;
			break;

		default:
			return;
		}

		if (PathFinder.instance.GetPath(newNodePos) <= 0)
			return;

		Node neighborNode = new Node();
		neighborNode.Init(newNodePos, startPos, endPos, PathFinder.instance.GetPath(newNodePos), this.valueG);
		neighborNode.parentNode = this;

		neighborList.Add(neighborNode);
	}
}

public class PathFinder : ManagerBase<PathFinder> {

	Dictionary<Vector3, int> _tileDict = new Dictionary<Vector3, int>();

	public bool _isDebug = false;

	public GameObject _debugOpenPathObj = null;
	public GameObject _debugClosePathObj = null;

	public bool _isStartFindPath = false;

	// Method

	protected override void Awake () {
		base.Awake();
	}

	public override void ActionSceneLoaded(SceneType sceneType)
	{
		SceneType[] typeArray = new SceneType[] { SceneType.GameStage };
		
		if (IsHaveSceneTypeFormArray(sceneType, typeArray) == true)
		{
			ClearPath();
		}
	}
	
//	public override void ActionSceneClosed(SceneType sceneType)

	class FindPathStruct
	{
		public Vector3 startPos;
		public Vector3 endPos;
		public Action<List<Vector3>> successAction;
		public Action failAction;
	}

	public void ClearPath()
	{
		_tileDict.Clear();
	}

	public void AddPath(Vector3 pos, int weight)
	{
		if (_tileDict.ContainsKey(pos) == true)
		{
			_tileDict[pos] = weight;
			return;
		}

		_tileDict.Add(pos, weight);
	}

	public int GetPath(Vector3 keyPos)
	{
		if (_tileDict.ContainsKey(keyPos) == false)
			return 0;

		return _tileDict[keyPos];
	}

	public void FindPath(Vector3 startPos,
	                     Vector3 endPos,
	                     Action<List<Vector3>> findSuccess,
	                     Action findFail)
	{
		if (_isDebug == true)
		{
			if (_isStartFindPath == true)
				return;

			_isStartFindPath = true;
		}

		FindPathStruct findPathstruct = new FindPathStruct();
		findPathstruct.startPos = startPos;
		findPathstruct.endPos = endPos;
		findPathstruct.successAction = findSuccess;
		findPathstruct.failAction = findFail;

		StartCoroutine(FindPathCoroutine(findPathstruct));
	}

	IEnumerator FindPathCoroutine(FindPathStruct findPathStruct)
	{
		Vector3 startPos = findPathStruct.startPos;
		Vector3 endPos = findPathStruct.endPos;
		Action<List<Vector3>> successAction = findPathStruct.successAction;
		Action failAction = findPathStruct.failAction;

		if (_tileDict.ContainsKey(startPos) == false)
		{
			_isStartFindPath = false;
			failAction();
			yield break;
		}

		if (_tileDict.ContainsKey(endPos) == false)
		{
			_isStartFindPath = false;
			failAction();
			yield break;
		}

		List<Node> openNodeList = new List<Node>();
		List<Node> closeNodeList = new List<Node>();

		Node firstNode = new Node();
		firstNode.Init(startPos, startPos, endPos, _tileDict[startPos], 0);

		openNodeList.Add(firstNode);

		while (openNodeList.Count > 0)
		{
			Node curCheckNode = openNodeList[0];

			if (_isDebug == true)
			{
				Debug.LogFormat("Pos:{0}, G:{1}, H:{2}, F:{3}",
				                curCheckNode.pos, 
				                curCheckNode.valueG, 
				                curCheckNode.valueH, 
				                curCheckNode.valueF);
			}

			if (curCheckNode.pos == endPos)
				break;

			curCheckNode.AddNeighborNode(Direction.Right, startPos, endPos);
			curCheckNode.AddNeighborNode(Direction.Left, startPos, endPos);
			curCheckNode.AddNeighborNode(Direction.Up, startPos, endPos);
			curCheckNode.AddNeighborNode(Direction.Down, startPos, endPos);

			foreach (Node neighborNode in curCheckNode.neighborList)
			{
				if (closeNodeList.Exists(
					(Node node) => { return node.pos == neighborNode.pos;}) == true)
				{
					continue;
				}

				if (openNodeList.Exists(
					(Node node) => { return node.pos == neighborNode.pos;}) == true)
				{
					continue;
				}

				openNodeList.Add(neighborNode);
				openNodeList.Sort(SortLowValueF);

				if (_isDebug == true)
				{
					GameObject debugObj = Instantiate(_debugOpenPathObj, neighborNode.pos, Quaternion.identity) as GameObject;
					Destroy(debugObj, 1.0f);
				}
			}

			closeNodeList.Add(curCheckNode);
			openNodeList.Remove(curCheckNode);

			if (_isDebug == true)
			{
				yield return new WaitForSeconds(0.1f);
			}
		}

		if (openNodeList.Count <= 0)
		{
			// Not find path.
			_isStartFindPath = false;
			failAction();
			yield break;
		}

		List<Vector3> resultPathPosList = CalculatePath(openNodeList[0]);

		if (_isDebug == true)
		{
			foreach (Vector3 pos in resultPathPosList)
			{
				GameObject debugObj = Instantiate(_debugClosePathObj, pos, Quaternion.identity) as GameObject;
				Destroy(debugObj, 1.0f);
		    }

			_isStartFindPath = false;
		}

		successAction(resultPathPosList);
	}

	List<Vector3> CalculatePath(Node lastNode)
	{
		List<Vector3> resultPosList = new List<Vector3>();

		Node checkNode = lastNode;

		while (checkNode != null)
		{
			resultPosList.Add(checkNode.pos);
			checkNode = checkNode.parentNode;
		}

		resultPosList.Reverse();
		return resultPosList;
	}

	public int SortLowValueF(Node lNode, Node rNode)
	{
		if (lNode.valueF > rNode.valueF)
			return 1;
		
		if (lNode.valueF < rNode.valueF)
			return -1;
		
		return 0;
	}


}
