using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public Node HoveredNode;

    private void Update()
    {
        Node temp = HoveredNode;
        HoveredNode = Graph.GetNodeUnderMouse();
        if(temp != HoveredNode)
        {
            if(temp != null)
            {
                //
            }
        }


        if(Input.GetMouseButtonDown(0))
        {

        }
    }

}
