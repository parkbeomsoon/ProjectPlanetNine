using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [SerializeField] GameObject _endCredit;

    void Start()
    {
        CheckEnd();
    }

    void CheckEnd()
    {
        if(DataManager._instance.dataClass._hp <= 0)
        {
            StartEnding();
        }
        else if (DataManager._instance.dataClass._progress == DataManager._instance._endMissionLevel)
        {
            StartEnding();
        }
    }

    void StartEnding()
    {
        _endCredit.SetActive(true);
        AudioManager._instance.PlayEndingBGM();
    }
}
