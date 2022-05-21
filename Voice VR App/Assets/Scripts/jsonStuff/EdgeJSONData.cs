using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EdgeJSONData
{
   public int source;
   public int target;
}
[System.Serializable]
public class EdgeJSONDataWrapper
{
   public EdgeJSONData[] edges;
}