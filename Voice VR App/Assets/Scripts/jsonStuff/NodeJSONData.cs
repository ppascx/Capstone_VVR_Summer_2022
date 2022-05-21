using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeJSONData 
{
    public string nodeType;
    public int id;
    public string src;

    public string title;

    public float positionX;
    public float positionY;
    public float positionZ;
}
[System.Serializable]
public class NodeJSONDataWrapper
{
    public NodeJSONData[] nodes;
}