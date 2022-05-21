#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN 
using System.Linq;
using UnityEngine.Windows.Speech;

/*
 * Class that is used by VoiceControlManager.cs script to handle all Windows voice recognition.
 * NOTE: The current implementation is fully continuous and therefore does not require
 * anything other than voice input while a video is playing to perform its commands.
 * 
 * Made by following this tutorial video: https://www.youtube.com/watch?v=29vyEOgsW8s
 */
public class WindowsVoiceControl
{
    private KeywordRecognizer keywordRecognizer;
    private InterpretCommand interpreter;
    
    /// <summary>
    /// Accepts an InterpretCommand objects used by the windows KeywordRecognizer 
    /// </summary>
    /// <param name="ic"></param>
    public WindowsVoiceControl(InterpretCommand ic)
    {
        // Create the recognizer that will use CommandRecognized as the callback/delegate
        interpreter = ic;
        keywordRecognizer = new KeywordRecognizer(interpreter.CommandArray.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += CommandRecognized;
        keywordRecognizer.Start();
    }

    /*
      Callback function used by KeywordRecognizer after speech has been interpreted.
      Note that this function does not use InterpretCommand.cs as KeywordRecognizer already
     "extracts" the desired command from the speech
     */

    /// <summary>
    /// Callback function used by KeywordRecognizer after speech has been interpreted.
    /// </summary>
    /// Note that KeywordRecognizer returns recognized phrases exactly as they are given
    /// to the contructor, so we must indicate that isExactCommand = true
    /// <param name="speech"></param>
    private void CommandRecognized(PhraseRecognizedEventArgs speech)
    {
        // Debug.Log(speech.text);
        interpreter.Parse(speech.text, true);
    }

    /// <summary>
    /// Function used to enable or disable voice recognition. 
    /// Accepts a boolean to either start or stop the keyword recognizer
    /// </summary>
    /// <param name="flag"></param>
    public void ToggleRecognition(bool flag)
    {
        if (flag == false)
        {
            keywordRecognizer.Stop();
        }
        else if (flag == true)
        {
            keywordRecognizer.Start();
        }
    }
}
#endif

