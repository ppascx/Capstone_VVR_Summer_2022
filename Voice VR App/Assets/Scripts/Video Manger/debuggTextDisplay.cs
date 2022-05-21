using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class debuggTextDisplay : MonoBehaviour
{
    //Debug
    public bool debugToScreen;
    public Text debugText;        
    string debugString = "";
    public GameObject MainCamera;
    private GraphController graphControl;
    //private VideoManager videoManager;
    
    // Start is called before the first frame update
    void Start()
    {        
        graphControl = MainCamera.GetComponent<GraphController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(debugText != null && debugToScreen == true) //If Text game object exists, and debug mode is on.
        {
                /* 
                debugString = "Is video clip null? " + (VideoClip==null) + "\n"
                + "Is video playing? " + IsPlaying() + "\n"
                ;
                */
                
                /*
                debugString = "is GraphController null? " + (graphControl == null) + "\n"
                + "Is Current Node? " + (graphControl.GetCurrentNode() == null) + "\n"
                + "Where is Current Node Source File? " + (graphControl.GetCurrentNode().value.src) + "\n"
                + "Does Current Node Resource File Exist? " + (Resources.Load(graphControl.GetCurrentNode().value.src) != null) + "\n"
                ;
                */
                debugText.text = debugString;
        }
    }
}
