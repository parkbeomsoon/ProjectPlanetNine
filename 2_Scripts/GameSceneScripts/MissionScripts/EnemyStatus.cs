using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int _maxHp = 20;
    int _nowHp;
    public int _att = 5;
    public float _speed = 3f;
    public float _attDist = 2.5f;
    public float _attAngle = 30f;

    void Awake()
    {
        InitStat();
    }

    void InitStat()
    {
        _att = 5;
        _speed = 3f;
        _attDist = 2.5f;
        _attAngle = 30f;
        _maxHp = 20;
        _nowHp = _maxHp;
    }

}
