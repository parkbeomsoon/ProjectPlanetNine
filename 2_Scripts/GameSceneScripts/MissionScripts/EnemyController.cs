using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    Animator _ani;
    NavMeshAgent _nav;
    Transform _playerTf;
    EnemyStatus _stat;
    PlayerStatus _playerStat;

    Vector3 _fisrtPos;

    float _sightAngle = 30f;
    float _speed = 3f;
    float _hp = 20;
    bool isDetected = false;
    bool targetOn = false;
    bool _isAttacking = false;
    bool gameEnd = false;
    bool _isDead = false;

    MissionManager mm;

    void Awake()
    {
        _playerStat = GameObject.FindGameObjectWithTag("PlayerObj").GetComponent<PlayerStatus>();
        _stat = GetComponent<EnemyStatus>();
        _ani = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
        _playerTf = GameObject.FindGameObjectWithTag("PlayerController").transform;
    }

    void Start()
    {
        _fisrtPos = transform.position;
        _nav.speed = _stat._speed;
        _nav.stoppingDistance = 2f;
        _nav.updateRotation = false;
        mm = GameObject.FindObjectOfType<MissionManager>();
    }
    void Update()
    {
        if (_hp <= 0)
            DeadEnemy();

        if (gameEnd || _isDead) return;
        if (_isAttacking) return;
        if (_playerStat._isSafetyZone) 
        {
            SetDestination(_fisrtPos);
            return;
        }

        if (!isDetected)
        {
            Detecting();
        }
        else if (isDetected && !targetOn)
        {
            MoveToPlayer();
        }
        else if (targetOn)
        {
            try
            {
                _nav.SetDestination(_playerTf.position);
            }
            catch { Debug.Log("NavMesh Error"); }
            //이동중이고 목적지에 도착했는지
            if (_nav.velocity.sqrMagnitude >= 0.2f * 0.2f && _nav.remainingDistance <= _stat._attDist)
            {
                AttackPlayer();
            }
            else if (_nav.remainingDistance > _stat._attDist)
            {
                targetOn = false;
            }
        }
    }
    bool Detecting()
    {
        Vector3 targetDir = (_playerTf.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, targetDir);

        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (theta <= _sightAngle)
        {
            isDetected = true;
            StartCoroutine(RotateAngle());
            return true;
        }

        isDetected = false;
        return false;
    }
    void SetDestination(Vector3 pos)
    {
        _nav.isStopped = false;
        float dist = Vector3.Distance(pos, transform.position);
        if (dist < 1.5f) _ani.SetBool("findPlayer", false);

        _ani.SetBool("canAttack", false);
        _nav.SetDestination(pos);
    }
    public void HitEnemy(float damage)
    {
        _hp -= damage;
        isDetected = true;
        targetOn = false;
        StartCoroutine(RotateAngle());
    }
    void MoveToPlayer()
    {
        _ani.SetBool("findPlayer", true);
        _ani.SetBool("canAttack", false);
        _nav.speed = _speed;
        _nav.isStopped = false;
        targetOn = true;
    }
    void AttackPlayer()
    {
        _nav.isStopped = true;
        _ani.SetBool("canAttack", true);
        _isAttacking = true;
    }
    public void PlayerHit()
    {
        //거리계산
        float dist = Vector3.Distance(_playerTf.position, transform.position);

        //각도 계산
        Vector3 cameraToObj = _playerTf.position - transform.position;
        float angle = Vector3.Angle(cameraToObj, transform.forward);

        if (dist < _stat._attDist && angle < _stat._attAngle)
        {
            _playerStat.GetDamage(_stat._att);
            if (_playerStat._nowHp <= 0)
            {
                _ani.StartPlayback();
                gameEnd = true;
            }
        }
        _isAttacking = false;
    }
    void DeadEnemy()
    {
        _nav.isStopped = true;
        _isDead = true;
        _ani.SetBool("isDead", _isDead);
    }

    public void DestroyEnemy()
    {
        mm.Clear(1);
        Destroy(gameObject);
    }

    IEnumerator RotateAngle()
    {
        while (!_isDead && isDetected)
        {
            Vector2 forward = new Vector2(transform.position.z, transform.position.x);
            Vector2 steeringTarget = new Vector2(_nav.steeringTarget.z, _nav.steeringTarget.x);

            Vector2 dir = steeringTarget - forward;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * angle;

            yield return new WaitForEndOfFrame();
        }
    }
}
