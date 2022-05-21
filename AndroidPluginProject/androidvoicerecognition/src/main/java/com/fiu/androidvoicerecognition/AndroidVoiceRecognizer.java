package com.fiu.androidvoicerecognition;

import android.content.Intent;
import android.os.Bundle;
import android.speech.RecognitionListener;
import android.speech.RecognizerIntent;
import android.speech.SpeechRecognizer;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import ai.kitt.snowboy.AppResCopy;
import ai.kitt.snowboy.Globals;
import ai.kitt.snowboy.audio.AudioDataSaver;
import ai.kitt.snowboy.audio.RecordingThread;

import java.util.Locale;

public class AndroidVoiceRecognizer {

    private SpeechRecognizer speechRec;
    private RecordingThread snowboyThread;
    private Intent speechIntent;
    private static final String TAG = AndroidVoiceRecognizer.class.getSimpleName();
    private boolean haveResult; // sentinel to ensure onResults runs only once
    /* 3-31-20: haveResult has been added as a temporary fix to a recent error in the "Speech Recognition"
    service bug reported on several forums: https://issuetracker.google.com/issues/152628934,
    https://stackoverflow.com/questions/60853257/android-recognitionlistener-onresults-being-called-twice */

    /**
     * Initializes the voice recognition class. Note: This constructor does not start recording
     * the user's voice. An explicit call to RecordingThread.startRecording() is required.
     *
     * IMPORTANT: Assumes permissions are checked on the Unity side using
     * UnityEngine.Android.Permission methods
     *
     * UnityPlayer.currentActivity is the Unity app's Activity
     * For more information, see https://developer.android.com/reference/android/app/Activity
     */
    public AndroidVoiceRecognizer() {
        // Set storage paths using the Activity's information of the device
        Globals.setWorkspace(UnityPlayer.currentActivity);

        // Copy snowboy assets (umdl, res, etc.) to shared/external storage
        AppResCopy.copyResFromAssetsToExt(UnityPlayer.currentActivity);

        initSpeechRecognizer();

        // Create RecognizerIntent for speechRecognizer, for more information visit:
        // https://developer.android.com/reference/android/speech/RecognizerIntent
        speechIntent = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
        // "Language Model" = "Freeform"
        speechIntent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL, RecognizerIntent.LANGUAGE_MODEL_FREE_FORM);
        // "Language" = "English"
        speechIntent.putExtra(RecognizerIntent.EXTRA_LANGUAGE, Locale.ENGLISH);
        // "Maximum Results" = 1; we are trusting the recognition engine to give us the best result first
        speechIntent.putExtra(RecognizerIntent.EXTRA_MAX_RESULTS, 1);

        snowboyThread = new RecordingThread(new AudioDataSaver(), this);
    }

    /**
     * Initialize's the Android SpeechRecognizer object.
     *
     * The use of runOnUiThread is because "SpeechRecognizer class's methods must be invoked only
     * from the main application (UI) thread". For more information, read bitter's replies in this thread:
     * https://forum.unity.com/threads/android-plugin-problem.181399/ as well as the official docs:
     * https://developer.android.com/reference/android/speech/SpeechRecognizer.html
     */
    private void initSpeechRecognizer()
    {
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                // Create new speech recognizer using the Unity app's Activity as context
                speechRec = SpeechRecognizer.createSpeechRecognizer(UnityPlayer.currentActivity);

                // Set the event handler to an "anonymous" AVRecognitionListener object
                speechRec.setRecognitionListener(new AVRecognitionListener());
            }
        });
    }

    /**
     * Stops listening for the Hotword and passes control to the built-in SpeechRecognizer
     * to interpret the user's voice input
     *
     * Called by methods in RecordingThread.java
     */
    public void startSpeechRecognizer() {
        // stop Hotword recognition
        snowboyThread.stopRecording();

        // we need a result from SpeechRecognizer
        haveResult = false;

        // pass control
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                //Log.d(TAG, "SpeechRecognizer listening on UI thread");
                speechRec.startListening(speechIntent);
            }
        });
    }

    /**
     * Used by the C# Unity side to start/stop recording during certain events
     * e.g. First startup, app in background, searching for a file, etc.
     */
    public void startRecording() {
        snowboyThread.startRecording();
    }

    public void stopRecording() {
        snowboyThread.stopRecording();
    }

    /**
     * Serves as an "event handler" for a speech recognition request placed by SpeechRecognizer
     */
    private class AVRecognitionListener implements RecognitionListener {

        // The name of the "GameObject" in Unity that contains VoiceControlManager.cs
        private final String VC_GAMEOBJECT = "VoiceControlManager";

        @Override
        public void onReadyForSpeech(Bundle params) {}

        @Override
        public void onBeginningOfSpeech() {}

        @Override
        public void onRmsChanged(float rmsdB) {}

        @Override
        public void onBufferReceived(byte[] buffer) {}

        @Override
        public void onEndOfSpeech() {}

        @Override
        public void onError(int error) {
            if (error != SpeechRecognizer.ERROR_SPEECH_TIMEOUT) {
                Log.e(TAG, "RecognitionListener error code: " + error);
            } // Timeout is common, no real need to log

            // start Hotword recognition again
            snowboyThread.startRecording();
        }

        /**
         * On successful interpretation of voice input, gets the 0th result of recognition and
         * calls the "Result" function of VoiceControl.cs
         *
         * NOTE: If more possible results are desired, the EXTRA_MAX_RESULTS option of
         * the RecognizerIntent initialized in AndroidVoiceRecognizer.java will have to be
         * modified.
         * @param results the speech recognition results
         */
        @Override
        public void onResults(Bundle results) {
            // If we have a result, we don't need another one
            if (haveResult)
                return;
            else
                haveResult = true;

            // send interpreted text message to designated Voice Control GameObject/Script
            String message = results.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION).get(0);
            UnityPlayer.UnitySendMessage(VC_GAMEOBJECT, "Result", message);
            // Log.d(TAG, "Interpreted message: " + message);

            // start Hotword recognition again
            snowboyThread.startRecording();
        }

        @Override
        public void onPartialResults(Bundle partialResults) {}

        @Override
        public void onEvent(int eventType, Bundle params) {}
    }
}
