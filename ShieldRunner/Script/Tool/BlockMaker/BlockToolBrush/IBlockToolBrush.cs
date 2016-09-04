using UnityEngine;
using System.Collections;

public interface IBlockToolBrush
{
    int ElementId { get; set; }

    IBlockElement Draw(Vector2 pos);
}
