using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Edge : MonoBehaviour
{
    public Node To;
    public Node From;
    private LineRenderer lr;
    public TextMeshProUGUI Text;

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

        if(Text != null)
        {
            Text.text = Mathf.RoundToInt((To.transform.position - From.transform.position).magnitude).ToString();
            Text.transform.position = (From.transform.position + To.transform.position) * 0.5f + Vector3.up * 0.5f;
        }
    }
}
