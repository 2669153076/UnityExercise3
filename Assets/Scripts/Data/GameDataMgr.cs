using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理数据的类
/// </summary>
public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance=>instance;

    public MusicData musicData; //音效相关数据
    public PlayerData playerData;   //玩家数据
    public List<RoleInfo> roleInfoList; //游戏角色数据
    public List<SceneInfo> sceneInfoList;   //地图数据
    public List<EnemyInfo> enemyInfoList;   //怪物数据
    public List<TowerInfo> towerInfoList;   //炮台数据

    public RoleInfo curSelRole; //记录当前选择的角色

    private GameDataMgr()
    {
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        enemyInfoList = JsonMgr.Instance.LoadData<List<EnemyInfo>>("EnemyInfo");
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");

    }

    /// <summary>
    /// 存储音乐、音效相关数据
    /// 音量大小
    /// 是否静音
    /// </summary>
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }

    /// <summary>
    /// 存储玩家数据
    /// </summary>
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }
   
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="resName">音效资源路径</param>
    public void PlaySound(string resName)
    {
        GameObject musicObj = new GameObject();
        AudioSource a = musicObj.AddComponent<AudioSource>();
        a.clip = Resources.Load<AudioClip>(resName);
        a.volume = musicData.soundVolume;
        a.mute = !musicData.soundIsPlaying;
        a.Play();

        GameObject.Destroy(musicObj, 1);
    }

}