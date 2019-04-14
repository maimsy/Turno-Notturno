using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoArt : MonoBehaviour
{
    void Start()
    {
        // Will attach a VideoPlayer to the main camera.
        GameObject camera = Camera.main.gameObject;
        var videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCameraAlpha = 0.5F;
        videoPlayer.url = "C:/Users/Teo/Desktop/Turno-Notturno/turno-notturno/Assets/videoArt1.mp4";
        videoPlayer.frame = 100;
        videoPlayer.isLooping = true;
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.Play();
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;
    }
}
