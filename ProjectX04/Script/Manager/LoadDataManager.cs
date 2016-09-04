using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using CustomXmlSerializerUtil;

public enum LoadDataType
{
	Field,
	ChaModel,
	Cha,
	Stage,
	Max,
}

public class LoadDataManager : ManagerBase<LoadDataManager> {
	
	const int FieldCountMax = 5;

	bool _isLoadDatas = false;

	// Prefab
	public PrefabDataDict<TileBase> _tilePrefabDict = new PrefabDataDict<TileBase>();
	public PrefabDataDict<ChaController> _chaPrefabDict = new PrefabDataDict<ChaController>();
	public PrefabDataDict<EffectController> _effectPrefabDict = new PrefabDataDict<EffectController>();
	public PrefabDataDict<Item> _itemPrefabDict = new PrefabDataDict<Item>();
	public PrefabDataDict<PortalBase> _portalPrefabDict = new PrefabDataDict<PortalBase>();
	public PrefabDataDict<HiderBase> _hiderPrefabDict = new PrefabDataDict<HiderBase>();

	// Method

	protected override void Awake () {
		base.Awake();

		LoadAllData();
	}

//	public override void ActionSceneLoaded(SceneType sceneType)
//	public override void ActionSceneClosed(SceneType sceneType)

	public void LoadAllData()
	{
        if (_isLoadDatas == true)
            return;
        
		// Load prefab.
		_tilePrefabDict.LoadPrefabAll();
		_chaPrefabDict.LoadPrefabAll();
		_effectPrefabDict.LoadPrefabAll();
		_itemPrefabDict.LoadPrefabAll();
		_portalPrefabDict.LoadPrefabAll();
		_hiderPrefabDict.LoadPrefabAll();
		
		// Load infodata.

        Transform dataManagerParent = ManagerController.instance.DataManagerObject.transform;

        FieldDataManager.instance.transform.SetParent(dataManagerParent);
        ChaModelManager.instance.transform.SetParent(dataManagerParent);
        ChaDataManager.instance.transform.SetParent(dataManagerParent);
        HiderDataManager.instance.transform.SetParent(dataManagerParent);
        StageDataManager.instance.transform.SetParent(dataManagerParent);
        UserDataManager.instance.transform.SetParent(dataManagerParent);

        FieldDataManager.instance.LoadData();
        ChaModelManager.instance.LoadData();
        ChaDataManager.instance.LoadData();
        HiderDataManager.instance.LoadData();
        StageDataManager.instance.LoadData();
        UserDataManager.instance.LoadData();
			
        _isLoadDatas = true;
	}
}
