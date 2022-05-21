using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VVR;

using UnityEngine.Video;

//This node holds a 360 image, very cool and cheaper on memory too!
public class Image360Node : NodeData
{


    // Start is called before the first frame update
    public override string Name => "image360";

    private Player player;
    private VideoPlayer VideoPlayer;

    public override void OnEnteringNode(){
            Debug.Log("Hello! You've entered a "+ Name+" Node!");

            player = Camera.main.GetComponent<Player>();
            player.ChangeTexture(src);


            VideoPlayer = Camera.main.GetComponent<VideoPlayer>();
            VideoPlayer.enabled = false;

    }
    
    public override void OnExitingNode(){
            Debug.Log("Goodbye! You're leaving a "+ Name+" Node!");

            player.ResetToDefaultMaterial();
    }
}
