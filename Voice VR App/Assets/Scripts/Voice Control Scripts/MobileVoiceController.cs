using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System;

//Note: this is the plugin we use for Android TextToSpeech https://github.com/j1mmyto9/Speech-And-Text-Unity-iOS-Android

namespace VVR
{
    public class MobileVoiceController : MonoBehaviour
    {
        const string LANG_CODE = "en-US";
        string word_store = "Test";

        void Start()
        {
            Setup(LANG_CODE);
            CheckPermission();
#if UNITY_ANDROID
            SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;
#endif
            SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        }

        void CheckPermission()
        {
#if UNITY_ANDROID

            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
#endif
        }
        

    #region Speech to Text

        public void StartListening()
        {
            try
            {
                SpeechToText.instance.StartRecording();
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }            
        }

        public void StopListening()
        {
            SpeechToText.instance.StopRecording();
        }

        void OnFinalSpeechResult(string result)
        {
            word_store = result;
            VVR.InterpretCommand.Parse(result.ToLower(), false);
        }

        void OnPartialSpeechResult(string result)
        {
            word_store = result;
            VVR.InterpretCommand.Parse(result.ToLower(), false);
        }

    #endregion


        void Setup(string code)
        {
            // TextToSpeech.instance.Setting(code, 1, 1);
            SpeechToText.instance.Setting(code);
        }

        public void Toggle_Changed(bool changed)
        {
            if(changed)
            {
                StartListening();
                Debug.Log("Starting Voice Recording");
            }
            else
            {
                StopListening();
                Debug.Log("Stoping Voice Recording");
            }
        }
    }

}