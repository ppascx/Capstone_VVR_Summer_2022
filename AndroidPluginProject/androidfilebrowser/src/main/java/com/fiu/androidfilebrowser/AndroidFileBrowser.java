package com.fiu.androidfilebrowser;

import android.content.Intent;

import com.unity3d.player.UnityPlayer;

public class AndroidFileBrowser
{
    private static final String TAG = AndroidFileBrowser.class.getSimpleName();
    private Intent FSResultActivityIntent;

    /**
     * Checks currentActivity and initializes an intent to start a FileSearchResultActivity
     *
     * UnityPlayer.currentActivity is the Unity app's Activity
     * For more information, see https://developer.android.com/reference/android/app/Activity
     */
    public AndroidFileBrowser()
    {
        if(UnityPlayer.currentActivity == null)
        {
            // Log.i(TAG, "currentActivity is null");
            return;
        }

        // Initialize intent to start a new Activity of type "FileSearchResultActivity" using
        // Unity's activity as the context
        FSResultActivityIntent = new Intent(UnityPlayer.currentActivity, FileSearchResultActivity.class);

    }

    /**
     * Starts a FileSearchResultActivity with the previously initialized intent
     *
     * Although this method's logic could exist within the constructor of AndroidFileBrowser,
     * this organization allows for a persistent AndroidFileBrowser object with important
     * runtime variables to exist persistently in C# while allowing the browser-opening action
     * to be done at some later time.
     */
    public void openBrowser()
    {
        // Starts FileSearchResultActivity
        UnityPlayer.currentActivity.startActivity(FSResultActivityIntent);
    }
}
