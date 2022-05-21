using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the point of this node is to have a video that is a fake "living world" environment
//it's really just a looping video that you have limited control over because you are not supposed to "pause" an environment
//it's an illusion.
public class Loop360Node : NodeData
{
    public override string Name => "loop360";
    public override void OnEnteringNode(){
            Debug.Log("Hello! You've entered a "+ Name+" Node!");

    }
    public override void OnExitingNode(){
            Debug.Log("Goodbye! You're leaving a "+ Name+" Node!");
    }
}