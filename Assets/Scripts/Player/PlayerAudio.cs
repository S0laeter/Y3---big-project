using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip playerHit;

    public AudioClip basic1;
    public AudioClip basic2;
    public AudioClip basic3;
    public AudioClip basic4;
    public AudioClip basic4_1;
    public AudioClip basic5;
    public AudioClip basic5_1;

    public AudioClip heavyCharging;
    public AudioClip heavyRelease;
    public AudioClip heavyRelease_1;

    public AudioClip skillCharging;
    public AudioClip skillCharging_1;
    public AudioClip skill1;
    public AudioClip skill2;
    public AudioClip skill2_1;

    public AudioClip ult1;
    public AudioClip ult2;
    public AudioClip ult3;

    public AudioClip air1;
    public AudioClip air2;
    public AudioClip plungeLand;

    public AudioClip dash;
    public AudioClip jump;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudioClip(string audioName)
    {
        switch (audioName)
        {
            case "playerHit":
                SoundManager.instance.PlaySoundClip(playerHit, this.transform, 1f);
                break;
            case "basic1":
                SoundManager.instance.PlaySoundClip(basic1, this.transform, 1f);
                break;
            case "basic2":
                SoundManager.instance.PlaySoundClip(basic2, this.transform, 1f);
                break;
            case "basic3":
                SoundManager.instance.PlaySoundClip(basic3, this.transform, 1f);
                break;
            case "basic4":
                SoundManager.instance.PlaySoundClip(basic4, this.transform, 1f);
                break;
            case "basic4.1":
                SoundManager.instance.PlaySoundClip(basic4_1, this.transform, 1f);
                break;
            case "basic5":
                SoundManager.instance.PlaySoundClip(basic5, this.transform, 1f);
                break;
            case "basic5.1":
                SoundManager.instance.PlaySoundClip(basic5_1, this.transform, 1f);
                break;
            case "heavyCharging":
                SoundManager.instance.PlaySoundClip(heavyCharging, this.transform, 1f);
                break;
            case "heavyRelease":
                SoundManager.instance.PlaySoundClip(heavyRelease, this.transform, 1f);
                break;
            case "heavyRelease.1":
                SoundManager.instance.PlaySoundClip(heavyRelease_1, this.transform, 1f);
                break;
            case "skillCharging":
                SoundManager.instance.PlaySoundClip(skillCharging, this.transform, 1f);
                break;
            case "skillCharging.1":
                SoundManager.instance.PlaySoundClip(skillCharging_1, this.transform, 1f);
                break;
            case "skill1":
                SoundManager.instance.PlaySoundClip(skill1, this.transform, 1f);
                break;
            case "skill2":
                SoundManager.instance.PlaySoundClip(skill2, this.transform, 1f);
                break;
            case "skill2.1":
                SoundManager.instance.PlaySoundClip(skill2_1, this.transform, 1f);
                break;
            case "air1":
                SoundManager.instance.PlaySoundClip(air1, this.transform, 1f);
                break;
            case "air2":
                SoundManager.instance.PlaySoundClip(air2, this.transform, 1f);
                break;
            case "plungeLand":
                SoundManager.instance.PlaySoundClip(plungeLand, this.transform, 1f);
                break;
            case "dash":
                SoundManager.instance.PlaySoundClip(dash, this.transform, 0.7f);
                break;
            case "jump":
                SoundManager.instance.PlaySoundClip(jump, this.transform, 1f);
                break;
            case "ult1":
                SoundManager.instance.PlaySoundClip(ult1, this.transform, 1f);
                break;
            case "ult2":
                SoundManager.instance.PlaySoundClip(ult2, this.transform, 1f);
                break;
            case "ult3":
                SoundManager.instance.PlaySoundClip(ult3, this.transform, 1f);
                break;

            default:
                break;
        }
    }


}
