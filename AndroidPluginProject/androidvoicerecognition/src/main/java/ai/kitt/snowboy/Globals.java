/**
 * Class that holds important global variables for the Snowboy library
 *
 * Notice that apart from dealing with storage directory names, it also holds the name of the
 * .umdl file for the Hotword, and important variables related to the umdl, such as "Sensitivity"
 * and "ApplyFrontEnd"
 *
 * Previously called "Constants"
 */
package ai.kitt.snowboy;

import android.content.Context;

public class Globals {
    public static final String ASSETS_RES_DIR = "snowboy";
    public static final String ACTIVE_RES = "common.res";
    public static final String ACTIVE_UMDL = "view_glass.umdl";
    public static final String ACTIVE_SENSITIVITY = "0.7";
    public static final boolean ACTIVE_APPLYFRONTEND = true;
    public static final int SAMPLE_RATE = 16000;
    public static String DEFAULT_WORK_SPACE;
    public static String SAVE_AUDIO;

    /**
     * Sets the path to the workspace that will contain the .umdl, .res, and the recording files
     *
     * Originally used Environment.getExternalStorageDirectory(), however with API level 29+
     * this method has become deprecated, and now must be done with Context.getExternalFilesDir()
     *
     * @param context the Activity of the app
     */
    public static void setWorkspace(Context context) {
        DEFAULT_WORK_SPACE = context.getExternalFilesDir(null).getAbsolutePath() + "/snowboy/";
        SAVE_AUDIO = DEFAULT_WORK_SPACE + "recording.pcm";
    }
}
