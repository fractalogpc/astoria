using UnityEngine;

public class RadarRandomMove : MonoBehaviour
{
    
    [SerializeField] private Transform _azimuth;
    [SerializeField] private Transform _elevation;

    [SerializeField] private float _azimuthRange = 10f;
    [SerializeField] private float _elevationRange = 10f;

    [SerializeField] private float _timeBetweenMoves = 1f;
    [SerializeField] private float _timeToMove = 1f;

    private float _azimuthOriginal;
    private float _elevationOriginal;
    private float _azimuthTarget;
    private float _elevationTarget;
    private float _azimuthStart;
    private float _elevationStart;
    
    private float _moveTimer = 0f;
    private float _waitTimer = 0f;
    private bool _isMoving = false;

    private void Start()
    {
        _azimuthOriginal = _azimuth.localEulerAngles.z;
        _elevationOriginal = _elevation.localEulerAngles.y;
        _azimuthStart = _azimuthOriginal;
        _elevationStart = _elevationOriginal;
        
        SetRandomTargets();
    }

    private void Update()
    {
        if (_isMoving)
        {
            MoveRadar();
        }
        else
        {
            WaitBeforeMove();
        }
    }

    private void MoveRadar()
    {
        _moveTimer += Time.deltaTime;
        float t = _moveTimer / _timeToMove;

        _azimuth.localEulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(_azimuthStart, _azimuthTarget, t));
        _elevation.localEulerAngles = new Vector3(0f, Mathf.LerpAngle(_elevationStart, _elevationTarget, t), 0f);

        if (_moveTimer >= _timeToMove)
        {
            _azimuthStart = _azimuth.localEulerAngles.z;
            _elevationStart = _elevation.localEulerAngles.y;
            _isMoving = false;
            _waitTimer = 0f;
            SetRandomTargets();
        }
    }

    private void SetRandomTargets()
    {
        _azimuthTarget = _azimuthOriginal + Random.Range(-_azimuthRange, _azimuthRange);
        _elevationTarget = _elevationOriginal + Random.Range(-_elevationRange, _elevationRange);
    }

    private void WaitBeforeMove()
    {
        _waitTimer += Time.deltaTime;

        if (_waitTimer >= _timeBetweenMoves)
        {
            _isMoving = true;
            _moveTimer = 0f;
        }
    }
}
