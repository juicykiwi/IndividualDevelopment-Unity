using UnityEngine;
using System.Collections;

public class BlockToolPickupItemBrush : IBlockToolBrush
{
    int _pickupItemId = 0;
    public int ElementId
    {
        get { return _pickupItemId; }
        set { _pickupItemId = value; }
    }

    public IBlockElement Draw(Vector2 pos)
    {
        PickupItemObject pickupItem = PickupItemManager.instance.CreatePickupItemObject(ElementId);
        if (pickupItem == null)
            return null;

        pickupItem.transform.position = pos;
        return pickupItem;
    }
}
