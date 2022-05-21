package com.fiu.androidfilebrowser;

import android.app.Activity;
import android.content.Intent;
import android.database.Cursor;
import android.net.Uri;
import android.os.Bundle;
import android.provider.OpenableColumns;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import java.io.File;
import java.io.FileOutputStream;
import java.io.InputStream;

/**
 * Activity created by the AndroidFileBrowser class to start the built-in Android File Browser "Activity",
 * process it's output (path of the user-selected file), and pass it back to LoadVideoURL.SetPath (C# script)
 *
 * In a way, FileSearchResultActivity is both a "dispatcher" for the actual built-in file browser,
 * but also a "listener" for the result it returns.
 */
public class FileSearchResultActivity extends Activity
{
    private final String TAG = FileSearchResultActivity.class.getSimpleName();
    private final int READ_REQUEST_CODE = 42;   // currently unused, but good to have if ever needed
    private final boolean OVERWRITE_CACHE = false;

    @Override
    protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);

        //Create a File Search activity and wait for a result
        try
        {
            // ACTION_OPEN_DOCUMENT is the intent to choose a file via the system's file browser.
            Intent fileSearchIntent = new Intent(Intent.ACTION_OPEN_DOCUMENT);

            // Filter to only show results that can be "opened", such as a file
            fileSearchIntent.addCategory(Intent.CATEGORY_OPENABLE);

            // Filter to show only videos, using the video MIME data type.
            fileSearchIntent.setType("video/*");

            startActivityForResult(fileSearchIntent, READ_REQUEST_CODE);
        }
        catch (Exception e)
        {
            e.printStackTrace();
            Log.e(TAG, "Error: " + e.getLocalizedMessage());

            // Terminate FileSearchResultActivity activity
            finish();
        }
    }

    /**
     * Handles the result of the ACTION_OPEN_DOCUMENT (fileSearchIntent) activity started previously
     *
     * @param requestCode The integer request code originally supplied to startActivityForResult(),
     *                    allowing you to identify who this result came from
     * @param resultCode The integer result code returned by the child activity through its setResult()
     * @param data An Intent, which can return result data to the caller (various data can be attached
     *             to Intent "extras")
     */
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data)
    {
        switch (resultCode)
        {
            case RESULT_OK:
                // Log.i(TAG, "resultCode: " + resultCode + " = OK" );
                if (data != null)
                {
                    Uri pickedUri = data.getData();
                    // Log.i(TAG, "pickedUri: " + pickedUri.toString());

                    String cachedFilePath = getCachePathFromURI(pickedUri);
                    // Log.i(TAG, "Cached file path: " + cachedFilePath);

                    if (cachedFilePath != null)
                    {
                        // Pass filepath back to method "SetFilePath" in GameObject "FileLoader"'s script
                        UnityPlayer.UnitySendMessage("FileLoader", "SetFilePath", cachedFilePath);
                    }
                    else Log.e(TAG, "Error: Unable to get file from cache");
                }
                else Log.e(TAG, "Error: Bad result data");
                break;
            case RESULT_CANCELED:
                Log.w(TAG, "No file has been selected" );
                break;
            default:
                // Log.i(TAG, "resultCode = IDK" );
                break;
        }

        // Terminate FileSearchResultActivity activity
        finish();
    }

    /**
     * Obtains the file's "display name" from the given URI
     *
     * Because the display name is provider-specific, there is no guarantee it is the file name
     * For more info, read https://developer.android.com/guide/topics/providers/content-provider-basics
     *
     * @param uri the given URI
     * @return the display name from the URI
     */
    private String getDisplayNameFromURI(Uri uri)
    {
        String displayName = null;
        Cursor cursor = getContentResolver().query(uri, null, null, null, null, null);
        if (cursor != null && cursor.moveToFirst())
        {
            displayName = cursor.getString(cursor.getColumnIndex(OpenableColumns.DISPLAY_NAME));
            // Log.i(TAG, "Display Name: " + displayName);
            cursor.close();
        }
        return displayName;
    }

    /**
     * Parses the URI into a display name for use as a filename and checks for its existence in cache.
     * If it exists, then the path to the file is returned. If it does not exist (or should be overwritten)
     * then the file is copied to cache and its path is returned.
     *
     * Note: This method does not need READ_EXTERNAL_STORAGE permission because we are using getCacheDir
     * (which implicitly belongs to the app) to do our file manipulation.
     *
     * A previous implementation of this URI parser used logic involving ContentResolver.query
     * and MediaStore.MediaColumns.DATA. The reason for the change was twofold: the DATA constant
     * is being deprecated in Android 10+ and there is no guarantee it would provide you an accessible
     * path on all versions of Android, regardless of permissions.
     *
     * @param uri the given URI
     * @return a usable file path for Unity
     */
    private String getCachePathFromURI(Uri uri)
    {
        try
        {
            File cachedFile = new File(getCacheDir().getAbsolutePath() + "/" +  getDisplayNameFromURI(uri));

            if (cachedFile.exists())
            {
                if (OVERWRITE_CACHE)
                {
                    // Log.i(TAG, "Overwriting old file in cache.");
                    cachedFile.delete();
                }
                else
                {
                    Log.w(TAG, "File exists in cache, no overwrite.");
                    return cachedFile.getAbsolutePath();
                }
            }

            // Copy file to cache
            InputStream is = getContentResolver().openInputStream(uri);
            FileOutputStream fos = new FileOutputStream(cachedFile);
            byte[] buffer = new byte[1024];
            int byteCount=0;
            while ((byteCount=is.read(buffer)) != -1) {
                fos.write(buffer, 0, byteCount);
            }
            fos.flush();
            is.close();
            fos.close();

            // Log.i(TAG, "File copied to cache successfully.");

            // Return the path to the file in cache
            return cachedFile.getAbsolutePath();
        }
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}
