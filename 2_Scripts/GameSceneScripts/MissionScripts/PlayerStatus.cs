using UnityEngine;
using UnityEngine.UI;
using DefineUtils;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] GameObject _hpObj;
    [SerializeField] Sprite[] _hpBar;

    int _hpCnt;
    int _maxHp = 20;
    float _hpPerOneCnt;
    bool _isDanger = false;

    public eMissionKind _specialization;
    public int _nowHp;
    public float _walkSpeed;
    public float _maxVelocityMove;
    public float _repairStat;
    public float _researchStat;

    public bool _isSafetyZone = false;

    void Awake()
    {
        InitStat();
    }
    void InitStat()
    {
        _hpCnt = _hpObj.transform.childCount;
        _nowHp = _maxHp;
        _hpPerOneCnt = _maxHp / _hpCnt;
        _specialization = (eMissionKind)DataManager._instance.dataClass._characterNumber;
        _walkSpeed = 4f;
        _maxVelocityMove = 4f;
        _repairStat = 1;
        _researchStat = 1;
        _isSafetyZone = false;

        SetSpecialStat();
    }
    public void GetDamage(int damage)
    {
        _nowHp -= damage;
        if (_nowHp <= _hpPerOneCnt * (_hpCnt-1))
        {
            _hpCnt -= 1;
            if(_nowHp <= _maxHp/2 && !_isDanger)
            {
                for(int i = 0; i < _hpObj.transform.childCount; i++)
                {
                    _hpObj.transform.GetChild(i).GetComponent<Image>().sprite = _hpBar[1];
                }
                _isDanger = true;
            }
            Destroy(_hpObj.transform.GetChild(_hpCnt).gameObject);
        }

        if(_nowHp <= 0)
        {
            GameObject.FindObjectOfType<MissionManager>()._isFailed = true;
            return;
        }
    }

    void SetSpecialStat()
    {
        if(_specialization == eMissionKind.RESEARCH)
        {
            _researchStat = 1.2f;
            _walkSpeed = 4.5f;
        }
        else if (_specialization == eMissionKind.REPAIR)
        {
            _repairStat = 1.2f;
            _walkSpeed = 4.5f;
        }
    }
}
