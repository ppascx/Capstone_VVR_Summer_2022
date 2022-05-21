/*
SpeechRecorderViewController.m plugin
this code was originally taken from the unity project:https://github.com/PingAK9/Speech-And-Text-Unity-iOS-Android
and then modified using other sources in order to make it continuous:
https://stackoverflow.com/questions/43834119/how-to-implement-speech-to-text-via-speech-framework
https://stackoverflow.com/questions/37821826/continuous-speech-recogn-with-sfspeechrecognizer-ios10-beta

This plugin initializes the recording and services necessary for the translation of speech
to a string. The speech service stops upon recognition of a word, so this code restarts these
services automatically. It also had different code in case on-device recognition is supported.
This plugin sends a message to a unity game object to return the result of the translation.
*/
#import "SpeechRecorderViewController.h"
#import <Speech/Speech.h>

@interface SpeechRecorderViewController ()
{
   // Speech recognize
   SFSpeechRecognizer *speechRecognizer;
   SFSpeechAudioBufferRecognitionRequest *recognitionRequest;
   SFSpeechRecognitionTask *recognitionTask;

   AVAudioSession *session;

   // Record speech using audio Engine
   AVAudioInputNode *inputNode;
   AVAudioEngine *audioEngine;
   NSString * LanguageCode;

   /*used to restart on-device voice recognition after a given amount of time*/
   NSTimer *onDeviceRecognitionTimer;

   /*used to end recognition after a given amount of time*/
   NSTimer *cancelAfterInputTimer;

   /*string where result from translation is stored*/
   NSString *transcriptText;

}
@end

@implementation SpeechRecorderViewController

- (id)init
{
   self = [super init];

   audioEngine = [[AVAudioEngine alloc] init];
   LanguageCode = @"en-US";
   NSLocale *local =[[NSLocale alloc] initWithLocaleIdentifier:LanguageCode];
   speechRecognizer = [[SFSpeechRecognizer alloc] initWithLocale:local];

   // Check Authorization Status
   // Make sure you add "Privacy - Microphone Usage Description" key and reason in Info.plist to request micro permison
   // And "NSSpeechRecognitionUsageDescription" key for requesting Speech recognize permison
   [SFSpeechRecognizer requestAuthorization:^(SFSpeechRecognizerAuthorizationStatus status) {
      //The callback may not be called on the main thread. Add an operation to the main queue to update the record button's state.
      dispatch_async(dispatch_get_main_queue(), ^{
         switch (status) {
            case SFSpeechRecognizerAuthorizationStatusAuthorized: {
               NSLog(@"SUCCESS");
               break;
            }
            case SFSpeechRecognizerAuthorizationStatusDenied: {
               NSLog(@"User denied access to speech recognition");
               break;
            }
            case SFSpeechRecognizerAuthorizationStatusRestricted: {
               NSLog(@"User denied access to speech recognition");
               break;
            }
            case SFSpeechRecognizerAuthorizationStatusNotDetermined: {
               NSLog(@"User denied access to speech recognition");
               break;
            }
         }
      });

   }];
   return self;
}

/*
set the settings for speech recognition. This is hardcoded in unity for simplicity sake.
*/
- (void)SettingSpeech: (const char *) _language
{
   LanguageCode = [NSString stringWithUTF8String:_language];
   NSLocale *local =[[NSLocale alloc] initWithLocaleIdentifier:LanguageCode];
   speechRecognizer = [[SFSpeechRecognizer alloc] initWithLocale:local];
}

/*
Creates access to microphone, starts a recording session and calls either on-device or
server recognition. This function is called only when the app is first launched or when
the user disables voice recognition. Calling this function again results in a small lag
on the video player.
*/
- (void)startRecording
{
   if (!audioEngine.isRunning)
   {
       [inputNode removeTapOnBus:0];
      if (recognitionTask)
      {
         [recognitionTask cancel];
         recognitionTask = nil;
      }

      session = [AVAudioSession sharedInstance]; //delete
      [session setCategory:AVAudioSessionCategoryPlayAndRecord mode:AVAudioSessionModeMeasurement options:AVAudioSessionCategoryOptionDefaultToSpeaker error:nil];
      [session setActive:TRUE withOptions:AVAudioSessionSetActiveOptionNotifyOthersOnDeactivation error:nil];

      if (inputNode == nil) {
         inputNode = audioEngine.inputNode;
      }

      recognitionRequest = [[SFSpeechAudioBufferRecognitionRequest alloc] init];

      AVAudioFormat *format = [inputNode outputFormatForBus:0];
      [inputNode installTapOnBus:0 bufferSize:1024 format:format block:^(AVAudioPCMBuffer * _Nonnull buffer, AVAudioTime * _Nonnull when) {
         [recognitionRequest appendAudioPCMBuffer:buffer];
      }];

      [audioEngine prepare];
      NSError *error1;
      [audioEngine startAndReturnError:&error1];
      NSLog(@"errorAudioEngine.description: %@", error1.description);

      recognitionRequest.shouldReportPartialResults = YES; //important for server recognition

      if (speechRecognizer.supportsOnDeviceRecognition) //set to false so it always uses server recognition
      {
         recognitionRequest.shouldReportPartialResults = NO; 
         recognitionRequest.requiresOnDeviceRecognition = YES;
         [self startOnDevice];
      }
      else    //if on-device recognition is not supported
      {
         [self startOnServer];
      }
   }//end if (!audioEngine.isRunning)
}// end startRecording


/*
Completely cancels the recognition service and recording. This is used by the toggle on
unity. After using this function, StartRecognition needs to be called again to initializ
most of the objects
*/
- (void)cancelRecording
{
   recognitionTask = nil;
   recognitionRequest = nil;
   [onDeviceRecognitionTimer invalidate];
   onDeviceRecognitionTimer = nil;
   [inputNode removeTapOnBus:0];
   [audioEngine stop];
   [recognitionRequest endAudio];
   UnitySendMessage("SpeechToText", "LogMessage", "recording has been stopped by user");
}
/*
Prints transcript string to console and sends it to Unity to be parsed
*/
-(void)returnResult
{
   NSLog(@"ONDEVICE RECOGNITION RESULT: %@!", transcriptText);
   UnitySendMessage("SpeechToText", "onResults", [transcriptText UTF8String]);
}

/*
Stop the recording and restart the service. Its called by a timer that stops 0.8 seconds
after voice input stops. Service restarted depends on whether the device supports
on-device or server recognition
*/
-(void)stopRecording:(NSTimer *)theTimer
{
   [self returnResult];
   [recognitionRequest endAudio];
   recognitionRequest = nil;
   [cancelAfterInputTimer invalidate];
   cancelAfterInputTimer = nil;

   if (speechRecognizer.supportsOnDeviceRecognition)
   {
      [self startOnDevice];
   }
   else
   {
      [self startOnServer];
   }
}

/*
used for on-device recognition. It is called by the timer after the amount of time
specified is reached.
*/
-(void)stopRecordingOnDevice:(NSTimer *)theTimer
{
   [recognitionRequest endAudio];
   recognitionRequest = nil;
   [self startOnDevice];
}

/*
Starts on-device recognition. If after 8 seconds the recognition service does not detect
a word, the recognitino service is stopped and restarted. This is to avoid lag on
responsiveness caused by leaving the on-device recognition service running for too long
*/
-(void)startOnDevice
{
   if (recognitionRequest == nil) {
      recognitionRequest = [[SFSpeechAudioBufferRecognitionRequest alloc] init];
   }

   //after 8 sec the plugin will call this function, stopped only if recognizer identifies a word
   onDeviceRecognitionTimer = [NSTimer scheduledTimerWithTimeInterval: 8.00 target: self selector:@selector(stopRecordingOnDevice:) userInfo:nil repeats:false];

   recognitionTask =[speechRecognizer recognitionTaskWithRequest:recognitionRequest resultHandler:^(SFSpeechRecognitionResult * _Nullable result, NSError * _Nullable error)
   {
      if (result != nil) {
         [onDeviceRecognitionTimer invalidate];
         onDeviceRecognitionTimer = nil;

         if (cancelAfterInputTimer != nil) {
            [cancelAfterInputTimer invalidate];
            cancelAfterInputTimer = nil;
         }
         cancelAfterInputTimer = [NSTimer scheduledTimerWithTimeInterval: 0.70 target: self selector:@selector(stopRecording:) userInfo:nil repeats:false];
         transcriptText = result.bestTranscription.formattedString;
      }
   }];
}


/*
Starts server recognition. After 1 minute, server recognition stops automatically due to
a limitation of the API. However, this is circumvented by checking for the state of the
recognition and restarting the service when the timeout is encountered
*/
-(void)startOnServer
{
   if (recognitionRequest == nil) {
      recognitionRequest = [[SFSpeechAudioBufferRecognitionRequest alloc] init];
   }

   recognitionTask =[speechRecognizer recognitionTaskWithRequest:recognitionRequest resultHandler:^(SFSpeechRecognitionResult * _Nullable result, NSError * _Nullable error)
   {
      if (result != nil)
      {
         if (cancelAfterInputTimer != nil) {
            [cancelAfterInputTimer invalidate];
            cancelAfterInputTimer = nil;
         }
         cancelAfterInputTimer = [NSTimer scheduledTimerWithTimeInterval: 0.70 target: self selector:@selector(stopRecording:) userInfo:nil repeats:false];

         transcriptText = result.bestTranscription.formattedString;
      }
      else
      {
         //if timeout to the server, restart
         if (recognitionTask.state == 4)
         {
            recognitionTask = nil;
            NSLog(@"SERVER RECOGNITION RESULT NULL");
            NSLog(@"SERVER RECOGNITION RESTARTING...");

            if (cancelAfterInputTimer != nil) {
               [cancelAfterInputTimer invalidate];
               cancelAfterInputTimer = nil;
            }
            cancelAfterInputTimer = [NSTimer scheduledTimerWithTimeInterval: 0.1 target: self selector:@selector(stopRecording:) userInfo:nil repeats:false];
         }
      }
   }];
}
@end

/*Functions used by c#
these are the declarations of the functions can be used on the c# plugin on unity
*/
extern "C"{
   SpeechRecorderViewController *vc = [[SpeechRecorderViewController alloc] init];
   void _TAG_startRecording(){
      [vc startRecording];
   }
   void _TAG_cancelRecording(){
      [vc cancelRecording];
   }
   void _TAG_SettingSpeech(const char * _language){
      [vc SettingSpeech:_language];
   }

}
