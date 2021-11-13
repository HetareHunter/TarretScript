using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class BlockDeathAudio : MonoBehaviour
{
    AudioSource _audioSource;
    AudioMixer _audioMixer;
    [SerializeField] AudioClip[] _audioClips;
    [SerializeField] float _minAudioPitch = 70.0f;
    [SerializeField] float _maxAudioPitch = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioMixer = _audioSource.outputAudioMixerGroup.audioMixer;
    }

    public void PlayDeathSound()
    {
        var randomNom = Random.Range(0, _audioClips.Length);
        var audioPitch = Random.Range(_minAudioPitch, _maxAudioPitch);
        _audioMixer.SetFloat("BlockSEPitch", audioPitch);
        _audioSource.PlayOneShot(_audioClips[randomNom]);
    }
}
