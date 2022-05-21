using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{

    public TextAsset textJSON; //you can get it like this...

    //Do [System.Serializable] on your classes if you want to see them in the inspector.
    public GraphDataWrapper myGraphData = new GraphDataWrapper();
    // Start is called before the first frame update
    void Start()
    {
        myGraphData = JsonUtility.FromJson<GraphDataWrapper>(textJSON.text);
        Debug.Log(myGraphData);
        Debug.Log(myGraphData.graph);
        Debug.Log(myGraphData.graph.moduleName);
        Debug.Log(myGraphData.nodes);
        Debug.Log(myGraphData.nodes.nodes[0].title);
        Debug.Log(myGraphData.edges);
        Debug.Log(myGraphData.edges.edges[0].source);
    }
}
