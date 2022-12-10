using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DefineUtils;

public class CameraRay : MonoBehaviour
{
    [SerializeField] Canvas _msgUI;
    [SerializeField] Text _kindText;
    [SerializeField] Text _keyText;

    public float _distRay = 10f;
    FPSController pc;
    Transform _uiPos;
    MissionManager _missionManager;

    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<FPSController>();
        _uiPos = GameObject.FindGameObjectWithTag("UIPos").transform;
        _msgUI.gameObject.SetActive(false);
        _missionManager = GameObject.FindObjectOfType<MissionManager>();
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, _distRay))
        {
            if (hit.collider.tag.Equals("RepairObj") || hit.collider.tag.Equals("ResearchObj"))
            {
                ObjectStatus stat = hit.collider.gameObject.GetComponent<ObjectStatus>();
                if (!stat._isSuccess)
                {
                    _kindText.text = string.Format($"{stat._objectKind.ToString()}");
                    _keyText.text = string.Format($"{DataManager._instance.dataClass.options.keyOptions[(int)eKeyOption.Interact]._keyCode.ToString()}");
                    _msgUI.gameObject.SetActive(true);
                    if (Input.GetKeyDown(DataManager._instance.dataClass.options.keyOptions[(int)eKeyOption.Interact]._keyCode))
                    {
                        //Debug.Log("수리");
                        GameObject go = Instantiate(WindowManager._instance.GetPrefabs(ePrefabs.MinigameWindow)
                            , _uiPos);
                        StartCoroutine(MinigameWait(go.GetComponent<MinigameWindow>(), hit.collider.gameObject));
                    }
                }
            }
            else
            {
                _msgUI.gameObject.SetActive(false);
            }
        }

    }

    IEnumerator MinigameWait(MinigameWindow mw, GameObject Object)
    {
        pc.MoveOnlyCam();
        mw._wndStart = true;
        mw._interactKind = Object.GetComponent<ObjectStatus>()._objectKind;
        _msgUI.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        while (!mw.minigameEnd)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (mw._isSuccess)
        {
            ObjectStatus os = Object.GetComponent<ObjectStatus>();
            os._isSuccess = true;
            os.gameObject.GetComponent<AudioSource>().Play();
            Object.transform.GetChild(0).gameObject.SetActive(false);
            if (os._objectKind == eInteractKind.수리)
                _missionManager.Clear(2);
            else _missionManager.Clear(3);
        }
        else
        {
            Object.GetComponent<ObjectStatus>()._isSuccess = false;
        }
        _msgUI.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        _msgUI.gameObject.SetActive(false);
        pc.SetMove();
    }
}
