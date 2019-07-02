using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(ParticleSystem))]
public class ParticleAttractor : MonoBehaviour
{
    [SerializeField]
    private Transform _targetTransform;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float _treshold = 1.0f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float _pause = 1.0f;

    private Transform _transform;
    private ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] _particles;
    private Vector3 _targetPosition;
    private float _cursorMultiplier = 1.0f;
    private float _cursor;
    private int _activeParticlesCount;
    private bool _isWorldSpace;

    public Transform TargetTransform
    {
        get { return _targetTransform; }
        set { _targetTransform = value; }
    }

    void Awake()
    {
        _transform = transform;
        _particleSystem = GetComponent<ParticleSystem>();
        _particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        _isWorldSpace = _particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World;
        _cursorMultiplier = 1.0f / (1.0f - _treshold);
    }

    void LateUpdate()
    {
        _activeParticlesCount = _particleSystem.GetParticles(_particles);
        _targetPosition = _targetTransform.position;

        if (!_isWorldSpace)
        {
            _targetPosition -= _transform.position;
        }

        for (int i = 0; i < _activeParticlesCount; i++)
        {
            _cursor = 1.0f - (_particles[i].remainingLifetime / _particles[i].startLifetime);
            if (_cursor >= _treshold)
            {
                _cursor -= _treshold;
                _cursor *= _cursorMultiplier;

                _particles[i].velocity = Vector3.zero;

                if (_cursor >= _pause)
                {
                    _particles[i].position = Vector3.Lerp(_particles[i].position, _targetPosition, _cursor * _cursor);
                }
            }

            if (_particles[i].position == _targetPosition) _particles[i].remainingLifetime = -1;
        }

        _particleSystem.SetParticles(_particles, _activeParticlesCount);
    }
}