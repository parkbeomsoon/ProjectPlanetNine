using UnityEngine;
using UnityEngine.UI;
using DefineUtils;

public class MissionExplainWindow : MonoBehaviour
{
    [SerializeField] Text _contentText;
    [SerializeField] Text _missionText;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetMissionContent(int mapNo)
    {
        string text = string.Empty;

        switch ((eMissionKind)mapNo)
        {
            case eMissionKind.SURVIVAL:
                text = "모든 적을 사살하십시오.";
                break;
            case eMissionKind.REPAIR:
                text = "모든 드론을 수리하십시오.";
                break;
            case eMissionKind.RESEARCH:
                text = "지역을 조사하십시오.";
                break;
        }

        _contentText.text = text;

        text = string.Empty;
        int cnt = DataManager._instance._mapList[mapNo][eMapObjectKind.Enemys];
        if(cnt != 0)
            text += string.Format($"적 : {DataManager._instance._mapList[mapNo][eMapObjectKind.Enemys]}");

        cnt = DataManager._instance._mapList[mapNo][eMapObjectKind.Repairs];
        if (cnt != 0)
            text += string.Format($"\n수리 : {DataManager._instance._mapList[mapNo][eMapObjectKind.Enemys]}");

        cnt = DataManager._instance._mapList[mapNo][eMapObjectKind.Research];
        if (cnt != 0)
            text += string.Format($"\n조사 : {DataManager._instance._mapList[mapNo][eMapObjectKind.Enemys]}");

        _missionText.text = text;
    }
}
