using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using Whisper;
using Whisper.Utils;

public class DictationController : MonoBehaviour
{
   public WhisperManager whisperManager;
   public MicrophoneRecord micRecord;
   public bool streamSegments = true;
   public bool doneProcessing = false;

   public String resultText = "";
        
   private string _buffer;

   private void Awake()
   {
      whisperManager.OnNewSegment += OnNewSegment;
      whisperManager.OnProgress += OnProgressHandler;

      micRecord.OnRecordStop += OnRecordStop;
   }

   public void toggleRecord()
   {
      // else actually record
      if (!micRecord.IsRecording)
      {
         doneProcessing = false;
         micRecord.StartRecord();
      }
      else
      {
         micRecord.StopRecord();
      }
   }

   private async void OnRecordStop(AudioChunk recordedAudio)
   {
      // when recording stops we process the audio and see what Whisper fucking thinks we're saying
      var res = await whisperManager.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
      if (res == null) return;

      resultText = res.Result;
      doneProcessing = true;
      // TODO: we do something with this text, also make sure we don't accidentally start recording again
   }

   private void OnProgressHandler(int progress)
   {
      // print message to console i promise we're doing something
      print($"processing message: {progress}%");
   }
   
   private void OnNewSegment(WhisperSegment segment)
   {
      // what the fuck is this
      if (!streamSegments)
         return;

      _buffer += segment.Text;
   }
}
