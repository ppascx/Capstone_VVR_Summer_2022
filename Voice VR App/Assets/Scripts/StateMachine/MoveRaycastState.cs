using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRaycastState : IState 
{
    //What references does this command need?
    private CommandHeardChannelSO _moveEvent = default;
    private Gaze _gaze;
    private bool raycasthit = false;

    RaycastSM _raycastMB;

    public MoveRaycastState(RaycastSM raycastSM, CommandHeardChannelSO move, Gaze gaze){
        //instantiate whatever data here
        _raycastMB = raycastSM;
        _moveEvent = move;
        _gaze = gaze;
    }
    // Start is called before the first frame update
    public void Enter()
    {
        _moveEvent.RaiseEvent();
        _gaze.enabled = true;
        //for now gaze only looks at position layer...change the layer for position UI when we add menu and the other thing.
    }
    public void DoState(string command = ""){
        switch (command){
            case "go":
                raycasthit = _gaze.Shoot();
                //should i have a number of attempts???
                if(raycasthit){
                    _raycastMB.ChangeState(_raycastMB.EmptyState);        
                }
                break;
            case "stay":
                _raycastMB.ChangeState(_raycastMB.EmptyState);
                _moveEvent.RaiseEvent();
                break;
            default:
                Debug.Log("lol idk that commmand");
                break;
        }    
    }
    // Update is called once per frame
    public void Exit()
    {
        //toggle event...will hide all position UIs
        //_moveEvent.RaiseEvent();
        _gaze.enabled = false;
    }

    public void SwitchContentState(string contentNodeTypeName){
    }
    public void LoadOnContentState(string source){
    }
}
