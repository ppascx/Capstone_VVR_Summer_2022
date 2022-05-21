using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRaycastState : IState 
{
    RaycastSM _raycastMB;

    public SelectRaycastState(RaycastSM raycastSM){
        //instantiate whatever data here
        _raycastMB = raycastSM;
    }
    // Start is called before the first frame update
    public void Enter()
    {
        
    }

    public void DoState(string command = ""){
        
    }
    // Update is called once per frame
    public void Exit()
    {
        
    }

    public void SwitchContentState(string contentNodeTypeName){
    }
    public void LoadOnContentState(string source){
    }
}
