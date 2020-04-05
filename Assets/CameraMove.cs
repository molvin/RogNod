using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float minSize = 2f;
    float maxSize = 15f;
    float sensitivity = 2f;

    public float StickFactor;
    private Vector2 grabPos;
    void Update()
    {
        Vector2 pos = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2) / StickFactor;
        transform.position = new Vector3(pos.x, pos.y, -10);
        UpdateZoom();
    }



    void UpdateZoom()
    {
        float size = Camera.main.orthographicSize;
        size -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        size = Mathf.Clamp(size, minSize, maxSize);
        Camera.main.orthographicSize = size;
    }
}
