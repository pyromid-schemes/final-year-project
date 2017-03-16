using UnityEngine;
using System.Collections;

public class DoorAudioManager : MonoBehaviour {

    public AudioClip OpenDoor;

    AudioSource Source;

    float Volume = 0.5f;

    void Awake()
    {
        Source = gameObject.AddComponent<AudioSource>();
    }

	void PlayDoorOpen()
    {
        Source.PlayOneShot(OpenDoor, Volume);
	}
}
