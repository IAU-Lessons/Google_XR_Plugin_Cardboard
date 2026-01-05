using System;
using UnityEngine;
using UnityEngine.Video;


public class VRManager : MonoBehaviour
{
    [System.Serializable]
    public class VideoUrlData
    {
        public string VideoID;
        public string VideoName;
        public string HighResVideoURL;
        public string LowResVideoURL;
    }

    [System.Serializable]
    public class VideoIDList
    {
        public VideoUrlData[] videoDatas;
    }

    public VideoPlayer videoPlayer;
    public TextAsset videoUrls;

    public VideoUrlData[] datas;

    public int activeVideoIndex;
    public float waitUntil = 10f;
    public bool isPreparingHighRes = false;
    
    void Start()
    {
        activeVideoIndex = 0;
        
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
     
        VideoIDList videoIDList = JsonUtility.FromJson<VideoIDList>(videoUrls.text);
        datas = new VideoUrlData[videoIDList.videoDatas.Length];

        for (int i = 0; i < videoIDList.videoDatas.Length; i++)
        {
            datas[i] = new VideoUrlData
            {
                VideoID = videoIDList.videoDatas[i].VideoID,
                VideoName = videoIDList.videoDatas[i].VideoName,
                HighResVideoURL = videoIDList.videoDatas[i].HighResVideoURL,
                LowResVideoURL = videoIDList.videoDatas[i].LowResVideoURL
            };
        }
        
        TryLoadHighRes();
    }

    private void Update()
    {
        Debug.Log(videoPlayer.isPrepared);

        if (isPreparingHighRes && !videoPlayer.isPrepared)
        {
            waitUntil -= Time.deltaTime;
            if (waitUntil <= 0f)
            {
                isPreparingHighRes = false;
                LoadLowRes();
            }
        }
        
        if (videoPlayer.isPrepared)
        {
            isPreparingHighRes = false;
            videoPlayer.Play();
        }
    }

    public void TryLoadHighRes()
    {
        waitUntil = 10f;
        isPreparingHighRes = true;
        videoPlayer.url = datas[activeVideoIndex].HighResVideoURL;
        videoPlayer.Prepare();
    }

    private void LoadLowRes()
    {
        videoPlayer.url = datas[activeVideoIndex].LowResVideoURL;
        videoPlayer.Prepare();
    }
}
