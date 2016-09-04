using UnityEngine;
using System.Collections;

public class LineView : MonoBehaviour {

	private LineRenderer lineRenderer;

	public Vector3 _ViewDir = new Vector3(0.0f,0.0f, 1.0f);
	public float _Degree = 30.0f;
	public float _ViewLength = 10.0f;

	private Vector3[] lits = new Vector3[10];

	// Use this for initialization
	void Start () {
//		GameObject cmain = transform.parent.gameObject;
		lineRenderer = GetComponent<LineRenderer>();

		Vector3 lzero = Vector3.zero;
		lzero.y = 0.5f;

		float perD = (_Degree * 0.1f);
		float hDegree = _Degree * 0.5f;

		for (int i=0; i<10; i++) {
			Quaternion quat = Quaternion.Euler( new Vector3( 0, hDegree - (perD*i), 0 )); // X축을 기준으로 30도 회전
			lits[i] = quat * _ViewDir * _ViewLength; 
			lits[i].y = 0.5f;
		}

		lineRenderer.SetWidth(0.2f, 0.2f);

		lineRenderer.SetPosition(0, lzero);
		
		for (int i=0; i<10; i++) {
			lineRenderer.SetPosition(1+i, lits[i]);
		}
		lineRenderer.SetPosition(11, lzero);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
