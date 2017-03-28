using UnityEngine;
using System.Collections;
/*
 * @author Japeth Gurr (jarg2)
 * Audio management script for Skeletons 
*/
public class SkeletonAudioManager : MonoBehaviour {

    public AudioClip Moan;
    public AudioClip Die;

    AudioSource Source;

    float Volume = 0.5f;
    float MoanChance = 0.005f;

    void Awake()
    {
        Source = gameObject.AddComponent<AudioSource>();
        Source.spatialBlend = 1;
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
