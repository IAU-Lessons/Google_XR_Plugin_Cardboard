using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VoskResultText : MonoBehaviour 
{
    public VoskSpeechToText VoskSpeechToText;

    public VideoPlayer videoPlayer;
    public VRManager vrManager;
    
    public bool isPhraseRecognized = false;

    private void Awake()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
    }

    private void Start()
    {
        vrManager = FindAnyObjectByType<VRManager>();
        videoPlayer = FindAnyObjectByType<VideoPlayer>();
    }

    private void OnTranscriptionResult(string obj)
    {
        var result = new RecognitionResult(obj);
        
        if (obj.ToLower().Contains("video durdur") && !isPhraseRecognized)
        {
            Debug.Log("Durdur");
            videoPlayer.playbackSpeed = 0f;
            isPhraseRecognized = true;

            VoskSpeechToText.ResetRecognizer();
            StartCoroutine(UnlockPhrase());
        }
        
        else if (obj.ToLower().Contains("video başlat"))
        {
            Debug.Log("başlat");

            videoPlayer.playbackSpeed = 1f;
            isPhraseRecognized = true;
            
            VoskSpeechToText.ResetRecognizer();
            StartCoroutine(UnlockPhrase());
        }
        
        else if (obj.ToLower().Contains("video sonraki"))
        {
            vrManager.activeVideoIndex++;
            vrManager.TryLoadHighRes();
            isPhraseRecognized = true;
            
            VoskSpeechToText.ResetRecognizer();
            StartCoroutine(UnlockPhrase());
        }
        
        else if (obj.ToLower().Contains("video önceki"))
        {
            vrManager.activeVideoIndex--;
            vrManager.TryLoadHighRes();
            isPhraseRecognized = true;
            
            VoskSpeechToText.ResetRecognizer();
            StartCoroutine(UnlockPhrase());
        }
        
        if (result.Partial)
        {
            if(result.Phrases == null || result.Phrases.Length == 0 || string.IsNullOrEmpty(result.Phrases[0].Text))
            {
                return; 
            }
            Debug.Log("Algılandı: " + result.Phrases[0].Text);
        }
        else
        {
            if(result.Phrases != null && result.Phrases.Length > 0 && !string.IsNullOrEmpty(result.Phrases[0].Text))
            {
                Debug.Log("Algılandı: " + result.Phrases[0].Text);
            }
        }
    }
    
    IEnumerator UnlockPhrase()
    {
        yield return new WaitForSeconds(0.6f);
        isPhraseRecognized = false;
    }
}