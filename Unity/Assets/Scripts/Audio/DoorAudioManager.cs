using UnityEngine;
using System.Collections;
/*
 * @author Japeth Gurr (jarg2)
 * Audio management script for doors 
*/
public class DoorAudioManager : MonoBehaviour {

    public AudioClip OpenDoor;

    AudioSource Source;

    float Volume = 0.5f;

    void Awake()
    {
        Source = gameObject.AddComponent<AudioSource>();
        Source.spatialBlend = 1;
    }

	public void PlayDoorOpen()
    {
        Source.PlayOneShot(OpenDoor, Volume);
	}
}
