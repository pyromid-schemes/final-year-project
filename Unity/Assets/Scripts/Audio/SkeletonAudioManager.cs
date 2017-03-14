using UnityEngine;
using System.Collections;

public class SkeletonAudioManager : MonoBehaviour {

    public AudioClip Moan;
    public AudioClip Die;

    AudioSource Source;

    float Volume = 0.5f;
    float MoanChance = 0.005f;

    void Awake()
    {
        Source = gameObject.AddComponent<AudioSource>();
    }
	
	void Update()
    {
        if(!Source.isPlaying && Random.value <= MoanChance)
        {
            PlayMoan();
        }
    }

    public void PlayDeath()
    {
        Source.PlayOneShot(Die, Volume);
    }

    void PlayMoan()
    {
        Source.PlayOneShot(Moan, Volume);
    }
}
