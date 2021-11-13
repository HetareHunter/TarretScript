using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockDeath : EnemyDeath
{
    Vector3 startPosi;
    Quaternion startAngle;
    [SerializeField] int _addScore = 100;

    Rigidbody _rb;
    Collider _collider;
    MeshDissolver _meshDissolver;
    BlockDeathAudio _blockDeathAudio;
    BlockMover _blockMover;

    float _normalDrag;
    float _normalAnglarDrag;
    bool _normalUseGravity;
    [SerializeField] float _strongDrag = 4.0f;
    [SerializeField] float _strongAnglarDrag = 4.0f;

    private void Awake()
    {
        _meshDissolver = GetComponent<MeshDissolver>();
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _blockDeathAudio = GetComponent<BlockDeathAudio>();
        if(TryGetComponent<BlockMover>(out var blockMover))
        {
            _blockMover = blockMover;
        }
        
        IsRigidBodyKinematicEnabled(true);
        IsCollisionEnabled(false);

        startPosi = transform.localPosition;
        startAngle = transform.localRotation;
        _normalDrag = _rb.drag;
        _normalAnglarDrag = _rb.angularDrag;
        _normalUseGravity = _rb.useGravity;
    }
    public void Reset()
    {
        if (_meshDissolver == null)
        {
            _meshDissolver.GetComponent<MeshDissolver>();
        }
        transform.localPosition = startPosi;
        transform.localRotation = startAngle;
        
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        NormalRigidBody();

        _meshDissolver.Reset();
        if (_blockMover != null)
        {
            _blockMover.Reset();
        }
    }

    public override void OnDead()
    {
        AddScore();
        IsCollisionEnabled(false);
        AfterDeadRigidBody();
        _meshDissolver.IsPlayDissolve(true);
        
        PlayDeadSound();
    }

    public override void AddScore()
    {
        ScoreManager.Instance.AddScore(_addScore);
    }

    public void AfterDeadRigidBody()
    {
        _rb.drag = _strongDrag;
        _rb.angularDrag = _strongAnglarDrag;
        _rb.useGravity = false;
    }

    void NormalRigidBody()
    {
        _rb.drag = _normalDrag;
        _rb.angularDrag = _normalAnglarDrag;
        _rb.useGravity = _normalUseGravity;
    }

    public void IsCollisionEnabled(bool enabled)
    {
        _collider.enabled = enabled;
    }

    public void IsRigidBodyKinematicEnabled(bool enabled)
    {
        _rb.isKinematic = enabled;
    }

    public void DoNonActivate()
    {
        _meshDissolver.DoNonActive();
        IsRigidBodyKinematicEnabled(true);
        IsCollisionEnabled(false);
    }

    public void DoActivate()
    {
        IsRigidBodyKinematicEnabled(false);
        IsCollisionEnabled(true);
    }

    void PlayDeadSound()
    {
        _blockDeathAudio.PlayDeathSound();
    }
}
