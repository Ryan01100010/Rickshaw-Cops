using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioPlayer;

    public AudioClip shootSound, explosionSound, hitSound;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void shoot(float v)
    {
        audioPlayer.PlayOneShot(shootSound, v);
    }

    public void explosion(float v)
    {
        audioPlayer.PlayOneShot(explosionSound, v);
    }

    public void hit(float v)
    {
        audioPlayer.PlayOneShot(hitSound, v);
    }
}
