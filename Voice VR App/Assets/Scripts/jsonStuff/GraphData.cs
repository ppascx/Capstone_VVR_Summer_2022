using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GraphData
{
    public bool directed;
    public string moduleName;
    public bool freeRoam;
}

[System.Serializable]
public class GraphDataWrapper
{
    public GraphData graph;
    public NodeJSONDataWrapper nodes;
    public EdgeJSONDataWrapper edges;
}