using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _brakingDistance;
    [SerializeField] private float _brakingPower;

    private readonly string _animatorParameter = "Movement";

    private int _animatorParameterID;
    private Rigidbody2D _rigidbody2D;
    private Animator _spaceplaneAnimator;
    private float _sqrBrakingDistance;
    private float _reversedBrakingPower;
    private float _moveSpeed;
    private Vector3 _destination;

    private void Awake()
    {
        _animatorParameterID = Animator.StringToHash(_animatorParameter);

        _rigidbody2D = GetComponent<Rigidbody2D>();

        _sqrBrakingDistance = _brakingDistance * _brakingDistance;
        _reversedBrakingPower = 1f - _brakingPower;

        Cursor cursor = FindObjectOfType<Cursor>();
        cursor.PositionChanged += OnCursorPositionChanged;
    }

    private void FixedUpdate()
    {
        Vector3 vector = _destination - transform.position;
        Vector3 direction = vector.normalized;
        float sqrMagnitude = vector.sqrMagnitude;

        Vector3 force = _moveSpeed * Mathf.Clamp01(sqrMagnitude) * direction;

        _rigidbody2D.AddForce(force);
        _spaceplaneAnimator.SetFloat(_animatorParameterID, force.x);

        if (sqrMagnitude < _sqrBrakingDistance)
        {
            _rigidbody2D.velocity *= _reversedBrakingPower;
        }
    }

    public void SetMoveSpeed(float value)
    {
        _moveSpeed = value;
    }

    public void SetAnimator(Animator animator)
    {
        _spaceplaneAnimator = animator;
    }

    private void OnCursorPositionChanged(Vector3 cursorPosition)
    {
        cursorPosition.z = 0f;
        _destination = cursorPosition;
    }
}
