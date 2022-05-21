using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyRaycastState : IState 
{

    RaycastSM _raycastMB;
    public EmptyRaycastState(RaycastSM raycastSM){
        //instantiate whatever data here
        _raycastMB = raycastSM;
    }
    // Start is called before the first frame update
    public void Enter()
    {
        
    }
    public void DoState(string command = ""){
        switch (command){
            case "move":
                Debug.Log("empty raycast state detected move commmand");
                _raycastMB.ChangeState(_raycastMB.MoveState);
                break;
            default:
                Debug.Log("lol idk that commmand");
                break;
        }
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
