using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReflectionHelper {

	static ReflectionHelper _instance = null;
	public static ReflectionHelper instance {
		get {
			if (_instance == null)
				_instance = new ReflectionHelper();

			return _instance;
		}
	}

	public void MemberParse(object obj, string variable)
	{
		string[] splitWord = {":"};
		string[] eachValue = variable.Split(splitWord, System.StringSplitOptions.RemoveEmptyEntries);
		
		if (eachValue == null || eachValue.Length <= 0)
			return;
		
		for (int splitIndex = 0; splitIndex < eachValue.Length; ++splitIndex)
		{
			System.Reflection.MemberInfo[] reflectionMemberInfoList = obj.GetType().GetMember(eachValue[splitIndex]);
			if (reflectionMemberInfoList.Length <= 0)
				continue;
			
			System.Reflection.MemberInfo reflectionMemberInfo = reflectionMemberInfoList[0];
			if (reflectionMemberInfo == null)
				continue;
			
			if (++splitIndex >= eachValue.Length)
				break;
			
			string value = eachValue[splitIndex];
			
			if (reflectionMemberInfo.MemberType == System.Reflection.MemberTypes.Field)
			{
				System.Reflection.FieldInfo reflectionFieldInfo = (System.Reflection.FieldInfo)reflectionMemberInfo;
				
				try
				{
					if (reflectionFieldInfo.FieldType == typeof(System.Single))
					{
						float fValue = System.Convert.ToSingle(value);
						reflectionFieldInfo.SetValue(obj, fValue);
					}
					else if (reflectionFieldInfo.FieldType == typeof(System.Int32))
					{
						int iValue = System.Convert.ToInt32(value);
						reflectionFieldInfo.SetValue(obj, iValue);
					}
					else if (reflectionFieldInfo.FieldType == typeof(System.String))
					{
						reflectionFieldInfo.SetValue(obj, value);
					}
					else if (reflectionFieldInfo.FieldType == typeof(Vector2))
					{
						string[] splitWordDetail = {"(", " ", ",", ")"};
						string[] eachValueDetail = value.Split(splitWordDetail, System.StringSplitOptions.RemoveEmptyEntries);
						
						if (eachValueDetail.Length == 2)
						{
							float x = float.Parse(eachValueDetail[0]);
							float y = float.Parse(eachValueDetail[1]);
							
							Vector2 vector2Value = new Vector2(x, y);
							reflectionFieldInfo.SetValue(obj, vector2Value);
						}
					}
				}
				catch(System.Exception ex)
				{
					Debug.LogErrorFormat("{0} : {1}", ex.GetBaseException().ToString(), ex.Message);
				}
			}
			
			/* Property sample
				else if (info.MemberType == System.Reflection.MemberTypes.Property)
				{
					System.Reflection.P	ropertyInfo propertyInfo = (System.Reflection.PropertyInfo)info;
					var v = 5.0f;
					propertyInfo.SetValue(v, 5.0f, null);
				}
				*/
		}
	}
}
