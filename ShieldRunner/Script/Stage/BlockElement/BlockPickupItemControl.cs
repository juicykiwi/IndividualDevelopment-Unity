using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BlockPickupItemControl : BlockElementControl<CreationPickupItemObjectInfo, PickupItemObject>
{
    protected override PickupItemObject CreateElement(CreationPickupItemObjectInfo creationInfo)
    {
        if (creationInfo == null)
            return null;

        PickupItemManager manager = PickupItemManager.instance;
        PickupItemObject pickupItemObject = manager.CreatePickupItemObject(creationInfo._uniqueId);
        if (pickupItemObject == null)
            return null;

        pickupItemObject.transform.SetParent(_parentBlockObject.transform);
        pickupItemObject.SetLocalPos(GameObjectHelper.RoundPosition2D(creationInfo._pos));

        return pickupItemObject;
    }

    public override void Add(PickupItemObject tObject)
    {
        if (tObject == null)
            return;

        tObject.transform.SetParent(_parentBlockObject.transform);
        tObject.SetLocalPos(GameObjectHelper.RoundPosition2D(tObject.transform.localPosition));

        _objectList.Add(tObject);
    }

    public override void Remove(PickupItemObject tObject)
    {
        if (tObject == null)
            return;

        _objectList.Remove(tObject);
        tObject.RemoveActionObject();
    }
}
