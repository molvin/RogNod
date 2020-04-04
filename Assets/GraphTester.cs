using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GraphTester : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Node node = Graph.GetNodeUnderMouse();
            Debug.Log($"Hit node: {node}");
        }
    }
    private void OnGUI()
    {
        if(GUILayout.Button("Next level"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
