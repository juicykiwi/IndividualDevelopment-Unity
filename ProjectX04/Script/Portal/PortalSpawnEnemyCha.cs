using UnityEngine;
using System.Collections;

public class PortalSpawnEnemyCha : PortalBase {

	public int _spawnEnemyId = 0;

	public override string _prefab { get { return "PortalSpawnEnemyCha"; } }

	public override void SetInfoData(PortalInfoData info)
	{
		base.SetInfoData(info);

		foreach (string variable in info._variableList)
		{
			ReflectionHelper.instance.MemberParse(this, variable);
		}
	}

	public override PortalInfoData CreateItemInfo()
	{
		PortalInfoData info = base.CreateItemInfo();
		info._variableList.Add(string.Format("_spawnEnemyId:{0}", _spawnEnemyId));
		
		return info;
	}

	public override void EnterTile (ChaController cha)
	{
		
	}
	
	public override void LeaveTile (ChaController cha)
	{
		
	}
}
