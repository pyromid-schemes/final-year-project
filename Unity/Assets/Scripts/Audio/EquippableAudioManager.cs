using UnityEngine;
using System.Collections;

public class EquippableAudioManager : MonoBehaviour {

    public AudioClip EnvironmentHit1;
    public AudioClip EnvironmentHit2;
    public AudioClip WeaponHit1;
    public AudioClip WeaponHit2;
    public AudioClip ShieldHit1;
    public AudioClip ShieldHit2;
    public AudioClip MonsterHit1;
    public AudioClip MonsterHit2;

    AudioSource Source;
    float TimeOfLastAudio;
    float PauseBetweenNewSounds = 0.1f;

    float Volume = 0.5f;

    void Awake()
    {
        Source = gameObject.AddComponent<AudioSource>();
    }

    public void PlayCollisionWith(string colliderTag)
    {
        switch (colliderTag)
        {
            case "Shield":
                ShieldHit(Volume);
                break;
            case "Weapon":
                WeaponHit(Volume);
                break;
            case "Monster":
                MonsterHit(Volume);
                break;
            case "Wall":
                EnvironmentHit(Volume);
                break;
            case "Floor":
                EnvironmentHit(Volume);
                break;
        }
    }

    public void EnvironmentHit(float vol)
    {
        if(!Source.isPlaying || TimeOfLastAudio + PauseBetweenNewSounds < Time.time)
        {
            TimeOfLastAudio = Time.time;
            if (Random.value > 0.5)
            {
                Source.PlayOneShot(EnvironmentHit1, vol);
            }
            else
            {
                Source.PlayOneShot(EnvironmentHit2, vol);
            }
        }
    }

    public void WeaponHit(float vol)
    {
        if (!Source.isPlaying || TimeOfLastAudio + PauseBetweenNewSounds < Time.time)
        {
            TimeOfLastAudio = Time.time;
            if (Random.value > 0.5)
            {
                Source.PlayOneShot(WeaponHit1, vol);
            }
            else
            {
                Source.PlayOneShot(WeaponHit2, vol);
            }
        }
    }

    public void ShieldHit(float vol)
    {
        if (!Source.isPlaying || TimeOfLastAudio + PauseBetweenNewSounds < Time.time)
        {
            TimeOfLastAudio = Time.time;
            if (Random.value > 0.5)
            {
                Source.PlayOneShot(ShieldHit1, vol);
            }
            else
            {
                Source.PlayOneShot(ShieldHit2, vol);
            }
        }
    }

    public void MonsterHit(float vol)
    {
        if (!Source.isPlaying || TimeOfLastAudio + PauseBetweenNewSounds < Time.time)
        {
            TimeOfLastAudio = Time.time;
            if (Random.value > 0.5)
            {
                Source.PlayOneShot(MonsterHit1, vol);
            }
            else
            {
                Source.PlayOneShot(MonsterHit2, vol);
            }
        }
    }
}
