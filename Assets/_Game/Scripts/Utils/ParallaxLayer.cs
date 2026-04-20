using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float _strength;

    [SerializeField] private bool _freezeX;
    [SerializeField] private bool _freezeY;

    private Transform _target;
    private Vector3 _targetPreviousPosition;
    private Vector2 _offset;

    private void Start()
    {
        _target = Camera.main.transform;
        _targetPreviousPosition = _target.position;
        _offset = transform.position - _target.position;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 delta = _target.position - _targetPreviousPosition;
        _targetPreviousPosition = _target.position;

        if (_freezeX) delta.x = 0f;
        if (_freezeY) delta.y = 0f;

        transform.position += delta * _strength;
    }
}
