using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
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
    public AudioClip skill1_1;
    public AudioClip skill2;
    public AudioClip skill2_1;

    public AudioClip air1;
    public AudioClip air2;
    public AudioClip plunge;
    public AudioClip plungeLand;

    public AudioClip dash;



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
            case "basic4_1":
                SoundManager.instance.PlaySoundClip(basic4_1, this.transform, 1f);
                break;
            case "basic5":
                SoundManager.instance.PlaySoundClip(basic5, this.transform, 1f);
                break;
            case "basic5_1":
                SoundManager.instance.PlaySoundClip(basic5_1, this.transform, 1f);
                break;

            default:
                break;
        }
    }


}
