using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField]
    Rect _rect = new Rect();

    float deltaTime = 0.0f;

    // Method

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = (int)_rect.height;
        style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

        GUI.Label(_rect, text, style);
    }
}