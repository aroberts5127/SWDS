using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon_Audio : MonoBehaviour {

    public float masterVolume;
    public float soundVolume;
    public float musicVolume;

    public void EnemyDeadAudio()
    {
        this.GetComponent<AudioSource>().PlayOneShot(Cannon_Global.Instance.Assets.EnemyDeathSound, .6f * masterVolume * soundVolume);
    }

    public void PickupAudio()
    {
        this.GetComponent<AudioSource>().PlayOneShot(Cannon_Global.Instance.Assets.PickupSound, 1 * masterVolume * soundVolume);
    }

    public void UpdateMasterVolume()
    {
        masterVolume = Cannon_Global.Instance.Presentation.MasterVolumeSlider.value;
        UpdateBGMVolume();
    }

    public void UpdateSFXVolume()
    {
        soundVolume = Cannon_Global.Instance.Presentation.SfxSlider.value;
    }

    public void UpdateMusicVolume()
    {
        musicVolume = Cannon_Global.Instance.Presentation.MusicSlider.value;
        UpdateBGMVolume();
    }

    private void UpdateBGMVolume()
    {
        Cannon_Global.Instance.Assets.BGMSource.volume = 1 * masterVolume * musicVolume;
    }
}
