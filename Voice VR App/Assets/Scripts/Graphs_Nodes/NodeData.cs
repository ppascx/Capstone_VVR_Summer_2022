using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//still needs more things to design from this.
public abstract class NodeData
{
    //this name string is what identifies it in the NodeData FACTORY class.
    public abstract string Name {get;}
    public string src;
    
    //runs and sets up whatever it needs when the node enters.
    public abstract void OnEnteringNode();
    //runs and sets up whatever it needs when the node exits.
    public abstract void OnExitingNode();
}
