using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float StickFactor;
    void Update()
    {
        Vector2 pos = new Vector2(Input.mousePosition.x - Screen.width / 2 , Input.mousePosition.y - Screen.height / 2) / StickFactor;
        transform.position = new Vector3(pos.x, pos.y, -10);
    }
}
