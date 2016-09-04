using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Security.Cryptography;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AesSecurityHelper : MonoBehaviour {
	//	Aes _aes = new AesCryptoServiceProvider();

	public string _keyStr = "";

	public string _originalText = "";
	public string _encryptText = "";
	public string _decryptText = "";

	RijndaelManaged _aes = new RijndaelManaged();

	// soltByte는 최소 8자리 이상 입력
	byte[] _soltByte = new byte[8] {0, 0, 0, 0, 0, 0, 0, 0};
	
	// Method
	
	void Awake () {
		InitAes();
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void InitAes()
	{
		_aes.KeySize = 256;
		_aes.BlockSize = 128;
		_aes.Mode = CipherMode.CBC;
		_aes.Padding = PaddingMode.PKCS7;

		Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(_keyStr, _soltByte, 1000);

		// Key And IV (Initialize Vector)
		_aes.Key = keyGenerator.GetBytes(_aes.KeySize / 8);
		_aes.IV = keyGenerator.GetBytes(_aes.BlockSize / 8);
	}
	
	public string Encrypt(string input)
	{
		ICryptoTransform encrypt = _aes.CreateEncryptor();
		byte[] inputByte = Encoding.UTF8.GetBytes(input);
		byte[] encryptByte = encrypt.TransformFinalBlock(inputByte, 0, inputByte.Length);
		
		return Convert.ToBase64String(encryptByte);
	}
	
	public string Decrypt(string encryptData)
	{
		ICryptoTransform decrypt = _aes.CreateDecryptor();
		
		byte[] encryptByte = Convert.FromBase64String(encryptData);
		byte[] decryptByte = decrypt.TransformFinalBlock(encryptByte, 0, encryptByte.Length);
		
		return Encoding.UTF8.GetString(decryptByte);
	}
	
	[ContextMenu("DoEncrypt")]
	public void DoEncrypt()
	{
		InitAes();
		_encryptText = Encrypt(_originalText);
	}
	
	[ContextMenu("DoDecrypt")]
	public void DoDecrypt()
	{
		InitAes();
		_decryptText = Decrypt(_encryptText);
	}
}

#if UNITY_EDITOR

[CustomEditor(typeof(AesSecurityHelper))]
public class AesSecurityHelperEditor : Editor
{
	AesSecurityHelper _target = null;
	
	// Method
	
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		
		if (_target == null)
		{
			_target = (AesSecurityHelper)this.target;
		}
		
		GUILayout.Space(20f);
		if (GUILayout.Button("Encrypt") == true)
		{
			_target.DoEncrypt();
		}
		
		if (GUILayout.Button("Decrypt") == true)
		{
			_target.DoDecrypt();
		}
	}
}

#endif
