using UnityEngine;
using System.Collections;

public enum BlockElementType
{
    None,
    Tile,
    Monster,
    PickupItem,
}

public interface IBlockElement
{
    BlockElementType ElementType { get; }
}
