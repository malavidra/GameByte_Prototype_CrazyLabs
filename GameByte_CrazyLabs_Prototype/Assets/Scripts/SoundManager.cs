using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip snd1, snd2, snd3, snd4, snd5, snd6;
    public AudioSource audiosrc;

    void Start()
    {
        audiosrc = gameObject.GetComponent<AudioSource>();
        snd1 = Resources.Load<AudioClip>("uvodna_spica");
        snd2 = Resources.Load<AudioClip>("presek");
        snd3 = Resources.Load<AudioClip>("start");
        snd4 = Resources.Load<AudioClip>("led");
        snd5 = Resources.Load<AudioClip>("led_die");
        snd6 = Resources.Load<AudioClip>("lost");
    }
    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "start":
                audiosrc.loop = true;
                audiosrc.clip = snd1;
                audiosrc.Play();
                break;
            case "presek":
                audiosrc.PlayOneShot(snd2);
                audiosrc.clip = snd3;
                audiosrc.Play();
                break;
            case "powerup":
                audiosrc.PlayOneShot(snd4);
                break;
            case "obstacle":
                audiosrc.PlayOneShot(snd5);
                break;
            case "lost":
                audiosrc.loop = true;
                audiosrc.clip = snd6;
                audiosrc.Play();
                break;
        }
    }
}

