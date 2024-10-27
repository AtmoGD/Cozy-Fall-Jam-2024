using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedClipPlayback : MonoBehaviour
{
    [SerializeField] private bool loop = false;
    [SerializeField] private float delayFrom = 0;
    [SerializeField] private float delayTo = 0;
    [SerializeField] private float randomPitchRange = 0;
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;
    
    void Start()
    {

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (loop == true) {
            randomDelayPlaybackCoroutine();
        }
    }




    private IEnumerator randomDelayPlaybackCoroutine(){
        Play();
        Debug.Log("Coroutine");
        yield return new WaitForSecondsRealtime(Random.Range(delayFrom, delayTo));
    }

    public void Play(){


        int randomIndex = Random.Range(0, audioClips.Length);
        AudioClip clip = audioClips[randomIndex];
        audioSource.clip = clip;
        audioSource.pitch = Random.Range(-randomPitchRange, randomPitchRange);
        Debug.Log("Played Sound");
        audioSource.Play();
    }
}
