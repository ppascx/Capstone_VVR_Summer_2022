package ai.kitt.snowboy.audio;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;

import ai.kitt.snowboy.Globals;
import android.media.AudioFormat;
import android.media.AudioRecord;
import android.media.MediaRecorder;
import android.util.Log;

import com.fiu.androidvoicerecognition.AndroidVoiceRecognizer;

import ai.kitt.snowboy.SnowboyDetect;

public class RecordingThread {
    static { System.loadLibrary("snowboy-detect-android"); }

    private static final String TAG = RecordingThread.class.getSimpleName();

    private static final String ACTIVE_RES = Globals.ACTIVE_RES;
    private static final String ACTIVE_UMDL = Globals.ACTIVE_UMDL;

    private static final int MAX_RECORD_ATTEMPTS = 10;
    
    private boolean shouldContinue;
    private AudioDataReceivedListener listener = null;
    private Thread thread;
    private AndroidVoiceRecognizer androidVoiceRecognizer;
    
    private String strEnvWorkSpace = Globals.DEFAULT_WORK_SPACE;
    private String activeModel = strEnvWorkSpace+ACTIVE_UMDL;    
    private String commonRes = strEnvWorkSpace+ACTIVE_RES;   
    
    private SnowboyDetect detector = new SnowboyDetect(commonRes, activeModel);

    public RecordingThread(AudioDataReceivedListener listener, AndroidVoiceRecognizer recognizer) {
        this.listener = listener;
        androidVoiceRecognizer = recognizer;

        detector.SetSensitivity(Globals.ACTIVE_SENSITIVITY);
        detector.SetAudioGain(1);
        detector.ApplyFrontend(Globals.ACTIVE_APPLYFRONTEND);
    }

    public void startRecording() {
        if (thread != null)
            return;

        shouldContinue = true;
        thread = new Thread(new Runnable() {
            @Override
            public void run() {
                record();
            }
        });
        thread.start();
    }

    public void stopRecording() {
        if (thread == null)
            return;

        shouldContinue = false;
        thread = null;
    }

    private void record() {
        android.os.Process.setThreadPriority(android.os.Process.THREAD_PRIORITY_AUDIO);

        // Buffer size in bytes: for 0.1 second of audio
        int bufferSize = (int)(Globals.SAMPLE_RATE * 0.1 * 2);
        if (bufferSize == AudioRecord.ERROR || bufferSize == AudioRecord.ERROR_BAD_VALUE) {
            bufferSize = Globals.SAMPLE_RATE * 2;
        }

        byte[] audioBuffer = new byte[bufferSize];
        AudioRecord record = new AudioRecord(
            MediaRecorder.AudioSource.DEFAULT,
            Globals.SAMPLE_RATE,
            AudioFormat.CHANNEL_IN_MONO,
            AudioFormat.ENCODING_PCM_16BIT,
            bufferSize);

        if (record.getState() != AudioRecord.STATE_INITIALIZED) {
            Log.e(TAG, "Audio Record can't initialize!");
            return;
        }

        int recordAttempts = 1;
        record.startRecording();

        /* Sometimes the "handoff" of the microphone from SpeechRecognizer to Snowboy takes longer
        than expected, so we attempt to reacquire the mic several times to give SpeechRecognizer enough
        time to release the microphone */
        while(record.getRecordingState() != AudioRecord.RECORDSTATE_RECORDING
                && recordAttempts < MAX_RECORD_ATTEMPTS) {
            Log.w(TAG, "Unable to obtain recording device, reattempting in 100ms");
            try {
                Thread.sleep(100);
                record.startRecording();
            } catch (Exception e) {
                e.printStackTrace();
            }
            recordAttempts++;
        }

        if (recordAttempts >= MAX_RECORD_ATTEMPTS)
        {
            // Unusual amount of time to acquire mic, abort Snowboy
            Log.e(TAG, "CRITICAL ERROR: Snowboy was unable to gain access to the device microphone, stopping Snowboy.");
            stopRecording();
        } // else Log.i(TAG, "Snowboy has obtained the device microphone successfully");

        if (null != listener) {
            listener.start();
        }
        // Log.d(TAG, "Started recording");

        long shortsRead = 0;
        detector.Reset();
        while (shouldContinue) {
            record.read(audioBuffer, 0, audioBuffer.length);

            if (null != listener) {
                listener.onAudioDataReceived(audioBuffer, audioBuffer.length);
            }
            
            // Converts to short array.
            short[] audioData = new short[audioBuffer.length / 2];
            ByteBuffer.wrap(audioBuffer).order(ByteOrder.LITTLE_ENDIAN).asShortBuffer().get(audioData);

            shortsRead += audioData.length;

            // Snowboy hotword detection.
            int result = detector.RunDetection(audioData, audioData.length);

            if (result > 0) {
                //Log.d(TAG, "Hotword has been detected! Result: " + result);
                androidVoiceRecognizer.startSpeechRecognizer();
            }

            /* Previously, the code above featured enum types MSG_VAD_NOSPEECH, MSG_ERROR,
            and MSG_VAD_SPEECH for result == -2, -1, and 0 respectively. We leave this comment in
            the event there is ever a need for the other result types. */
        }
        record.stop();
        record.release();

        if (null != listener) {
            listener.stop();
        }
        // Log.d(TAG, String.format("Recording stopped. Samples read: %d", shortsRead));
    }
}
