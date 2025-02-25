using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Random = UnityEngine.Random;

public enum State {
    Chasing,
    Wandering,
    Attacking,
    Eating
}

public class WandererCore : MonoBehaviour, IDamageable, IListener
{
    [SerializeField] private float _fullHealth = 100.0f;
    [SerializeField] private float _health;
    [SerializeField] private WandererMovement _movement;
    [SerializeField] private VisionCone _vision;
    [SerializeField] private RagdollToggle _ragdoll;
    [SerializeField] private LayerMask _playerLayerMask;
    
    private GameObject _currentPlayerTarget;
    private Vector3 _lastPlayerPosition;
    private Vector3 _nextPoint = Vector3.zero;
    private float _lastAttackTime = 0;

    public State _state = State.Wandering;

    public void TakeDamage(float damage, Vector3 hitPosition) {
        _health -= damage;
        if (_health <= 0) {
            _movement.Stop();
            _ragdoll.Ragdoll(true);
            _ragdoll.ApplyForce((transform.position - hitPosition).normalized * 1.0f);
            SoundManager.Instance.UnregisterListener(this);
            Destroy(gameObject, 0f);
        }
    }
    private void OnValidate() {
        _health = _fullHealth;
    }
    private void Start() {
        SoundManager.Instance.RegisterListener(this);
    }
    private void Update() {
        _lastAttackTime += Time.deltaTime;
        switch(_state) {
            case State.Wandering:
                if (_nextPoint == Vector3.zero) {
                    _nextPoint = generateNewPoint();
                } else {
                    if (Vector3.Distance(_nextPoint, this.transform.position) < 3) {
                        _nextPoint = generateNewPoint();
                    }
                }

                foreach (GameObject obj in _vision.VisibleObjects) {
                    if (!obj.CompareTag("Player")) continue;
                    _currentPlayerTarget = obj;
                    _state = State.Chasing;
                    return;
                }

                _movement.SetTarget(_nextPoint);
                _movement.Go();
                break;
            case State.Chasing:
                _lastPlayerPosition = _currentPlayerTarget.transform.position;
                if (Vector3.Distance(_lastPlayerPosition, this.transform.position) < 2) {
                    _state = State.Attacking;
                    return;
                }

                bool canSeePlayer = false;
                foreach (GameObject obj in _vision.VisibleObjects) {
                    if (obj.CompareTag("Player")) {
                        canSeePlayer = true;
                        break;
                    }
                }

                if (!canSeePlayer) {
                    _nextPoint = _lastPlayerPosition;
                    _state = State.Wandering;
                    return;
                }

                _movement.SetTarget(_lastPlayerPosition);
                _movement.Go();
                break;
            case State.Attacking:
                Collider[] players = Physics.OverlapSphere(this.transform.position, 2.0f, _playerLayerMask);
                GameObject closestPlayer = null;
                float closestDistance = float.MaxValue;

                foreach (Collider player in players)
                {
                    float distance = Vector3.Distance(player.transform.position, this.transform.position);
                    if (distance < closestDistance)
                    {
                        closestPlayer = player.gameObject;
                        closestDistance = distance;
                    }
                }

                if (closestPlayer != null && _lastAttackTime > 1) {
                    // this.transform.LookAt(closestPlayer.transform.position);
                    closestPlayer.GetComponentInChildren<IDamageable>().TakeDamage(10, this.transform.position);
                    _lastAttackTime = 0;
                }

                _state = State.Chasing;
                break;
            default:
                _state = State.Wandering;
                break;
        }
    }

    private Vector3 generateNewPoint() {
        float x = Random.value * 100 + this.transform.position.x;
        float z =  Random.value * 100 + this.transform.position.z;
        float y = 0;
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(x, 10000, z), Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"))) {
            y = 10000 - hit.distance;
        }
        return new Vector3(x, y, z);
    }

    public void OnSoundHeard(SoundEvent soundEvent) {
        float power = soundEvent.intensity / Mathf.Pow(Vector3.Distance(soundEvent.position, this.transform.position), 2);
        Debug.Log("sound received: " + power);
         if (power > 0.2f) {
            Debug.Log("0.2 heard");
            _currentPlayerTarget = GameObject.FindWithTag("Player");
            _state = State.Chasing;
        } else if (power > 0.1f) {
            Debug.Log("0.1 heard");
            _state = State.Wandering;
            _nextPoint = soundEvent.position;
            _movement.SetTarget(_nextPoint);
            _movement.Go();
        }
    }
}