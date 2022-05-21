using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VVR;

using UnityEngine.Video;
public class ContentTypeSM : StateMachineMB
{
    public ImageContentState ImageNodeState { get; private set; }
    public VideoContentState VideoNodeState { get; private set; }
    public MenuContentState MenuNodeState { get; private set; }

    private Player _player;
    private VideoPlayer _VideoPlayer;
    private VideoManager _VideoManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = Camera.main.GetComponent<Player>();
        _VideoPlayer = Camera.main.GetComponent<VideoPlayer>();
        _VideoManager = Camera.main.GetComponent<VideoManager>();

        ImageNodeState = new ImageContentState(this,_player,_VideoPlayer);
        VideoNodeState = new VideoContentState(this,_VideoPlayer,_VideoManager);
        MenuNodeState = new MenuContentState(this);
        

        //we always start here.
        //ChangeState(MenuState);
        ChangeState(ImageNodeState);
    }
    public void SwitchContentState(string contentNodeTypeName){
        CurrentState.SwitchContentState(contentNodeTypeName);
    }

    public void LoadOnContentState(string source){
        CurrentState.LoadOnContentState(source);
    }
}
