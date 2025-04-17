using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSourceObject;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void PlaySoundClip(AudioClip audioClip, Transform audioLocation, float volume)
    {
        //spawn the audio source
        AudioSource audioSource = Instantiate(audioSourceObject, audioLocation.position, Quaternion.identity);

        //assign the sound clip and play
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        //destroy it after done playing
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void PlayRandomSoundClip(AudioClip[] audioClip, Transform audioLocation, float volume)
    {
        //take a random sound from the array
        int rand = Random.Range(0, audioClip.Length);

        //spawn the audio source
        AudioSource audioSource = Instantiate(audioSourceObject, audioLocation.position, Quaternion.identity);

        //assign the sound clip and play
        audioSource.clip = audioClip[rand];
        audioSource.volume = volume;
        audioSource.Play();

        //destroy it after done playing
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }





    IEnumerator DisableAfterTime(GameObject gameObj, float time)
    {
        yield return new WaitForSeconds(time);
        gameObj.SetActive(false);
    }


}