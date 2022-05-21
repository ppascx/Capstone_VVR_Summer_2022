using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//a node object
public class GraphNode
{
    //it's nodedata value
    public NodeData value;
    //neighboring nodes
    List<GraphNode> neighbors;
    //position in 3D space, depending on the NodeData, we do different things with this Vector.
    Vector3 position;

    //constructor
    public GraphNode(NodeData value, Vector3 pos = new Vector3()){
        this.value = value;
        neighbors = new List<GraphNode>();
        position = pos;
    }
    public NodeData Value{
        get {return value;}
        
    }
    public Vector3 Position{
        get {return position;}
        set {
            position = value;
        }
    }

    //returns as readonly list.
    public IList<GraphNode> Neighbors{
        get {return neighbors.AsReadOnly();}
    }

    //this is an unweighted graph. only true if neighbor operation carried out.
    public bool AddNeighbor(GraphNode neighbor){
        if(neighbors.Contains(neighbor)){
            return false;
        }else{
            neighbors.Add(neighbor);
            return true;
        }

    }
    // same as above
    public bool RemoveNeighbor(GraphNode neighbor){
        return neighbors.Remove(neighbor);
    }

    //removes all neighbors, idk if we'll even use this.
    public bool RemoveAllNeighbors(){
        for(int i =neighbors.Count - 1; i>=0; i--){
            neighbors.RemoveAt(i);
        }
        return true;
    }
    //to print a node and its neighbors
    public override string ToString()
    {
        StringBuilder nodeString = new StringBuilder();
        nodeString.Append("[Node Value: " + value.Name + " Neighbors: ");
        for (int i = 0; i < neighbors.Count; i++){
            //Debug.Log(neighbors.Count);
            nodeString.Append(neighbors[i].Value.Name + " ");    
        }
        nodeString.Append("]");
        return nodeString.ToString();
    }
    //calls to entering node and exiting node of whatever data node type it is holding.
    public void OnEnteringNode(){
        value.OnEnteringNode();
        return;
    }

    public void OneExitingNode(){
        value.OnExitingNode();
        return;
    }

}
