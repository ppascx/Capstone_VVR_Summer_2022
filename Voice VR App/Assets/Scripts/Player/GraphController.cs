using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour
{   
    Graph graph;
    
    GraphNode currentnode;

    GameObject sphere;

    #region State Machine
    private ContentTypeSM contentSM;
    #endregion
    
    private JsonData jsonReader;
    
    private void Start() {
        
        sphere = GameObject.Find("Sphere");
        contentSM = gameObject.GetComponent<ContentTypeSM>(); 

        jsonReader = gameObject.GetComponent<JsonData>();

        //test to create a new graph.
        graph = new Graph();
        //var node1 = NodeFactory.GetNode("menuhome");
        //graph.AddNode(node1); 
        //node1.Position = new Vector3(0,0,0);
        //var node2 =  NodeFactory.GetNode("video360");
        //graph.AddNode(node2);
        //graph.AddEdge(node1,node2);
        //Debug.Log(node1);
        //Debug.Log(node2.ToString());
        //node2.Position = new Vector3(0,0,5);
        //var node3 =  NodeFactory.GetNode("position3d");
        //graph.AddNode(node3);
        //graph.AddEdge(node3,node2);
        //node3.Position = new Vector3(0,0,10);
        //
        //var node4 =  NodeFactory.GetNode("loop360");
        //graph.AddNode(node4);
        //graph.AddEdge(node4,node3);
        //node4.Position = new Vector3(0,0,15);
        //print("start" + graph.ToString());
//
//
        //var node5 = NodeFactory.GetNode("loop360");
        //graph.AddNode(node5);
        //graph.AddEdge(node3,node5);
        //node5.Position = new Vector3(-8,0,5);
        //
        //
        //var node6 = NodeFactory.GetNode("video360");
        //graph.AddNode(node6);
        //graph.AddEdge(node3,node6);
        //graph.AddEdge(node5,node6);
        //node6.Position = new Vector3(-8,0,15);
        //ChangeCurrentNode(node1);
        //Debug.Log("current Node is" + currentnode.ToString());
        

        /*
        ///////////////////TEST WITH FALL VIDEOS
        var falls = NodeFactory.GetNode("video360");
        falls.Position = new Vector3(0,0,0);
        falls.value.src = "Assets/Resources/Videos/video1.mp4";


        var falls2 = NodeFactory.GetNode("video360");
        // falls2.Position = new Vector3(3,0,0);
        falls2.Position = new Vector3(2,0,1);
        falls2.value.src = "Assets/Resources/Videos/video1.mp4";

        graph.AddNode(falls);
        graph.AddNode(falls2);
        graph.AddEdge(falls,falls2);
        ChangeCurrentNode(falls);
        */
        
        ////////////////////TEST With Images
        //var rus1 = NodeFactory.GetNode("image360");
        //var rus2 = NodeFactory.GetNode("image360");
        //var rus3 = NodeFactory.GetNode("image360");
        //rus1.Position = new Vector3(0,0,0);
        //rus2.Position = new Vector3(0,0,3);
        //rus3.Position = new Vector3(0,0,6);
        //rus1.value.src = "Images_360/kazanskaya_square";
        //rus2.value.src = "Images_360/saint_Isaac_cathedral";
        //rus3.value.src = "Images_360/savior_on_blood";
        //graph.AddNode(rus1);
        //graph.AddNode(rus2);
        //graph.AddNode(rus3);
        //graph.AddEdge(rus1,rus2);
        //graph.AddEdge(rus3,rus2);
        //ChangeCurrentNode(rus1);
        //Debug.Log("current Node is" + currentnode.ToString());


        ////////////////////////TEST WITH IMage to Video and Video to Image
        //var nod1 = NodeFactory.GetNode("image360");
        //var nod2 = NodeFactory.GetNode("video360");
        //var nod3 = NodeFactory.GetNode("image360");
        //nod1.Position = new Vector3(0,0,0);
        //nod2.Position = new Vector3(0,0,3);
        //nod3.Position = new Vector3(0,0,6);
        //nod1.value.src = "Images_360/kazanskaya_square";
        //nod2.value.src = "Videos/video2";
        //nod3.value.src = "Images_360/savior_on_blood";
        //graph.AddNode(nod1);
        //graph.AddNode(nod2);
        //graph.AddNode(nod3);
        //graph.AddEdge(nod1,nod2);
        //graph.AddEdge(nod3,nod2);
        //ChangeCurrentNode(nod1);
        //Debug.Log("current Node is" + currentnode.ToString());

        //TEST WITH THE NEW JSON READER
        // Debug.Log(jsonReader);
        // foreach( NodeJSONData node in jsonReader.myGraphData.nodes.nodes){
        //     var temp = NodeFactory.GetNode(node.nodeType);
        //     temp.Position = new Vector3(node.positionX,node.positionY,node.positionZ);
        //     temp.value.src = node.src;
        //     graph.AddNode(temp);
        // }
        // foreach (EdgeJSONData edge in jsonReader.myGraphData.edges.edges){
        //     graph.AddEdge(graph.nodes[edge.source],graph.nodes[edge.target]);
        // }
        // ChangeCurrentNode(graph.nodes[0]);
        // Debug.Log("current Node is" + currentnode.ToString());
    }

    public bool ChangeCurrentNode(Vector3 pos){
        GraphNode node = graph.FindNodeByPosition(pos);
        if(node != null){
            return ChangeCurrentNode(node);
        }
        return false;
        
    }
    public bool ChangeCurrentNode(GraphNode changeNode){
        //TO DO:  make the exit and entering async functions AND error handle.
        if(changeNode != null){
            if (currentnode != null){
                //currentnode.OneExitingNode();
            }
            //will move to different node state if need be
            contentSM.SwitchContentState(changeNode.value.Name);
            currentnode = changeNode;
            //changeNode.OnEnteringNode();
            contentSM.LoadOnContentState(currentnode.value.src);

            //moves sphere position as well!
            sphere.transform.position = changeNode.Position;
            this.transform.position = changeNode.Position;
            Debug.Log("Changing current note to:  "+ changeNode.value.src);
            return true;
        }
        return false;
        
    }
    // Test function, will delete later
    public bool PlayVideoFileTest(string source)
    {
        graph = new Graph();
        var videoNode = NodeFactory.GetNode("video360");
        videoNode.Position = new Vector3(0, 0, 0);
        videoNode.value.src = source;
        graph.AddNode(videoNode);
        return ChangeCurrentNode(videoNode);
    }
    public IList<GraphNode> GetAllNodes(){
        return graph.Nodes;
    }
    public IList<GraphNode> GetAllCurrentNodeNeighbors(){
        return currentnode.Neighbors;
    }

    public GraphNode GetCurrentNode()
    {
        return currentnode;
    }
}