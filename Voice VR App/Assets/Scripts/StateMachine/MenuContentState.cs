using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VVR;


public class MenuContentState : IState
{
    ContentTypeSM _contentMB;
    GameObject _sphere;

    public MenuContentState(ContentTypeSM contentSM){
        //instantiate whatever data here
        _contentMB = contentSM;
        _sphere = Player.Sphere;
    }
    public void Enter()
    {
        _sphere.SetActive(false);
    }

    public void Exit()
    {
        Debug.Log("exiting menu");
        _sphere.SetActive(true);
    }

    public void SwitchContentState(string contentNodeTypeName){
        //this should only run when we either exit menu or select a new course based on menu
        switch (contentNodeTypeName){
            case "menu":
                //go back to where we were
                Debug.Log("leaving menu");
                _contentMB.RevertState();
                break;
            default:
                Debug.Log("lol idk that node type");
                break;
        }
    }
    public void LoadOnContentState(string source){
    }

    public void DoState(string command = ""){
        
    }
}
