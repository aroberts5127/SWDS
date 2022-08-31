using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cannon_SaveLoad {

    public static void InitializeSaveData()
    {
        PlayerPrefs.SetInt(Cannon_GlobalSettings.HASPLAYED, 1);
        PlayerPrefs.SetInt(Cannon_GlobalSettings.HIGHSCORE, 0);
        PlayerPrefs.SetFloat(Cannon_GlobalSettings.VOLUME_MASTER, 1f);
        PlayerPrefs.SetFloat(Cannon_GlobalSettings.VOLUME_SOUNDS, 1f);
        PlayerPrefs.SetFloat(Cannon_GlobalSettings.VOLUME_MUSIC, 1f);
        PlayerPrefs.SetString(Cannon_GlobalSettings.ACCTNAME, "");
        PlayerPrefs.SetString(Cannon_GlobalSettings.ACCTID, "");
        PlayerPrefs.SetInt(Cannon_GlobalSettings.ACCTCURRENCY, 0);
    }

    public static void SaveHighScore(int Highscore)
    {
        PlayerPrefs.SetInt(Cannon_GlobalSettings.HIGHSCORE, Highscore);
    }

    public static void SaveVolumeLevels()
    {
        PlayerPrefs.SetFloat(Cannon_GlobalSettings.VOLUME_MASTER, Cannon_Global.Instance.Audio.masterVolume);
        PlayerPrefs.SetFloat(Cannon_GlobalSettings.VOLUME_SOUNDS, Cannon_Global.Instance.Audio.soundVolume);
        PlayerPrefs.SetFloat(Cannon_GlobalSettings.VOLUME_MUSIC, Cannon_Global.Instance.Audio.musicVolume);
    }

    public static void LoadGame()
    {
        Cannon_Global.Instance.Audio.masterVolume = PlayerPrefs.GetFloat(Cannon_GlobalSettings.VOLUME_MASTER);
        Cannon_Global.Instance.Audio.soundVolume = PlayerPrefs.GetFloat(Cannon_GlobalSettings.VOLUME_SOUNDS);
        Cannon_Global.Instance.Audio.musicVolume = PlayerPrefs.GetFloat(Cannon_GlobalSettings.VOLUME_MUSIC);
    }
}
