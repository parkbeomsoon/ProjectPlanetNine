using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject _light;

    //TpvGameScene �÷��̾�

    #region ī�޶� ����

    public bool _camCanMove = true;
    public float _sensitivity = 1f;
    public float _maxAngle = 60f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    public Camera _cam;

    #endregion

    #region ������ ����


    public float _speed = 10;
    float _EndBackPosZ = -5000f;
    Transform tf;
    bool _playerCanMove = true;

    #endregion

    #region �̼� ���� �˻�

    [SerializeField] GameObject _missionObj;
    float _offSetZ = 50f;
    float _nextMissionPosZ;
    int _nowMissionLevel;

    #endregion

    void Awake()
    {
        tf = transform;
        _cam = Camera.main;
        _camCanMove = false;
        _playerCanMove = false;
    }

    void Start()
    {
        LoadCharacterData();
        StartCoroutine(WaitScene());
        StartCoroutine(MissionProcess());
    }
    void Update()
    {
        #region ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject go = GameObject.FindGameObjectWithTag(DefineUtils.ePrefabs.MenuWindow.ToString());

            if (go == null)
            {
                SetDontMove();
                Instantiate(WindowManager._instance.GetPrefabs(DefineUtils.ePrefabs.MenuWindow));
            }
            else
            {
                SetMove();
                Destroy(go);
            }

        }
        #endregion

        //���ּ� ��鸲
        tf.rotation = Quaternion.Euler(new Vector3(0, 0, (Mathf.PingPong(Time.time, 5))));

        #region ī�޶� ������
        if (_camCanMove)
        {
            yaw += Input.GetAxis("Mouse X") * _sensitivity;

            pitch -= Input.GetAxis("Mouse Y") * _sensitivity;


            pitch = Mathf.Clamp(pitch, -_maxAngle, _maxAngle);
            yaw = Mathf.Clamp(yaw, -_maxAngle, _maxAngle);

            _cam.transform.localEulerAngles = new Vector3(pitch, yaw, 0);
            //ī�޶� ������ ����Ʈ ������
            _light.transform.localEulerAngles = new Vector3(pitch, yaw, 0);
        }
        #endregion
    }

    void FixedUpdate()
    {
        #region �÷��̾� �̵�
        if (_playerCanMove)
        {
            float mv = 0f;

            if ((mv = Input.GetAxis("Vertical")) != 0)
            {
                //���� ���� Ȯ��
                if(tf.position.z > _EndBackPosZ)
                    tf.position += Vector3.forward * mv * _speed;
                else if (mv >= 0)
                    tf.position += Vector3.forward * mv * _speed;
            }
        }
        #endregion
    }

    void LoadCharacterData()
    {
        tf.position = new Vector3(tf.position.x, tf.position.y, DataManager._instance.dataClass._characterPosZ);
        _nowMissionLevel = DataManager._instance.dataClass._progress;
    }

    IEnumerator WaitScene()
    {
        //�� ���� ���
        while (true)
        {
            if (GameObject.FindGameObjectWithTag("TpvUI").GetComponent<TpvUI>()._isStart)
            {
                SetMove();
                break;
            }
            else yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator MissionProcess()
    {
        //�̼����� �˻�
        while (true)
        {
            if(_nowMissionLevel == DataManager._instance._endMissionLevel)
            {
                break;
            }
            
            _nextMissionPosZ = _missionObj.transform.GetChild(_nowMissionLevel).transform.position.z;
            if (tf.position.z > _nextMissionPosZ - _offSetZ && tf.position.z < _nextMissionPosZ + _offSetZ)
            {
                //������ �̼�1
                if(GameObject.FindGameObjectWithTag("MissionWindow") == null)
                {
                    Camera.main.transform.localEulerAngles = Vector3.zero;
                    SetDontMove();
                    GameObject.FindGameObjectWithTag("TpvUI").GetComponent<TpvUI>().OnArrivedMssionPos(_nowMissionLevel);
                }
            }
            else
            {
                if(GameObject.FindGameObjectWithTag("MissionWindow") != null)
                {
                    Destroy(GameObject.FindGameObjectWithTag("MissionWindow"));
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    #region ���������� �Լ�
    public void SetDontMove()
    {
        _camCanMove = false;
        _playerCanMove = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void SetMove()
    {
        _camCanMove = true;
        _playerCanMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion
}
