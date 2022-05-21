using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the point of this node is to be an actual position in 3D, real 3D.
class Position3DNode : NodeData
{
    public override string Name => "position3d";
    public override void OnEnteringNode(){
        Debug.Log("Hello! You've entered a "+ Name+" Node!");

    }
    
    public override void OnExitingNode(){
        Debug.Log("Goodbye! You're leaving a "+ Name+" Node!");
    }
}
