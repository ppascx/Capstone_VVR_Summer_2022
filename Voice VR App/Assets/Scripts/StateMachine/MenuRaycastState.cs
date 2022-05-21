using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRaycastState : IState 
{

    RaycastSM _raycastMB;

    //put here the state machine AND whatever data or events you need here
    public MenuRaycastState(RaycastSM raycastSM){
        //instantiate whatever data here
        _raycastMB = raycastSM;
    }

    //do whatever you need to do on the exit and enter here.
    public void Enter()
    {
        
    }
    public void DoState(string command = ""){
        
    }
    public void Exit()
    {
        
    }

    public void SwitchContentState(string contentNodeTypeName){
    }
    public void LoadOnContentState(string source){
    }
}
