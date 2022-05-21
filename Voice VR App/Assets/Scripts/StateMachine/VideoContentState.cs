using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using VVR;
public class VideoContentState : IState
{
    ContentTypeSM _contentMB;

    
    private VideoPlayer _VideoPlayer;
    private VideoManager _VideoManager;

    private string _previousSource;

    public VideoContentState(ContentTypeSM contentSM, VideoPlayer videoPlayer, VideoManager videoManager){
        //instantiate whatever data here
        _contentMB = contentSM;
        _VideoPlayer = videoPlayer;
        _VideoManager = videoManager;
    }
    public void Enter()
    {
        _VideoPlayer.enabled = true;
    }

    public void Exit()
    {
        //stop the video?
    }

    public void SwitchContentState(string contentNodeTypeName){
        switch (contentNodeTypeName){
            case "video360":
                Debug.Log("already in video content node");
                break;
            case "menu":
                _contentMB.ChangeState(_contentMB.MenuNodeState);
                break;
            case "image360":
                _contentMB.ChangeState(_contentMB.ImageNodeState);
                break;
            default:
                Debug.Log("lol idk that node type");
                break;
        }

    }
    public void LoadOnContentState(string source){
        if(source == "" && _previousSource != null){
            _VideoPlayer.url = _previousSource;
        
        }else{
            _VideoPlayer.url = source;
            _previousSource = source;
        }
        _VideoPlayer.Play();

    }

    public void DoState(string command = ""){
        switch (command){
            case "play":
                _VideoManager.Play();
                break;
            case "pause":
                _VideoManager.Pause();
                break;
            default:
                Debug.Log("lol idk that commmand");
                break;
        }    
    }
}
