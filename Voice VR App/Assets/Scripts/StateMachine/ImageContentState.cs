using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VVR;

using UnityEngine.Video;
public class ImageContentState : IState
{

    ContentTypeSM _contentMB;


    private Player _player;
    private VideoPlayer _VideoPlayer;

    private string _previousSource;

    public ImageContentState(ContentTypeSM contentSM, Player player, VideoPlayer videoPlayer){
        //instantiate whatever data here
        _contentMB = contentSM;
        _player = player;
        _VideoPlayer = videoPlayer;
    }
    public void Enter()
    {
        _VideoPlayer.enabled = false;
    }

    public void Exit()
    {
        _player.ResetToDefaultMaterial();
    }

    public void SwitchContentState(string contentNodeTypeName){

        switch (contentNodeTypeName){
            case "video360":
                _contentMB.ChangeState(_contentMB.VideoNodeState);
                break;
            case "menu":
                _contentMB.ChangeState(_contentMB.MenuNodeState);
                break;
            case "image360":
                Debug.Log("already in image content node");
                break;
            default:
                Debug.Log("lol idk that node type");
                break;
        }
    }
    public void LoadOnContentState(string source){
        if(source == "" && _previousSource != null){
            _player.ChangeTexture(_previousSource);    
        }else{
            _player.ChangeTexture(source);
            _previousSource = source;
        }
       
    }

    public void DoState(string command = ""){

    }
}

