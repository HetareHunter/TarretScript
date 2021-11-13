using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHitSound : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip[] nearSounds;
    [SerializeField] AudioClip[] farSounds;
    [SerializeField] float audioVolume = 0.2f;

    /// <summary> 音の近い音か遠い音かの判断する閾値 </summary>
    [SerializeField] float soundDistanceThreshold = 50.0f;

    /// <summary>乱数を格納する </summary>
    int nearSoundNum = 0;
    /// <summary>乱数を格納する </summary>
    int farSoundNum = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        //自分の位置とメインカメラの距離を計算
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        //Debug.Log(distance);
        if (distance <= soundDistanceThreshold)
        {
            audioSource.PlayOneShot(nearSounds[nearSoundNum], audioVolume);
            nearSoundNum = Random.Range(0, nearSounds.Length);
        }
        else
        {
            audioSource.PlayOneShot(farSounds[farSoundNum], audioVolume);
            farSoundNum = Random.Range(0, farSounds.Length);
        }
    }
}
