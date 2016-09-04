using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : ManagerBase<PlayerPrefsManager> {

	ShaHashHelper _shaHashHelper = null;
	AesSecurityHelper _aesSecurityHelper = null;

	// Method

	protected override void Awake () {
		base.Awake();

		if (_shaHashHelper == null)
		{
			_shaHashHelper = this.gameObject.AddComponent<ShaHashHelper>();
		}

		if (_aesSecurityHelper == null)
		{
			_aesSecurityHelper = this.gameObject.AddComponent<AesSecurityHelper>();
		}
	}

//	public override void ActionSceneLoaded(SceneType sceneType)
//	public override void ActionSceneClosed(SceneType sceneType)

	void ExampleUse()
	{	
		string key = "TestGoGoKey";
		string value = "TestValue!!   TestValue!!";
		
		DeleteAll();
		
		SetString(key, value);
		
		string getValueString = GetString(key);
		Debug.LogFormat("Get PlayerPrefs Key:{0}, Value:{1}", key, getValueString);
	}

	public void Save()
	{
		PlayerPrefs.Save();
	}

	public void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	public void SetString(string key, string value)
	{
		string hashKey = _shaHashHelper.Hash(key);
		string hashValue = _shaHashHelper.Hash(value);
		string encryptValue = _aesSecurityHelper.Encrypt(value + hashValue);

		PlayerPrefs.SetString(hashKey, encryptValue);
	}

	public string GetString(string key)
	{
		string hashKey = _shaHashHelper.Hash(key);

		if (PlayerPrefs.HasKey(hashKey) == false)
			return "";

		string encryptValue = PlayerPrefs.GetString(hashKey, "");
		if (encryptValue.Length <= 0)
			return "";

		string decryptValue = _aesSecurityHelper.Decrypt(encryptValue);
		if (decryptValue.Length < ShaHashHelper.HashSize)
			return "";

		string value = decryptValue.Substring(0, decryptValue.Length - ShaHashHelper.HashSize);
		string valueHash = decryptValue.Substring(decryptValue.Length - ShaHashHelper.HashSize);

		if (_shaHashHelper.Hash(value) != valueHash)
			return "";

		return value;
	}
}
