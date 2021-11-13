using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [SerializeField] GameObject _centerObj;
    [SerializeField] Vector3 _axis = Vector3.up;
    [SerializeField] float _rotateSpeed = 1.0f;
    Rigidbody _rb;
    public bool _rotateMode;
    bool _startRotateMode;
    float _angle = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _startRotateMode = _rotateMode;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateTranslation();
    }
    public void Reset()
    {
        _rotateMode = _startRotateMode;
    }

    void RotateTranslation()
    {
        if (_rotateMode)
        {
            transform.RotateAround(_centerObj.transform.position, _axis, (_angle + _rotateSpeed + Time.deltaTime) % 360);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _rotateMode = false;
        _rb.useGravity = true;
    }
}
