using UnityEngine;

public class MissionPortalManager : MonoBehaviour
{
    [SerializeField] GameObject _portals;

    void Start()
    {
        if (DataManager._instance.dataClass._progress == DataManager._instance._endMissionLevel) return;
        OpenPortal(DataManager._instance.dataClass._progress);
    }

    public void OpenPortal(int No)
    {
        Instantiate(_portals, transform.GetChild(No));
    }

}
