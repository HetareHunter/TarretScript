using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    public float power = 10;
    Rigidbody m_rb;
    AudioPlayer audioPlayer;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        audioPlayer = GetComponent<AudioPlayer>();
    }
    private void OnEnable()
    {
        //audioPlayer.AudioPlay();
    }

    public void Fire()
    {
        m_rb.velocity = transform.forward * speed;
        audioPlayer.AudioPlay();
        //Invoke("NotActive", activeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroyBullet();
    }

    private void OnCollisionEnter(Collision collision)
    {
        DestroyBullet();
    }

    void DestroyBullet()
    {
        m_rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }

}
