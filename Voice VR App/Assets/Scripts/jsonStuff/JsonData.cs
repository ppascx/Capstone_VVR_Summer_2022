using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonData : MonoBehaviour 
{
    string filename = "graph_test_01.json";
    string path;

    public GraphDataWrapper myGraphData = new GraphDataWrapper();

	// Use this for initialization
	void Start () 
	{
        path = Application.persistentDataPath + "/" + filename;
        Debug.Log(path);

        ReadData();

        Debug.Log(myGraphData);
        Debug.Log(myGraphData.graph);
        Debug.Log(myGraphData.graph.moduleName);
        Debug.Log(myGraphData.nodes);
        Debug.Log(myGraphData.nodes.nodes[0].title);
        Debug.Log(myGraphData.edges);
        Debug.Log(myGraphData.edges.edges[0].source);
	}
	
	// Update is called once per frame
	//void Update () 
	//{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
      //      gameData.date = System.DateTime.Now.ToShortDateString();
      //      gameData.time = System.DateTime.Now.ToShortTimeString();

            //Quest q1 = new Quest();
            //q1.name = "Deliver Beer";
            //q1.desc = "Deliver beer to Fred";
            //gameData.quests.Add(q1);

            //Quest q2 = new Quest();
            //q2.name = "Deliver Food";
            //q2.desc = "Deliver food to Bob";
            //gameData.quests.Add(q2);

    //        SaveData();
    //    }
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        ReadData();
    //    }
	//}

    void SaveData()
    {
        //JsonWrapper wrapper = new JsonWrapper();
        //wrapper.gameData = gameData;

        //string contents = JsonUtility.ToJson(wrapper, true);
        //System.IO.File.WriteAllText(path, contents);
    }

    void ReadData()
    {
        try
        {
            if (System.IO.File.Exists(path))
            {
                string contents = System.IO.File.ReadAllText(path);
                Debug.Log(contents);
                myGraphData = JsonUtility.FromJson<GraphDataWrapper>(contents);
                //gameData = wrapper.gameData;
                //Debug.Log(gameData.date + ", " + gameData.time);

                //foreach (Quest q in gameData.quests)
                //{
                //    Debug.Log(q.desc);
                //}
            }
            else
            {
                Debug.Log("Unable to read the save data, file does not exist");
                //gameData = new GameData();
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}
