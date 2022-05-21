using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the point of this node is to represet a home base for the player from which to 
//launch the content...probably will remove idk
public class MenuHomeNode : NodeData
{
    public override string Name => "menuhome";
    public override void OnEnteringNode(){
            Debug.Log("Hello! You've entered a "+ Name+" Node!");

    }
    
    public override void OnExitingNode(){
            Debug.Log("Goodbye! You're leaving a "+ Name+" Node!");
    }
}
