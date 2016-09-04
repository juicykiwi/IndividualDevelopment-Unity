using UnityEngine;
using System.Collections;

public class FieldIndex {

	public int _fieldMainId = 0;
	public int _fieldSubId = 0;

	public override bool Equals(object obj)
	{
		if (obj == null)
			return false;

		FieldIndex fieldIndex = obj as FieldIndex;
		if (fieldIndex == null)
			return false;

		if (fieldIndex._fieldMainId != this._fieldMainId)
			return false;

		if (fieldIndex._fieldSubId != this._fieldSubId)
			return false;

		return true;
	}

	public override int GetHashCode ()
	{
		return _fieldMainId ^ _fieldSubId;
	}
}
