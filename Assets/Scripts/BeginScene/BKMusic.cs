using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instance=>instance;

    private AudioSource bkSource;

    private void Awake()
    {
        instance = this;
        bkSource = GetComponent<AudioSource>();

        MusicData data = GameDataMgr.Instance.musicData;
        SetPlaying(data.musicIsPlaying);
        ChangeVolume(data.musicVolume);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlaying(bool isPlaying)
    {
        bkSource.mute = !isPlaying;
    }

    public void ChangeVolume(float volume)
    {
        bkSource.volume = volume;

        
    }
}
