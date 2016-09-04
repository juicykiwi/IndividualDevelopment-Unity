using UnityEngine;
using System.Collections;

public class sMainLogic
{
	private static sMainLogic MainLogicInstance;
	
	//use data
	public MainLogic data;
	
	private sMainLogic() {}
	
	public static sMainLogic Instance{
		get{
			if(MainLogicInstance == null)
			{
				MainLogicInstance = new sMainLogic();
			}
			return MainLogicInstance;
		}
	}
}

public class MainLogic : MonoBehaviour {

	public GameObject[] _ATeamList;
	public GameObject[] _BTeamList;

	void Awake () {
		sMainLogic.Instance.data = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
