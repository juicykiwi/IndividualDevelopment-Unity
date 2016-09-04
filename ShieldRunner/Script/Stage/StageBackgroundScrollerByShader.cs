using UnityEngine;
using System.Collections;

public class StageBackgroundScrollerByShader : MonoBehaviour
{
	SpriteRenderer _spriteRenderer = null;

	[SerializeField]
	float _scrollValueX = 0f;

    // Method

	void Awake()
	{
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	public void OnChangedCameroPosition(Vector3 cameraPos)
	{
		float diffPosX = cameraPos.x - transform.position.x;
		Vector3 newPos = transform.position;
		newPos.x += diffPosX;
		transform.position = newPos;

		_scrollValueX += diffPosX * 0.01f;
        if (_scrollValueX > 1f)
        {
            _scrollValueX -= 1f;
        }

		_spriteRenderer.material.SetFloat("_MoveValueX", _scrollValueX);
	}
}
