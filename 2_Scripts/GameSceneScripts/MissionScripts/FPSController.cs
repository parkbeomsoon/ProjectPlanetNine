using System.Collections;
using UnityEngine;
using DefineUtils;

public class FPSController : MonoBehaviour
{
    #region 카메라 움직임 변수

    public bool _camCanMove = true;
    public float _sensitivity = 1f;
    public float _maxAngle = 60f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float _actShotZoffSet = 0.03f;
    Camera _playerCam;
    GameObject _playerObj;
    Vector3 _playerPosOrigin;

    #endregion

    #region 플레이어 움직임 변수

    Rigidbody _rigidBody;

    public bool _canMove = true;
    public bool _isWalk = false;

    #endregion

    #region 공격용 변수

    [SerializeField] GameObject[] _weapons;
    [SerializeField] GameObject[] _hitEffectPrefab;
    GameObject _nowWeaponObj;
    int _nowWeaponNo = 0;
    int _maxEffectCnt = 50;
    float _laserWeaponAttDelayTime = 1f;
    float[] _laserTime = { 90f, 30f };
    float[] _laserDamage = { 3f, 10f };
    float[] _weaponDistArr = { Mathf.Infinity, 5f };
    bool _noLaser = false;

    public bool _isAttack = false;

    public int GetNowWeaponNo
    {
        get { return _nowWeaponNo; }
    }
    public int getLaserTime
    {
        get { return (int)_laserTime[_nowWeaponNo]; }
    }
    #endregion

    [SerializeField] MissionExplainWindow _explainWindow;
    PlayerStatus _playerStat;
    float _sponTime = 1f;
    bool _isSponed = false;
    public bool _isPaused = false;
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _playerCam = Camera.main;
        _playerObj = GameObject.FindGameObjectWithTag("PlayerObj");
        _playerStat = _playerObj.GetComponent<PlayerStatus>();
    }

    void Start()
    {
        _playerPosOrigin = _playerObj.transform.position;
        StartCoroutine(LaserDelay());
        StartCoroutine(Spawn());
        _nowWeaponObj = Instantiate(_weapons[0], _playerObj.transform);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!_isSponed || _isPaused) return;   

        #region 조작
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject go = GameObject.FindGameObjectWithTag(DefineUtils.ePrefabs.MenuWindow.ToString());

            if (go == null)
            {
                SetDontMove();
                Instantiate(WindowManager._instance.GetPrefabs(DefineUtils.ePrefabs.MenuWindow));
                _isPaused = true;
            }
            else
            {
                SetMove();
                _isPaused = false;
                Destroy(go);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _explainWindow.gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            _explainWindow.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(DataManager._instance.dataClass.options.keyOptions[(int)eKeyOption.SwapWeapon1]._keyCode))
        {
            _nowWeaponNo = 0;
            Destroy(_nowWeaponObj.gameObject);
            _nowWeaponObj = Instantiate(_weapons[_nowWeaponNo], _playerObj.transform);
        }
        else if (Input.GetKeyDown(DataManager._instance.dataClass.options.keyOptions[(int)eKeyOption.SwapWeapon2]._keyCode))
        {
            if(_playerStat._specialization == eMissionKind.SURVIVAL)
            {
                _nowWeaponNo = 1;
                Destroy(_nowWeaponObj.gameObject);
                _nowWeaponObj = Instantiate(_weapons[_nowWeaponNo], _playerObj.transform);
            }
        }

        #endregion

        #region 카메라 움직임
        if (_camCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _sensitivity;

            pitch -= Input.GetAxis("Mouse Y") * _sensitivity;
        }

        pitch = Mathf.Clamp(pitch, -_maxAngle, _maxAngle);
        transform.localEulerAngles = new Vector3(0, yaw, 0);
        if(pitch > 23)
        {
            if (_playerObj.transform.localPosition.z > -0.1f)
                _playerObj.transform.localPosition -= Vector3.forward * Time.deltaTime * 3;//Mathf.Clamp((23 - pitch), -0.2f,0.2f);
        }
        else
        {
            if (_playerObj.transform.localPosition.z < _playerPosOrigin.z)
                    _playerObj.transform.localPosition += Vector3.forward * Time.deltaTime * 3;
        }
        _playerCam.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        _playerObj.transform.localEulerAngles = new Vector3(pitch + 5, _playerObj.transform.localEulerAngles.y, _playerObj.transform.localEulerAngles.z);
        #endregion

        #region 공격

        if (Input.GetMouseButtonDown(0))
        {
            _isAttack = true;
            _playerObj.transform.position -= transform.forward * _actShotZoffSet;
            GameObject go = GameObject.FindGameObjectWithTag("Weapon");
            go.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();
            if(_nowWeaponNo == 1)
            {
                go.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().Play();
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (_laserTime[_nowWeaponNo] < 0) 
            {
                if (!_noLaser)
                {
                    _noLaser = true;
                    GameObject go2 = GameObject.FindGameObjectWithTag("Weapon");
                    go2.transform.GetChild(0).gameObject.SetActive(false);
                }
                return; 
            }
            
            _laserTime[_nowWeaponNo] -= Time.deltaTime;
            GameObject go = GameObject.FindGameObjectWithTag("Weapon");
            
            if(go.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().particleCount == 3)
                go.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Pause();

            Transform lightTf = go.transform.GetChild(0).GetChild(0).transform;
            int layerMask = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Map"));
            Ray ray = new Ray(lightTf.position, -lightTf.forward);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, _weaponDistArr[_nowWeaponNo], layerMask))
            {
                
                if (_laserWeaponAttDelayTime < 0)
                {
                    if (hitData.collider.tag.Equals("EnemyObj"))
                    {
                        hitData.collider.gameObject.GetComponent<EnemyController>().HitEnemy(_laserDamage[_nowWeaponNo]);
                    }
                    _laserWeaponAttDelayTime = 1f;
                }
                if(GameObject.FindGameObjectsWithTag("Effect").Length < _maxEffectCnt)
                {
                    GameObject hitEffect = Instantiate(_hitEffectPrefab[_nowWeaponNo], hitData.point, Quaternion.LookRotation(hitData.normal),hitData.transform);
                    Destroy(hitEffect, 0.5f);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            _isAttack = false;
            _playerObj.transform.position += transform.forward * _actShotZoffSet;
            GameObject go = GameObject.FindGameObjectWithTag("Weapon");
            
            go.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Clear();
            if (_nowWeaponNo == 1)
            {
                go.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().Stop();
            }
        }
        #endregion

    }

    void FixedUpdate()
    {
        #region 캐릭터 움직임
        if (_canMove)
        {
            Vector3 targetVec = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));

            if(targetVec.x != 0 || targetVec.z != 0)
                _isWalk = true;
            else
                _isWalk = false;

            targetVec = transform.TransformDirection(targetVec) * _playerStat._walkSpeed;
            Vector3 vctVec = _rigidBody.velocity;
            Vector3 vctMoveVec = targetVec - vctVec;
            vctMoveVec.x = Mathf.Clamp(vctMoveVec.x, -_playerStat._maxVelocityMove, _playerStat._maxVelocityMove);
            vctMoveVec.z = Mathf.Clamp(vctMoveVec.z, -_playerStat._maxVelocityMove, _playerStat._maxVelocityMove);
            vctMoveVec.y = 0;

            _rigidBody.AddForce(vctMoveVec,ForceMode.VelocityChange);
        }
        #endregion

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("SafetyZone"))
        {
            _playerStat._isSafetyZone = true;
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("SafetyZone"))
        {
            _playerStat._isSafetyZone = false;
        }

    }

    public void SetDontMove()
    {
        _camCanMove = false;
        _canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void MoveOnlyCam()
    {
        _camCanMove = true;
        _canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }
    public void SetMove()
    {
        _camCanMove = true;
        _canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    IEnumerator LaserDelay()
    {
        while (true)
        {
            if (_isAttack)
            {
                _laserWeaponAttDelayTime -= Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator Spawn()
    {
        SetDontMove();

        while (!_isSponed)
        {
            _sponTime -= Time.deltaTime;
            if (_sponTime <= 0)
            {
                SetMove();
                _isSponed = true;
                _playerPosOrigin = _playerObj.transform.localPosition;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
