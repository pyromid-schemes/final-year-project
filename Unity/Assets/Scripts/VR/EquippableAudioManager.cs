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

    readonly float VEL_TO_VOL_SCALE = 0.2f;

    void Awake()
    {
        Source = gameObject.AddComponent<AudioSource>();
    }

    public void PlayCollisionWith(string colliderTag)
    {
        switch (colliderTag)
        {
            case "Shield":
                ShieldHit(gameObject.GetComponent<Rigidbody>().velocity.magnitude * VEL_TO_VOL_SCALE);
                break;
            case "Weapon":
                WeaponHit(gameObject.GetComponent<Rigidbody>().velocity.magnitude * VEL_TO_VOL_SCALE);
                break;
            case "Monster":
                MonsterHit(gameObject.GetComponent<Rigidbody>().velocity.magnitude * VEL_TO_VOL_SCALE);
                break;
            case "Wall":
                PlayEnvironmentHit(gameObject.GetComponent<Rigidbody>().velocity.magnitude * VEL_TO_VOL_SCALE);
                break;
            case "Floor":
                PlayEnvironmentHit(gameObject.GetComponent<Rigidbody>().velocity.magnitude * VEL_TO_VOL_SCALE);
                break;
        }
    }

    public void PlayEnvironmentHit(float vol)
    {
        if (Random.value > 0.5)
        {
            Source.PlayOneShot(EnvironmentHit1, vol);
        }
        else
        {
            Source.PlayOneShot(EnvironmentHit2, vol);
        }
    }

    public void WeaponHit(float vol)
    {
        if (Random.value > 0.5)
        {
            Source.PlayOneShot(WeaponHit1, vol);
        }
        else
        {
            Source.PlayOneShot(WeaponHit2, vol);
        }
    }

    public void ShieldHit(float vol)
    {
        if (Random.value > 0.5)
        {
            Source.PlayOneShot(ShieldHit1, vol);
        }
        else
        {
            Source.PlayOneShot(ShieldHit2, vol);
        }
    }

    public void MonsterHit(float vol)
    {
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
