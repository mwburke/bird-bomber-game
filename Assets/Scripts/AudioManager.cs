using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	// Audio players components.
	public AudioSource EffectsSource;

	// Random pitch adjustment range.
	public float lowPitchRange = 0.9f;
	public float highPitchRange = 1.1f;

	// Singleton instance.
	public static AudioManager Instance = null;

	// Initialize the singleton instance.
	private void Awake() {
		// If there is not already an instance of SoundManager, set it to this.
		if (Instance == null) {
			Instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this) {
			Destroy(gameObject);
		}

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(gameObject);
	}

	// Play a single clip through the sound effects source.
	public void Play(AudioClip clip) {
		EffectsSource.clip = clip;
		EffectsSource.Play();
	}

	// Play a random clip from an array, and randomize the pitch slightly.
	public void PlayOneShotRandomPitch(AudioClip clip, float volume) {
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		EffectsSource.pitch = randomPitch;
		EffectsSource.PlayOneShot(clip, volume);
	}

}