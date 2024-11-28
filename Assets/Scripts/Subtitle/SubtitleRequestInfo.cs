using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SubtitleRequestInfo
{
    public List<string> subtitles;
    public int fontSize;
    public float subtitlesDelay;
    public Transform parent;
    public Vector2 position;
    public Vector2 size;
    public Vector2 offset;
    public AudioClip[] typeSound;
    public UnityAction callBack;
    public SubtitleRequestInfo() 
    {
        subtitles = new List<string>();
        fontSize = 36;
        subtitlesDelay= 0.1f;
        parent = null;
        position = Vector2.zero;
        size = new Vector2(240,60);
        offset = new Vector2(0,80);
        typeSound = null;
        callBack = null;
    }
}
