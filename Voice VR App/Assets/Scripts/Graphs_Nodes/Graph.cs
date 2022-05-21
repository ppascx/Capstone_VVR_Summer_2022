using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public class Graph
{
    //list of nodes.
    public List<GraphNode> nodes = new List<GraphNode>();
    public int Count{
        get{return nodes.Count;}
    }
    //returns as read only.
    public IList<GraphNode> Nodes{
        get {return nodes.AsReadOnly();}
    }

    //removes all neighbors from each nodes so nodes can be garbage collected.
    public void Clear(){
        foreach(GraphNode node in nodes){
            node.RemoveAllNeighbors();
        }
        for(int i = nodes.Count-1;i>=0;i--){
            nodes.RemoveAt(i);
        }
    }
    //adding nodes to a graph, lets us know if operation carried through
    public bool AddNode(GraphNode node){
        if (Find(node)!=null){
            Debug.Log("Failed to Added Node");
            return false;
        }else{
            nodes.Add(node);
            Debug.Log("Added Node: " + node.value.src);
            return true;
        }
    }
    //since it's an undirected graph, we add 2 edges from one neighbor to the other.
    public bool AddEdge(GraphNode nodearg1, GraphNode nodearg2){
        GraphNode node1 = Find(nodearg1);
        GraphNode node2 = Find(nodearg2);

        if ((node1 == null) || (node2 == null)){
            Debug.Log("Failed to add edge.");
            return false;
        } else if (node1.Neighbors.Contains(node2)){
            //edge already exists
            Debug.Log("Attempted to add edge that already exists.");
            return false;
        } else {
            node1.AddNeighbor(node2);
            node2.AddNeighbor(node1);
            Debug.Log("Added edge from (" + node1.value.src + ") to (" + node2.value.src + ").");
            return true;
        }

    }

    //idk if we'll even use this or removeedge.
    public bool RemoveNode(GraphNode value){
        GraphNode removeNode = Find(value);
        if (removeNode == null){
            return false;
        }else{
            nodes.Remove(removeNode);
            foreach (GraphNode node in nodes){
                node.RemoveNeighbor(removeNode);
            }
            return true;
        }
    }

    public bool RemoveEdge(GraphNode value1, GraphNode value2){
        GraphNode node1 = Find(value1);
        GraphNode node2 = Find(value2);

        if ((node1 == null) || (node2 == null)){
            return false;
        } else if (!node1.Neighbors.Contains(node2)){
            //edge doesn't exist
            return false;
        } else {
            node1.RemoveNeighbor(node2);
            node2.RemoveNeighbor(node1);
            return true;
        }
    }
    //Finds by Node value...frankly, don't know if we'll use this.
    public GraphNode Find(GraphNode value){
        foreach (GraphNode node in nodes){
            if (node.Value.Equals(value.Value)){
                return node;
            }
        }
        
        return null;
    }
    //finds node by positions....btw backend should never allow a graph that has two nodes on the same spot in 3d space.
    public GraphNode FindNodeByPosition(Vector3 value){
        foreach (GraphNode node in nodes){
            if (node.Position.Equals(value)){
                return node;
            }
        }
        
        return null;
    }
    //prints graph as string
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < Count; i++){
            builder.Append(nodes[i].ToString());
            if(i<Count-1){
                builder.Append(",");
            }
        }
        return builder.ToString();
    }
}
