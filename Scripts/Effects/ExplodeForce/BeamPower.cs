using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPower : MonoBehaviour
{
    [SerializeField] float power;
    Rigidbody m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        m_rb.AddForce(transform.forward * power,ForceMode.Impulse);
    }

    void DeathBeamPower()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DeathBeamPower();
        var collisionBlockDeath = collision.gameObject.GetComponent<BlockDeath>();

        if (collisionBlockDeath != null)
        {
            collisionBlockDeath.IsCollisionEnabled(false);
            collisionBlockDeath.AfterDeadRigidBody();
        }
    }
}
