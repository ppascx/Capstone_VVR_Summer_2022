using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;


namespace VVR
{
    public class VideoManager: MonoBehaviour
    {   
        #region Video Speed and Field of Views constants
        private const float FASTFORWARD_SPEED = 2.0f;
        private const float REWIND_SECONDS = 0.5f;
        private const int ZOOM_VIEW = 20;
        private const int NORMAL_VIEW = 60;
        #endregion

        #region Video Variables
        private int setZoomView = NORMAL_VIEW;
        private bool isZooming = false;
        private bool isRewinding = false;
        private float zoomTimeCount = 0.0f;
        private float rewindTimeCount = 0.0f;

        /*
        this is used to fix bug on WebGL and iOS builds
        will have to revisit to see if this bug was actually fixed
        with a current update
        */
        private double currentPlayerTime = 0.0;
        #endregion

        #region Video objects
        public VideoClip VideoClip; //NOTE: THIS IS NEVER USED, VIDEO IS CONTROLED IN 'GraphController.cs'
        private VideoPlayer VideoPlayer;
        #endregion

        //Debug
        public bool debugToScreen;
        public Text debugText;        
        string debugString = "";

        void Awake()
        {
            Time.timeScale = 1; //Time passes as fast as realtime  
        }

        void Start()
        {   
            
            //Initialize the video player and project the video onto the player sphere 
            VideoPlayer = gameObject.AddComponent<VideoPlayer>();
            VideoPlayer.playOnAwake = false;
            VideoPlayer.clip = VideoClip; //NOTE: THIS IS NEVER USED, VIDEO IS CONTROLED IN 'GraphController.cs'
            VideoPlayer.renderMode = VideoRenderMode.MaterialOverride;
            VideoPlayer.targetMaterialRenderer = Player.Sphere.gameObject.GetComponent<Renderer>();
            VideoPlayer.targetMaterialProperty = "_MainTex";
            VideoPlayer.Play();
            VideoPlayer.isLooping = true;
        }

        void Update() 
        {
            ZoomUpdate();
            RewindUpdate();

            //This segment of code prints debug variables on screen
            /*
            if(debugText != null && debugToScreen == true)
            {
                debugString = "Is video clip null? " + (VideoClip==null) + "\n"
                + "Is video playing? " + IsPlaying() + "\n"
                ;
                debugText.text = debugString;
            }
            */
        }

        //Play function to be paired with the play button in the UI object in scene
        public void Play()
        {   
            isRewinding = false;
            
            if(VideoPlayer.canSetPlaybackSpeed)
            {
                VideoPlayer.playbackSpeed = 1;
            }
            else 
            {
                Debug.LogWarning("Changing playback speed is not supported on this platform");
            }

            VideoPlayer.Play();
        }

        //Pause function to be paired with the play button in the UI object in scene
        public void Pause()
        {
            isRewinding = false;
            VideoPlayer.Pause();
        }

        //Stop function to be paired with the play button in the UI object in scene
        public void Stop()
        {
            isRewinding = false;
            VideoPlayer.Stop();
        }


        public void StepForward()
        {
            isRewinding = false;
            VideoPlayer.StepForward();
        }


        /*public void StepBack()
        {
            isRewinding = false;
            VideoPlayer.frame--;
        }
        */

        
        public void FastForward()
        {
            isRewinding = false;
            if(VideoPlayer.canSetPlaybackSpeed)
            {
                VideoPlayer.playbackSpeed = FASTFORWARD_SPEED;
            }
            else
            {
                Debug.LogWarning("Changing playback speed is not supported on this platform");
            }
        }


        public void Rewind()
        {
            VideoPlayer.Pause();
            isRewinding = true;
            rewindTimeCount = 0;

#if UNITY_IOS || UNITY_WEBGL
            currentPlayerTime = VideoPlayer.time;
#endif
        }
        
        //Changes view from Zoom to Normal Fields of View starts ZoomUpdate
        public void Zoom()
        {
            isZooming = true;
            zoomTimeCount = 0;

            if(setZoomView == NORMAL_VIEW)
            {
                setZoomView = ZOOM_VIEW;
            }
            else
            {
                setZoomView = NORMAL_VIEW;
            }       
        }
        
        /*
        Returns current state of the player.
        True = Video is actively playing
        False = Video is not actively playing
        */
        public bool IsPlaying()
        {
            return VideoPlayer.isPlaying;
        }


        private void ZoomUpdate()
        {
            if(isZooming)
            {
                if(zoomTimeCount < 1)
                {
                    /*
                    Changes between camera's current Field of View to the
                    setZoomView using zoomTimeCount.
                    */
                    zoomTimeCount += Time.deltaTime;
                    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 
                    setZoomView, zoomTimeCount);
                }
                else
                {
                    zoomTimeCount = 0;
                    isZooming = false;
                }
            }    
        }

        
        private void RewindUpdate()
        {
            if(isRewinding)
            {
                rewindTimeCount += Time.deltaTime;

                if(rewindTimeCount > REWIND_SECONDS)
                {
                    if(VideoPlayer.time < REWIND_SECONDS)
                    {
                        VideoPlayer.time = 0;
                        isRewinding = false;
                    }
                    else
                    {
#if UNITY_IOS || UNITY_WEBGL
                        currentPlayerTime -= REWIND_SECONDS;
                        VideoPlayer.time = currentPlayerTime;
#else
                        VideoPlayer.time -= REWIND_SECONDS;
#endif
                    }
                    rewindTimeCount = 0;
                }
            }
        }

        /*
        This LoopVideo is a function that loops the video
        there could be an integrated function that also does this 
        but I will have to look at the class to see if there is.

        Also there is some button logic that was omitted
        This could be integrated into a separate button script that
        can action the buttons to change color once activated
        will need to discuss with the time if this is something 
        that is of interest to the project.
        */
        public void LoopVideo()
        {
            var tempTime = VideoPlayer.time;
            
            VideoPlayer.isLooping = !VideoPlayer.isLooping;

            if(!VideoPlayer.isLooping)
            {
                VideoPlayer.time = tempTime;
            }
        }
        /*
        Will need to talk to Jonhas to see if how this function is integrated with the 
        script and if StepBack will replace this.
        */
        public void StepBack()
        {
            if (!VideoPlayer.isPlaying) return;
            isRewinding = false;
            VideoPlayer.time -= Time.deltaTime * 2f;
        }

    }
}
