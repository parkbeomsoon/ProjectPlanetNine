using DefineUtils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TpvUI : MonoBehaviour
{
    #region 프로필 창

    [SerializeField] RectTransform _progressRt;
    [SerializeField] RectTransform _hpRt;
    [SerializeField] RawImage _characterImage;
    [SerializeField] Text _nameText;
    [SerializeField] Texture[] _renderTexture;
    private float _progressOffset = 75f;
    private int _hpOffset = 23;

    #endregion

    public bool _isStart = false;
    PlayerController _player;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        StartCoroutine(WaitScene());
        InitData();
    }

    void InitData()
    {
        SetHP(DataManager._instance.dataClass._hp);
        SetProgress(DataManager._instance.dataClass._progress);
        SetName(DataManager._instance.dataClass._name);
        SetAbilityImage(DataManager._instance.dataClass._characterNumber);
    }

    void SetAbilityImage(int no)
    {
        _characterImage.texture = _renderTexture[no];
    }
    void SetName(string text)
    {
        _nameText.text = text;
    }
    void SetHP(int hp)
    {
        _hpRt.sizeDelta = new Vector2(_hpRt.sizeDelta.x, hp * _hpOffset);
    }
    void SetProgress(int progress)
    {
        _progressRt.sizeDelta = new Vector2(_progressRt.sizeDelta.x, progress * _progressOffset);
    }

    IEnumerator WaitScene()
    {
        float _enableTime = 2.5f;
        transform.GetChild(0).gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(_enableTime);
        transform.GetChild(0).gameObject.SetActive(true);
        _isStart = true;
    }

    public void OnArrivedMssionPos(int missionNo)
    {
        string content = string.Empty, missionType = string.Empty;

        switch (missionNo)
        {
            case 0:
                content = "A-0 구역 확보";
                missionType = "적 섬멸";
                break;
            case 1:
                content = "A-1 구역 확보 및 탐사 드론 수리";
                missionType = "드론 수리";
                break;
            case 2:
                content = "A-2 구역 확보 및 지역 조사";
                missionType = "지역 조사";
                break;
        }
        StartCoroutine(StartMissionWnd(content,missionType));
    }

    IEnumerator StartMissionWnd(string content, string missionType)
    {
        GameObject go = Instantiate(WindowManager._instance.GetPrefabs(ePrefabs.MissionWindow),transform);
        MessageBox msg = go.GetComponent<MessageBox>();

        msg.SetText(string.Format($"{content}\n중요 미션 : {missionType}"));

        while (!msg._clicked)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (msg._isConfirm)
        {
            DataManager._instance.dataClass._characterPosZ = _player.transform.position.z - 200;
            DataManager._instance.SaveData();

            Destroy(go);
            SceneControlManager._instance.SetMap(DataManager._instance.dataClass._progress);
            SceneControlManager._instance.StartFpvGameScene();
        }
        else
        {
            Destroy(go);
        }
    }
}
