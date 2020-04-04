using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public Node To;
    public Node From;
    private LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (To == null || From == null)
            return;

        Vector3[] positions = { To.transform.position, From.transform.position };
        lr.SetPositions(positions);
    }
}
