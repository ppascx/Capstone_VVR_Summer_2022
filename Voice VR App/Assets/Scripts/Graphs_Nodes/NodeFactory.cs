using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

//the way this factory works is that it takes in a string and we get the type.
//it uses something called assemblies, now i don't really know how it works, but it does.
//it looks for all classes that are children of NodeData and lists them to populate the Dictionary on startup.
//needs to be tested on other platforms...hopefully it works...
//otherwise we'll have to do a switch statement or register all types manaually lol
public static class NodeFactory
{
    //dictionary, we get the NodeData.Name value and we get the type.
    private static Dictionary<string, Type> nodesByName;
    private static bool IsInitialized => nodesByName != null;

    public static void InitializeFactory(){

        if(IsInitialized){
            return;
        }

        var nodeTypes = Assembly.GetExecutingAssembly().GetTypes().Where(myType => myType.IsClass && myType.IsSubclassOf(typeof(NodeData)));
        nodesByName = new Dictionary<string, Type>();
        //foreach(var x in nodeTypes){
            //Debug.Log(x.ToString());
        //}
        foreach(var type in nodeTypes){
            var tempNode = Activator.CreateInstance(type) as NodeData;
            nodesByName.Add(tempNode.Name, type);
        }
    }
    //checks if initailzed. will return a FULL graphNode BUT its NodeData will be of the type we need.
    //i don't see a scenario in which a node changes types and functionality at runtime.
   public static GraphNode GetNode(string nodeType){
        InitializeFactory();
        if(nodesByName.ContainsKey(nodeType)){
            //Debug.Log("type found in key!");
            Type type = nodesByName[nodeType];
            var node = new GraphNode((Activator.CreateInstance(type) as NodeData));
            //Debug.Log("sending " +node.Value.Name);
            return node;
        }
        return null;
    }
}