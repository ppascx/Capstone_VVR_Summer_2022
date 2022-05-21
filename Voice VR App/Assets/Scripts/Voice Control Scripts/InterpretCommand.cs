using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace VVR
{
    [System.Serializable]
    public class InterpretCommand : MonoBehaviour 
    {
        private const int MAXLENGTH = 20;

        #region State Machine
        private RaycastSM raycastSM;
        private ContentTypeSM contentSM;
        
        #endregion


        public static Dictionary<string, Action> commandArray;  

        void Start()
        {
            
            VideoManager videoManager = gameObject.GetComponent<VideoManager>();
            PlayerMovement playerMovement = gameObject.GetComponent<PlayerMovement>();
            raycastSM = gameObject.GetComponent<RaycastSM>(); 
            contentSM = gameObject.GetComponent<ContentTypeSM>(); 
           
            commandArray = new Dictionary<string, Action>()
            {
                /* Video Controls */
                { "play", videoManager.Play },
                { "pause", videoManager.Pause },
                { "stop", videoManager.Stop },
                { "zoom", videoManager.Zoom },
                { "fast forward", videoManager.FastForward },  
                { "rewind", videoManager.Rewind },
                { "loop video", videoManager.LoopVideo },
                { "step back", videoManager.StepBack },

                /* Player Movement */
                // { "stay", camera.StopTurn},  
                { "look left", playerMovement.snap_left },  
                { "look right", playerMovement.snap_right }, 
                { "look north", playerMovement.north },
                { "look south", playerMovement.south },
                { "look east", playerMovement.east },
                { "look west", playerMovement.west },
                { "look up", playerMovement.look_up },
                { "look down", playerMovement.look_down },

                /* Move */

                { "move", () => {raycastSM.DoState("move");} },
                { "stay", () => {raycastSM.DoState("stay");} },
                { "go", () => {raycastSM.DoState("go");} },
                { "menu", () => {raycastSM.DoState("menu");
                                 contentSM.SwitchContentState("menu");   
                                 contentSM.LoadOnContentState("");} }

                /*Ray Cast*/
                // { "play", () => contentSM.DoState("play"); },
                // { "pause", () => contentSM.DoState("pause"); }
                // { "snap forward", () => camera.Turn(Direction.FORWARD, TurnSpeed.SNAP) }

            };


        }

        /// <summary>
        /// If the string passed contains one of the keywords for playback, it triggers the command on
        /// the video player. The string must have a max length of 20 chars
        /// </summary>
        /// MAXLENGTH only really affects platforms which pass full speech strings into parse (Android, WebGL)
        /// <param name="command">the voice command string</param>
        /// <param name="isExactCommand">if the command is exactly worded</param>
        public static void Parse(string command, bool isExactCommand = false)
        {

            if (command.Length >= MAXLENGTH)
            {
                return;
            }

           /* Small optimization for platforms that return the exact string commands and not full
            freeform strings of text (Windows); makes it so we use the command as a key into the
            dictionary instead of iterating over the entire dictionary */
            if (isExactCommand)
            {
                commandArray[command].Invoke();
                return; // complete parsing
            }

            /* Iterate over the dictionary and find the correct command to invoke/call. Although
            less performant, it is necessary for platforms which pass in full freeform strings of
            text to Parse */
            foreach (KeyValuePair<string, Action> entry in commandArray)
            {
                if (command.Contains(entry.Key))
                {
                    entry.Value.Invoke();
                    return; // complete parsing
                }
            }
            
        }

        /*
        <summary>
        For debugging purposes. Enables text component in the scene and then passes a string to it
        </summary>
        <param name="str"></param>
        */
        // public static string PrintToScene(string str)
        // {
        //     /*
        //     This return was created to debug the interaction between
        //     Mobile Voice Controller and Interpret Command
        //     */
        //     return str;
            

        // }

    }

}
