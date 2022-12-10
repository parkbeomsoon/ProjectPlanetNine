using DefineUtils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartWindow : MonoBehaviour
{
    [SerializeField] Button _loadGameButton;

    void Start()
    {
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DataManager._instance.LoadData();
        AudioManager._instance.PlayAudioClipBGM();
        if (DataManager._instance.dataClass._name.Equals(string.Empty))
        {
            _loadGameButton.interactable = false;
        }
    }

    public void OnClickNewStartBtn()
    {
        StartCoroutine(NewGameMessageBox("새로운 게임을 시작하시겠습니까? \n이전 게임기록이 삭제됩니다."));
    }
    public void OnClickSaveStartBtn()
    {
        SceneControlManager._instance.StartTpvGameScene();
    }
    public void OnClickSettingBtn()
    {
        GameObject go = Instantiate(WindowManager._instance.GetPrefabs(ePrefabs.SettingsWindow));
        go.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void OnClickQuitBtn()
    {
        StartCoroutine(QuitMessageBox("종료하시겠습니까?"));
    }

    IEnumerator NewGameMessageBox(string content)
    {
        GameObject go = Instantiate(WindowManager._instance.GetPrefabs(ePrefabs.MessageBox));
        go.GetComponent<Canvas>().worldCamera = Camera.main;
        MessageBox msg = go.GetComponent<MessageBox>();
        msg.SetText(content);

        while (!msg._clicked)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (msg._isConfirm)
        {
            Destroy(go);
            WindowManager._instance.OpenNewCharacterWnd();
        }
        else
        {
            Destroy(go);
        }
    }
    IEnumerator QuitMessageBox(string content)
    {
        GameObject go = Instantiate(WindowManager._instance.GetPrefabs(ePrefabs.MessageBox));
        go.GetComponent<Canvas>().worldCamera = Camera.main;
        MessageBox msg = go.GetComponent<MessageBox>();
        msg.SetText(content);

        while (!msg._clicked)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (msg._isConfirm)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            Destroy(go);
        }
    }
}
