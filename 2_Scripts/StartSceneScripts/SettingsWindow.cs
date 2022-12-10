using DefineUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    #region[사운드 변수]

    [SerializeField] GameObject _soundMenu;
    [SerializeField] Text _soundBgmValText;
    [SerializeField] Text _soundEftValText;
    [SerializeField] Slider _backgroundSlider;
    [SerializeField] Slider _effectSlider;

    float _soundBackgroundVal;
    float _soundEffectVal;

    #endregion

    #region[조작 변수]


    [SerializeField] GameObject _keySetMenu;
    [SerializeField] GameObject _optionParentObj;
    [SerializeField] Text[] _optionsText;
    [SerializeField] Dropdown _optionMoveTypeDropdown;
    List<DataManager.KeyOption> _options;

    #endregion

    #region[기능 변수]

    RectTransform _bgRt;
    Coroutine _nowCoroutin;

    bool _doAnything = false;
    string _inputString = string.Empty;

    #endregion

    void Awake()
    {
        _bgRt = transform.GetChild(0).GetComponent<RectTransform>();
        LoadSoundSet();
        LoadKeySet();
    }
    void Start()
    {
        StartCoroutine(SceneControlManager._instance.WindowGrow(_bgRt));
        OnClickSoundSettingButton();
    }

    private void LateUpdate()
    {
        if (Input.anyKey)
        {
            _inputString = Input.inputString;
        }
    }

    #region[사운드 함수]

    void LoadSoundSet()
    {
        _soundBackgroundVal = DataManager._instance.dataClass.options.soundOptions[(int)eSoundOption.Background]._value;
        _soundEffectVal = DataManager._instance.dataClass.options.soundOptions[(int)eSoundOption.Effect]._value;

        _backgroundSlider.value = _soundBackgroundVal;
        _effectSlider.value = _soundEffectVal;

        string soundBackText = ((int)(_soundBackgroundVal * 100)).ToString();
        _soundBgmValText.text = soundBackText;

        string soundEftText = ((int)(_soundEffectVal * 100)).ToString();
        _soundEftValText.text = soundEftText;
    }

    public void OnChangeSoundBackSliderValue()
    {
        //소수점 둘째짜리까지
        _soundBackgroundVal = (float)System.Math.Truncate(((double)_backgroundSlider.value * 100))/100;

        string soundBackText = ((int)(_soundBackgroundVal * 100)).ToString();
        _soundBgmValText.text = soundBackText;
        AudioManager._instance.SetVolumeBGM(_soundBackgroundVal);
    }
    public void OnChangeSoundEftSliderValue()
    {
        //소수점 둘째짜리까지
        _soundEffectVal = (float)System.Math.Truncate(((double)_effectSlider.value * 100)) / 100;

        string soundEftText = ((int)(_soundEffectVal * 100)).ToString();
        _soundEftValText.text = soundEftText;
    }

    #endregion

    #region[조작 함수]
    
    void LoadKeySet()
    {
        _options = new List<DataManager.KeyOption>();

        DataManager.KeyOption temp = new DataManager.KeyOption();

        temp._option = eKeyOption.Interact;
        temp._keyCode = DataManager._instance.dataClass.options.keyOptions[(int)eKeyOption.Interact]._keyCode;
        _options.Add(temp);

        temp = new DataManager.KeyOption();
        temp._option = eKeyOption.SwapWeapon1;
        temp._keyCode = DataManager._instance.dataClass.options.keyOptions[(int)eKeyOption.SwapWeapon1]._keyCode;
        _options.Add(temp);

        temp = new DataManager.KeyOption();
        temp._option = eKeyOption.SwapWeapon2;
        temp._keyCode = DataManager._instance.dataClass.options.keyOptions[(int)eKeyOption.SwapWeapon2]._keyCode;
        _options.Add(temp);

        _optionsText[(int)eKeyOption.Interact].text = _options[(int)eKeyOption.Interact]._keyCode.ToString().Replace("Alpha", "");
        _optionsText[(int)eKeyOption.SwapWeapon1].text = _options[(int)eKeyOption.SwapWeapon1]._keyCode.ToString().Replace("Alpha", "");
        _optionsText[(int)eKeyOption.SwapWeapon2].text = _options[(int)eKeyOption.SwapWeapon2]._keyCode.ToString().Replace("Alpha", "");

    }
    
    public void OnClickOptionButton()
    {
        if(!_doAnything)
            _nowCoroutin = StartCoroutine(ChangeSettingKey());
        else
        {
            StopCoroutine(_nowCoroutin);
            AllButtonSetWhite();
            _nowCoroutin = StartCoroutine(ChangeSettingKey());
        }
    }
    IEnumerator ChangeSettingKey()
    {
        _doAnything = true;
        Button btn = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        Image img = btn.gameObject.GetComponent<Image>();
        float alphaVal = 1;

        while (true)
        {
            if (_inputString != string.Empty)
                break;

            img.color = new Color(1, 1, 1, alphaVal);

            alphaVal -= 0.01f;
            if (alphaVal <= 0) alphaVal = 1;

            yield return new WaitForEndOfFrame();
        }
        
        btn.transform.GetChild(0).GetComponent<Text>().text = _inputString.ToUpper();
        int idx = btn.transform.parent.GetSiblingIndex();

        char[] keyChar = btn.transform.GetChild(0).GetComponent<Text>().text.ToCharArray();
        if(keyChar.Length > 1) yield return null;

        if(int.TryParse((keyChar[0].ToString()), out int result))
        {
            _options[idx]._keyCode = (KeyCode)48 + result;
        }
        else
        {
            _options[idx]._keyCode = (KeyCode)keyChar[0] + 32;
        }

        _doAnything = false;
        _inputString = string.Empty;
        img.color = new Color(1, 1, 1, 1);


        
        yield return null;
    }

    void AllButtonSetWhite()
    {
        Button[] btns = _optionParentObj.transform.GetComponentsInChildren<Button>();
        for(int i = 0; i < btns.Length; i++)
        {
            btns[i].GetComponent<Image>().color = Color.white;
        }
    }
    #endregion

    #region[기능 함수]

    public void OnClickSoundSettingButton()
    {
        _keySetMenu.SetActive(false);
        _soundMenu.SetActive(true);
    }
    public void OnClickKeySettingButton()
    {
        _soundMenu.SetActive(false);
        _keySetMenu.SetActive(true);
    }
    public void OnClickNoSaveQuitButton()
    {
        StartCoroutine(NoSaveMessageBox("저장하지 않고 종료 하시겠습니까?"));
    }
    public void OnClickCompletionButton()
    {
        StartCoroutine(SaveMessageBox("저장 하시겠습니까?"));
    }
    IEnumerator NoSaveMessageBox(string content)
    {
        GameObject go = Instantiate(WindowManager._instance.GetPrefabs(ePrefabs.MessageBox));
        if(SceneControlManager._instance._curScene == eSceneType.TitleScene)
            go.GetComponent<Canvas>().worldCamera = Camera.main;
        MessageBox msg = go.GetComponent<MessageBox>();
        msg.SetText(content);

        while (!msg._clicked)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (msg._isConfirm)
        {
            AudioManager._instance.LoadVolumeBGM();
            AudioManager._instance.LoadVolumeSFX();
            Destroy(go);
            Destroy(gameObject);
        }
        else
        {
            Destroy(go);
        }
    }
    IEnumerator SaveMessageBox(string content)
    {
        GameObject go = Instantiate(WindowManager._instance.GetPrefabs(ePrefabs.MessageBox));
        if (SceneControlManager._instance._curScene == eSceneType.TitleScene)
            go.GetComponent<Canvas>().worldCamera = Camera.main;
        MessageBox msg = go.GetComponent<MessageBox>();
        msg.SetText(content);

        while (!msg._clicked)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (msg._isConfirm)
        {
            //데이터 저장

            //사운드값
            DataManager._instance.dataClass.options.soundOptions[(int)eSoundOption.Background]._value = _soundBackgroundVal;
            DataManager._instance.dataClass.options.soundOptions[(int)eSoundOption.Effect]._value = _soundEffectVal;
            //키값
            DataManager._instance.dataClass.options.keyOptions[(int)eKeyOption.Interact]._keyCode = _options[(int)eKeyOption.Interact]._keyCode;
            DataManager._instance.dataClass.options.keyOptions[(int)eKeyOption.SwapWeapon1]._keyCode = _options[(int)eKeyOption.SwapWeapon1]._keyCode;
            DataManager._instance.dataClass.options.keyOptions[(int)eKeyOption.SwapWeapon2]._keyCode = _options[(int)eKeyOption.SwapWeapon2]._keyCode;

            DataManager._instance.SaveData();
            Destroy(go);
            AudioManager._instance.LoadVolumeBGM();
            AudioManager._instance.LoadVolumeSFX();
            Destroy(gameObject);
        }
        else
        {
            Destroy(go);
        }
    }

    #endregion

}
