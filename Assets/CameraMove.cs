using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float minSize = 2f;
    float maxSize = 15f;
    float sensitivity = 2f;

    public float DragFactor;
    private Vector3 grabPos;
    private Vector3 cameraPos;
    void Update()
    {
        DragCamera();
        UpdateZoom();
    }

    void FocusCamera(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y, 10);
    }

    void DragCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grabPos = Input.mousePosition;
            cameraPos = transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = grabPos - Input.mousePosition;
            transform.position = cameraPos + delta * DragFactor;
        }
    }


    void UpdateZoom()
    {
        float size = Camera.main.orthographicSize;
        size -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        size = Mathf.Clamp(size, minSize, maxSize);
        Camera.main.orthographicSize = size;
    }
}
