using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageBackgroundScroller : MonoBehaviour
{
    const float BackgroundMoveOffset = 0.1f;
    const float BackgroundPosPriod = 20.4f;

    [SerializeField]
    List<Transform> _backgroundTransform = new List<Transform>();

    // Method

    public void OnChangedCameroPosition(Vector3 cameraPos)
    {
        float diffPosX = cameraPos.x - transform.position.x;
        Vector3 newPos = transform.position;
        newPos.x += diffPosX;
        transform.position = newPos;

        for (int index = 0; index < _backgroundTransform.Count; ++index)
        {
            Vector3 localPos = _backgroundTransform[index].localPosition;
            localPos.x -= (diffPosX * BackgroundMoveOffset);

            if (localPos.x <= -BackgroundPosPriod)
            {
                localPos.x += BackgroundPosPriod * 3f;
            }

            _backgroundTransform[index].localPosition = localPos;
        }
    }
}
