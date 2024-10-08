using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : PanelBase
{
    public Button closeBtn;
    public Toggle musicToggle;
    public Toggle soundToggle;
    public Slider musicSlider;
    public Slider soundSlider;

    protected override void Init()
    {

        musicSlider.value = GameDataMgr.Instance.musicData.musicVolume;
        soundSlider.value = GameDataMgr.Instance.musicData.soundVolume;
        musicToggle.isOn = GameDataMgr.Instance.musicData.musicIsPlaying;
        soundToggle.isOn = GameDataMgr.Instance.musicData.soundIsPlaying;

        closeBtn.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.SaveMusicData();
            UIManager.Instance.HidePanel();
        });

        musicToggle.onValueChanged.AddListener((value) =>
        {
            BKMusic.Instance.SetPlaying(value);
            GameDataMgr.Instance.musicData.musicIsPlaying = value;
        });
        musicSlider.onValueChanged.AddListener((value) =>
        {
            BKMusic.Instance.ChangeVolume(value);
            GameDataMgr.Instance.musicData.musicVolume = value;
        });

        soundToggle.onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.musicData.soundIsPlaying = value;
        });
        soundSlider.onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.musicData.soundVolume = value;
        });

    }
}

