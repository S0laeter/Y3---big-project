using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudio : MonoBehaviour
{
    public AudioClip bossHit;

    public AudioClip[] punch;
    public AudioClip slam;
    public AudioClip shoot;

    public AudioClip rapidFire;

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
            case "bossHit":
                SoundManager.instance.PlaySoundClip(bossHit, this.transform, 1f);
                break;
            case "punch":
                SoundManager.instance.PlayRandomSoundClip(punch, this.transform, 1f);
                break;
            case "slam":
                SoundManager.instance.PlaySoundClip(slam, this.transform, 1f);
                break;
            case "shoot":
                SoundManager.instance.PlaySoundClip(shoot, this.transform, 1f);
                break;
            case "rapidFire":
                SoundManager.instance.PlaySoundClip(rapidFire, this.transform, 1f);
                break;
            case "dash":
                SoundManager.instance.PlaySoundClip(dash, this.transform, 0.7f);
                break;

            default:
                break;
        }
    }
}
