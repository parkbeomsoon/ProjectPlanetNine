using System.Collections;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    int _targetEnemyCnt = 0;
    int _targetRepairCnt = 0;
    int _targetResearchCnt = 0;

    int _clearEnemyCnt = 0;
    int _clearRepairCnt = 0;
    int _clearResearchCnt = 0;

    float _endDelayTime = 3f;

    public bool _isStart = false;
    public bool _isFailed = false;
    void Update()
    {
        if (_isStart)
        {
            if (_clearEnemyCnt == _targetEnemyCnt && _clearRepairCnt == _targetRepairCnt && _clearResearchCnt == _targetResearchCnt)
            {
                StartCoroutine(OpenResultWindow(true));
                _isStart = false;
            }

            if (_isFailed)
            {
                StartCoroutine(OpenResultWindow(false));
                _isStart = false;
            }
        }
    }

    public void SetTarget(int enemyCnt, int repairCnt, int researchCnt)
    {
        _targetEnemyCnt = enemyCnt;
        _targetRepairCnt = repairCnt;
        _targetResearchCnt = researchCnt;

        _isStart = true;
    }

    public void Clear(int kind)
    {
        if (kind == 1)
            _clearEnemyCnt++;
        else if (kind == 2)
            _clearRepairCnt++;
        else if (kind == 3)
            _clearResearchCnt++;
    }
    IEnumerator OpenResultWindow(bool isSuccess)
    {
        GameObject go = Instantiate(WindowManager._instance.GetPrefabs(DefineUtils.ePrefabs.ResultWindow));
        ResultWindow rw = go.GetComponent<ResultWindow>();

        if (isSuccess)
        {
            rw.SetResultText("미션 성공");
            DataManager._instance.dataClass._progress += 1;
            DataManager._instance.SaveData();
        }
        else
        { 
            rw.SetResultText("미션 실패");
            DataManager._instance.dataClass._hp -= 2;
            DataManager._instance.SaveData();
        }

        yield return new WaitForSeconds(_endDelayTime);
        SceneControlManager._instance.StartTpvGameScene();
    }

}
