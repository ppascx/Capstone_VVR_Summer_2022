using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

//each node represents a different video link
public class Video360Node : NodeData
{
    public override string Name => "video360";
    //public string VideoClip;
    private VideoPlayer VideoPlayer;
    //private AudioSource AudioSource;

    public override void OnEnteringNode(){
        Debug.Log("Hello! You've entered a "+ Name+" Node! with src set to "+src);
        //create your own start function to do this in.
        VideoPlayer = Camera.main.GetComponent<VideoPlayer>();
        VideoPlayer.enabled = true;
        //AudioSource = Camera.main.GetComponent<AudioSource>();
        //VideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        //VideoPlayer.SetTargetAudioSource(0, AudioSource);
        //VideoPlayer.source = VideoSource.VideoClip;
        VideoPlayer.clip = Resources.Load<VideoClip>(src) as VideoClip;
        //VideoPlayer.controlledAudioTrackCount = 1;
        //VideoPlayer.EnableAudioTrack(0, true);
        //AudioSource.volume=1.0f;
        VideoPlayer.Play();
    }
    
    public override void OnExitingNode(){
            Debug.Log("Goodbye! You're leaving a "+ Name+" Node!");
    }
}