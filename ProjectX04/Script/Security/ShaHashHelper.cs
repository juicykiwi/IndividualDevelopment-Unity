using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Security.Cryptography;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShaHashHelper : MonoBehaviour {

	public const int HashSize = 40;

	public string _targetText = "";
	public string _hashText = "";

	SHA1Managed _sha = new SHA1Managed();
	
	// Method
	
	void Awake () {
		
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public string Hash(string input)
	{
		byte[] dataByte = Encoding.UTF8.GetBytes(input);
		byte[] hashByte = _sha.ComputeHash(dataByte);
		
		Debug.LogFormat("Hash size : {0}", _sha.HashSize);
		
		string hashToString = "";
		for (int i = 0; i < hashByte.Length; ++i)
		{
			hashToString += hashByte[i].ToString("x2");
		}
		
		Debug.LogFormat("HashString Lenght : {0}", hashToString.Length);
		
		return hashToString;
	}
	
	public void DoHash()
	{
		_hashText = Hash(_targetText);
	}
}

#if UNITY_EDITOR

[CustomEditor(typeof(ShaHashHelper))]
public class ShaHashHelperEditor : Editor
{
	ShaHashHelper _target = null;
	
	// Method
	
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		
		if (_target == null)
		{
			_target = (ShaHashHelper)this.target;
		}
		
		GUILayout.Space(20f);
		if (GUILayout.Button("Hash") == true)
		{
			_target.DoHash();
		}
	}
}

#endif
